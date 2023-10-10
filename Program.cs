using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

void TransposeMatrix(List<List<int>> matrix, List<List<int>> result, int rowStart, int rowEnd)
{

	for (int i = rowStart; i < rowEnd; i++)
	{
		for (int j = 0; j < matrix[rowStart].Count; j++)
		{
			result[j][i] = matrix[i][j];
		}
	}

}


var rnd = new Random();

var numberOfThreads = new List<int>() { 1, 2, 4, 10 };

int A = rnd.Next(2000, 3000);
int B = rnd.Next(2000, 3000);

var matrix = new List<List<int>>(A);
for(int i = 0; i < A; i++)
{
	matrix.Add(new List<int>(B));
	for(int j = 0;j < B; j++)
	{
		matrix[i].Add(rnd.Next(-1000, 1000));
	}
}

foreach (var threadCount in numberOfThreads)
{
	var threads = new List<Thread>(threadCount);

    int rowsPerThread = A / threadCount;
	var result = new List<List<int>>(B);

	for(int i  = 0; i < B; i++)
	{
		result.Add(new List<int>(A));
		for (int j = 0; j < A; j++)
		{
			result[i].Add(0);
		}
	}

	
    for (int i = 0; i < threadCount; i++)
	{
        int startRow = i * rowsPerThread;
        int endRow = (i == threadCount - 1) ? A : (i + 1) * rowsPerThread;

		threads.Add(new Thread(() =>
		{
			TransposeMatrix(matrix, result, startRow, endRow);
		}));
    }

    var sw = new Stopwatch();
    sw.Start();
	for (int i = 0; i < threadCount; i++)
	{
		threads[i].Start();
	}

    for (int i = 0;i < threads.Count; i++)
	{
		threads[i].Join();
	}
	sw.Stop();

	Console.WriteLine($"Threads: {threadCount} Time: {sw.ElapsedMilliseconds}");

}
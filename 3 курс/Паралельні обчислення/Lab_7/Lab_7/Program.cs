using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int n = 1000;   
        int k = 8;      
        int startVertex = 100; 

        Random rand = new Random();
        
        double[,] graph = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                if (i == j)
                    graph[i, j] = 0;
                else
                {
                    if (rand.NextDouble() < 0.25) 
                    {
                        double weight = rand.Next(5, 100);
                        graph[i, j] = weight;
                        graph[j, i] = weight;
                    }
                    else
                    {
                        graph[i, j] = double.PositiveInfinity;
                        graph[j, i] = double.PositiveInfinity;
                    }
                }
            }
        }

        Console.WriteLine($"Граф {n}×{n}");
        Console.WriteLine($"Стартова вершина a = {startVertex}");
        Console.WriteLine($"Кількість потоків: {k}\n");

        //Послідовна версія
        Stopwatch sw = Stopwatch.StartNew();
        double weightSeq = PrimSequential(graph, n, startVertex);
        sw.Stop();
        double tSeq = sw.Elapsed.TotalMilliseconds;

        Console.WriteLine($"Послідовно: {tSeq:F2} мс");
        Console.WriteLine($"Сума ваг MST = {weightSeq:F2}\n");

        //Паралельна версія
        sw.Restart();
        double weightPar = PrimParallel(graph, n, startVertex, k);
        sw.Stop();
        double tPar = sw.Elapsed.TotalMilliseconds;

        Console.WriteLine($"Паралельно ({k} потоків): {tPar:F2} мс");
        Console.WriteLine($"Сума ваг MST = {weightPar:F2}\n");

        //Прискорення та ефективність 
        double speedup = tSeq / tPar;
        double efficiency = speedup / k * 100;
        Console.WriteLine($"Прискорення (S{k}) = {speedup:F3}");
        Console.WriteLine($"Ефективність (E{k}) = {efficiency:F2}%");
    }

    //Послідовний алгоритм Прима
    static double PrimSequential(double[,] graph, int n, int start)
    {
        bool[] inMST = new bool[n];
        double[] key = new double[n];
        for (int i = 0; i < n; i++)
            key[i] = double.PositiveInfinity;

        key[start] = 0;
        double totalWeight = 0;

        for (int count = 0; count < n - 1; count++)
        {
            int u = MinKey(key, inMST, n);
            if (u == -1) break;

            inMST[u] = true;
            totalWeight += key[u];

            for (int v = 0; v < n; v++)
            {
                if (graph[u, v] < key[v] && !inMST[v])
                    key[v] = graph[u, v];
            }
        }

        return totalWeight;
    }

    //Паралельний алгоритм Прима
    static double PrimParallel(double[,] graph, int n, int start, int numThreads)
    {
        bool[] inMST = new bool[n];
        double[] key = new double[n];
        for (int i = 0; i < n; i++)
            key[i] = double.PositiveInfinity;

        key[start] = 0;
        double totalWeight = 0;

        for (int count = 0; count < n - 1; count++)
        {
            int u = MinKey(key, inMST, n);
            if (u == -1) break;

            inMST[u] = true;
            totalWeight += key[u];

            int chunk = n / numThreads;
            Thread[] threads = new Thread[numThreads];

            for (int t = 0; t < numThreads; t++)
            {
                int startIdx = t * chunk;
                int endIdx = (t == numThreads - 1) ? n : startIdx + chunk;

                threads[t] = new Thread(() =>
                {
                    for (int v = startIdx; v < endIdx; v++)
                    {
                        if (!inMST[v] && graph[u, v] < key[v])
                        {
                            key[v] = graph[u, v];
                        }
                    }
                });
                threads[t].Start();
            }

            foreach (var thread in threads)
                thread.Join();
        }

        return totalWeight;
    }
    
    static int MinKey(double[] key, bool[] inMST, int n)
    {
        double min = double.PositiveInfinity;
        int minIndex = -1;

        for (int v = 0; v < n; v++)
        {
            if (!inMST[v] && key[v] < min)
            {
                min = key[v];
                minIndex = v;
            }
        }
        return minIndex;
    }
}

using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        int n = 50000;     
        int k = 8;        
        int a = 0;        

        var rand = new Random(0);
        
        double[,] graph = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j) graph[i, j] = 0;
                else
                    graph[i, j] = rand.NextDouble() < 0.1 ? rand.Next(5, 100) : double.PositiveInfinity;
            }
        }

        Console.WriteLine($"Граф {n}x{n}");
        Console.WriteLine($"Стартова вершина: a = {a}");
        Console.WriteLine($"Кількість потоків: {k}\n");

        // ======== ПОСЛІДОВНА ВЕРСІЯ ========
        double[] distSeq = new double[n];
        var sw = Stopwatch.StartNew();
        DijkstraSequential(graph, a, distSeq);
        sw.Stop();
        double tSeq = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Послідовно: {tSeq:F2} мс");

        // ======== ПАРАЛЕЛЬНА ВЕРСІЯ ========
        double[] distPar = new double[n];
        sw.Restart();
        DijkstraParallel(graph, a, distPar, k);
        sw.Stop();
        double tPar = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"Паралельно ({k} потоків): {tPar:F2} мс\n");

        // ======== ПРИСКОРЕННЯ ТА ЕФЕКТИВНІСТЬ ========
        double speedup = tSeq / tPar;
        double efficiency = (speedup / k) * 100;
        Console.WriteLine($"Прискорення (S{k}) = {speedup:F3}");
        Console.WriteLine($"Ефективність (E{k}) = {efficiency:F2}%");
    }
    
    // ПОСЛІДОВНИЙ АЛГОРИТМ ДЕЙКСТРИ
    static void DijkstraSequential(double[,] graph, int src, double[] dist)
    {
        int n = graph.GetLength(0);
        bool[] visited = new bool[n];
        for (int i = 0; i < n; i++) { dist[i] = double.PositiveInfinity; visited[i] = false; }
        dist[src] = 0;

        for (int count = 0; count < n - 1; count++)
        {
            int u = MinDistanceSeq(dist, visited);
            if (u == -1) break;
            visited[u] = true;

            for (int v = 0; v < n; v++)
            {
                if (!visited[v] && graph[u, v] != double.PositiveInfinity)
                {
                    double newDist = dist[u] + graph[u, v];
                    if (newDist < dist[v]) dist[v] = newDist;
                }
            }
        }
    }
    
    // ПАРАЛЕЛЬНИЙ АЛГОРИТМ ДЕЙКСТРИ 
    static void DijkstraParallel(double[,] graph, int src, double[] dist, int numThreads)
    {
        int n = graph.GetLength(0);
        bool[] visited = new bool[n];
        for (int i = 0; i < n; i++) { dist[i] = double.PositiveInfinity; visited[i] = false; }
        dist[src] = 0;

        var options = new ParallelOptions { MaxDegreeOfParallelism = numThreads };

        for (int count = 0; count < n - 1; count++)
        {
            int u = ParallelMinDistance(dist, visited, options);
            if (u == -1) break;
            visited[u] = true;
            
            Parallel.For(0, n, options, v =>
            {
                if (!visited[v] && graph[u, v] != double.PositiveInfinity)
                {
                    double newDist = dist[u] + graph[u, v];
                    if (newDist < dist[v]) dist[v] = newDist;
                }
            });
        }
    }
    
    // ПАРАЛЕЛЬНИЙ ПОШУК МІНІМАЛЬНОГО dist[v]
    static int ParallelMinDistance(double[] dist, bool[] visited, ParallelOptions options)
    {
        object lockObj = new object();
        double min = double.PositiveInfinity;
        int minIndex = -1;

        Parallel.For(0, dist.Length, options, () => (localMin: double.PositiveInfinity, localIndex: -1),
            (v, state, local) =>
            {
                if (!visited[v] && dist[v] < local.localMin)
                    local = (dist[v], v);
                return local;
            },
            local =>
            {
                lock (lockObj)
                {
                    if (local.localMin < min)
                    {
                        min = local.localMin;
                        minIndex = local.localIndex;
                    }
                }
            });

        return minIndex;
    }
    
    // ПОСЛІДОВНИЙ ПОШУК МІНІМАЛЬНОГО dist[v]
    static int MinDistanceSeq(double[] dist, bool[] visited)
    {
        double min = double.PositiveInfinity;
        int minIndex = -1;
        for (int v = 0; v < dist.Length; v++)
        {
            if (!visited[v] && dist[v] <= min)
            {
                min = dist[v];
                minIndex = v;
            }
        }
        return minIndex;
    }
}

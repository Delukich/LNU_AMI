using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = Console.ReadLine();
        if (Directory.Exists(path))
        {
            Traverse(path, 0);
        }
        else
        {
            Console.WriteLine("not exit");
        }
    }

    static void Traverse(string path, int indent)
    {
        
    }
}
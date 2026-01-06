//Завдання №1
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введіть кількусть членів послідовності: ");
        int n = int.Parse(Console.ReadLine());
        Console.WriteLine($"Сума членів послідовності: {Fibonachi(n)} ");
    }

    static int Fibonachi(int n)
    {
        if (n <= 0)
            return 0;
        if (n == 1)
            return 1;

        int a = 0;
        int b = 1;
        int sum = 1;
        
        for (int i = 2; i < n; i++)
        {
            int next = a + b;
            sum += next;
            a = b;
            b = next;
        }
        
        return sum;
    }
}
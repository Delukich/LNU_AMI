//Завдання №4
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введіть довжину пароля: ");
        int length = Convert.ToInt32(Console.ReadLine());
        
        string password = GeneratePassword(length);
        Console.WriteLine($"Згенерований пароль: {password}");
    }

    static string GeneratePassword(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                      
        Random random = new Random();
        char[] password = new char[length];
        
        for (int i = 0; i < length; i++)
        {
            password[i] = chars[random.Next(chars.Length)];
        }

        return new string(password);
    }
}
using System;
using System.Collections.Generic;

public interface ITransaction<T>
{
    void Deposit(double amount);
    
    void Transfer(double amount, T account);
}

public class Account<T> : ITransaction<Account<T>>
{
    public string Acc_nubmer { get; }
    public string Owner { get; }
    public double Balance { get; protected set; }

    public Account(string acc_nubmer, string owner, double balance)
    {
        Acc_nubmer = acc_nubmer;
        Owner = owner;
        Balance = balance;
    }

    public void Deposit(double amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сума поповнення має бути більше за нуль");
        }

        Balance += amount;
        Console.WriteLine($"Рахунок поповнено на {amount}");
    }

    public bool Transfer(double amount, Account<T> account)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сума переказу має бути більше за нуль");
        }

        if (Balance < amount)
        {
            Console.WriteLine("Недостатньо коштів для переказу");
            return false;
        }

        Balance -= amount;
        account.Deposit(amount);
        Console.WriteLine($"Гроші переказано {Acc_nubmer} на {account.Acc_nubmer}");
        return true;
    }

    public override string ToString()
    {
        return $"Рахунок: {Acc_nubmer}, Власник: {Owner}, Баланс: {Balance:C}";
    }
}

public class SalaryAccount(string acc_number, string owner, string balance)
{
    public SalaryAccount(string acc_nubmer, string owner, double balance) : base(acc_nubmer, owner, balance)
    {
        
    }
}

public class DepositAccount(string acc_number, string owner, string deposit)
{
    public double InterfaceRate { get; }
    
    public DepositAccount(string acc_number, string owner, string deposit, double interfaceRate) : base(acc_number, owner, deposit)
    {
        InterfaceRate = interfaceRate;
    }
    
    public void AddInterfaceRate()
    {
        var interest = Balance * InterfaceRate;
        Deposit(interest);
    }
}


class Progman
{
    static void Main()
    {
        Account<int> account = new Account<int>("123456789", "Ivan", 1000);
        account.Deposit(100);
        Console.WriteLine(account.Balance);
    }
}

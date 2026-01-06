// Варіант 2: Розробка системи управління книгами у бібліотеці
// Ваша програма повинна містити наступні елементи:
// Створення інтерфейсу ILibraryItem, який містить методи для видачі книги, повернення книги та перевірки стану книги.
// Створення класу Book, який реалізує інтерфейс ILibraryItem та містить інформацію про книгу (наприклад, назва, автор, рік видання тощо).
// Побудова ієрархії класів для користувачів бібліотеки: базовий клас LibraryUser, який містить загальні властивості, та похідні класи, наприклад, Student, Teacher тощо.
// Використання конструкторів для ініціалізації об'єктів класів та деструкторів для звільнення ресурсів.
// Синхронний виклик методів через делегат для видачі та повернення книг.
// Визначення події для сповіщення про зміну статусу книги та організація взаємодії об'єктів через цю подію.
// Розробка класу винятків для обробки помилок у випадку виникнення проблем під час видачі або повернення книги.

using System;

public interface ILibraryItem
{
    void Issue();
    void Return();
    bool CheckStatus();
}

public class Book : ILibraryItem
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    private bool IsIssued;

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
        IsIssued = false;
    }

    public void Issue()
    {
        if (IsIssued)
            throw new BookException($"The book \"{Title}\" is already issued");
        IsIssued = true;
        Console.WriteLine($"The book \"{Title}\" has been issued");
    }

    public void Return()
    {
        if (!IsIssued)
            throw new BookException($"The book \"{Title}\" is already returned");
        IsIssued = false;
        Console.WriteLine($"The book \"{Title}\" has been returned");
    }

    public bool CheckStatus()
    {
        return IsIssued;
    }
}

public class LibraryUser
{
    public string Name { get; set; }

    public LibraryUser(string name)
    {
        Name = name;
    }
}


public class Student : LibraryUser
{
    public int StudentId { get; set; }

    public Student(string name, int studentId) : base(name)
    {
        StudentId = studentId;
    }
}

public class Teacher : LibraryUser
{
    public string Subject { get; set; }

    public Teacher(string name, string subject) : base(name)
    {
        Subject = subject;
    }
}

public delegate void BookStatusHandler(string message);

public class Library
{
    public static event BookStatusHandler BookStatusChanged;

    public static void NotifyStatusChange(string message)
    {
        BookStatusChanged?.Invoke(message);
    }
}

public class BookException : Exception
{
    public BookException(string message) : base(message) { }
}

public class Program
{
    public static void Main(string[] args)
    {
        Library.BookStatusChanged += (message) => Console.WriteLine($"Event: {message}");

        Book book1 = new Book("Zahar Berkut", "Ivan Franko", 1895);
        Book book2 = new Book("Kobzar", "Taras Shevchenko", 1840);

        try
        {
            book1.Issue();
            Library.NotifyStatusChange($"The status of the book \"{book1.Title}\" has changed");
            book1.Return();
            Library.NotifyStatusChange($"The status of the book \"{book1.Title}\" has changed");
            book1.Return(); 
        }
        catch (BookException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
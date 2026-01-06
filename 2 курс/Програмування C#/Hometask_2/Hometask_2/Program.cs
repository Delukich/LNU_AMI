using System;
using System.Collections.Generic;
using System.Linq;

public class Book : IEquatable<Book>
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Book(string title, string author, string isbn, int year)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = year;
    }

    public override string ToString()
    {
        return $"{Title} — {Author} ({PublicationYear}) | ISBN: {ISBN} | Доступна: {IsAvailable}";
    }

    public bool Equals(Book other)
    {
        return other != null && ISBN == other.ISBN;
    }

    public override int GetHashCode()
    {
        return ISBN.GetHashCode();
    }
}

public class BookAlreadyExistsException : Exception
{
    public BookAlreadyExistsException(string message) : base(message) { }
}

public class BookNotFoundException : Exception
{
    public BookNotFoundException(string message) : base(message) { }
}

public class Library
{
    private List<Book> books = new List<Book>();
    private Stack<Book> borrowedBooksStack = new Stack<Book>();
    private Queue<string> waitingList = new Queue<string>();
    private Dictionary<string, string> userBorrowing = new Dictionary<string, string>();

    public void AddBook(Book book)
    {
        if (books.Any(b => b.ISBN == book.ISBN))
            throw new BookAlreadyExistsException($"Книга з ISBN {book.ISBN} вже існує.");
        books.Add(book);
    }

    public void RemoveBook(string isbn)
    {
        var book = books.FirstOrDefault(b => b.ISBN == isbn);
        if (book == null)
            throw new BookNotFoundException($"Книгу з ISBN {isbn} не знайдено.");
        books.Remove(book);
    }

    public Book FindBookByISBN(string isbn)
    {
        return books.FirstOrDefault(b => b.ISBN == isbn);
    }

    public List<Book> FindBooksByAuthor(string author)
    {
        return books.Where(b => b.Author == author).ToList();
    }

    public List<Book> GetAvailableBooks()
    {
        return books.Where(b => b.IsAvailable).ToList();
    }

    public void BorrowBook(string isbn, string userName)
    {
        var book = FindBookByISBN(isbn);
        if (book == null)
            throw new BookNotFoundException($"Книгу з ISBN {isbn} не знайдено.");

        if (book.IsAvailable)
        {
            book.IsAvailable = false;
            userBorrowing[isbn] = userName;
            borrowedBooksStack.Push(book);
            Console.WriteLine($"{userName} отримав книгу: {book.Title}");
        }
        else
        {
            waitingList.Enqueue(isbn);
            Console.WriteLine($"Книга \"{book.Title}\" недоступна. Додано до черги очікування.");
        }
    }

    public void ReturnBook(string isbn)
    {
        var book = FindBookByISBN(isbn);
        if (book == null)
            throw new BookNotFoundException($"Книгу з ISBN {isbn} не знайдено.");

        book.IsAvailable = true;
        userBorrowing.Remove(isbn);
        if (borrowedBooksStack.Any() && borrowedBooksStack.Peek().ISBN == isbn)
            borrowedBooksStack.Pop();

        Console.WriteLine($"Книгу \"{book.Title}\" повернено до бібліотеки.");

        if (waitingList.Any())
        {
            string nextIsbn = waitingList.Dequeue();
            var nextBook = FindBookByISBN(nextIsbn);
            if (nextBook != null && nextBook.IsAvailable)
            {
                nextBook.IsAvailable = false;
                userBorrowing[nextBook.ISBN] = "Наступний користувач (із черги)";
                borrowedBooksStack.Push(nextBook);
                Console.WriteLine($"Книга \"{nextBook.Title}\" автоматично видана наступному в черзі.");
            }
        }
    }

    public void PrintBorrowedStack()
    {
        Console.WriteLine("Стек виданих книг:");
        foreach (var b in borrowedBooksStack)
            Console.WriteLine(b);
    }

    public void PrintWaitingList()
    {
        Console.WriteLine("Черга на книги:");
        foreach (var isbn in waitingList)
            Console.WriteLine(isbn);
    }

    public void PrintUserBorrowing()
    {
        Console.WriteLine("Видані книги користувачам:");
        foreach (var pair in userBorrowing)
            Console.WriteLine($"ISBN: {pair.Key} — Користувач: {pair.Value}");
    }
}

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Library library = new Library();

        try
        {
            library.AddBook(new Book("Кобзар", "Тарас Шевченко", "978-966-111-0001", 1840));
            library.AddBook(new Book("Тіні забутих предків", "Михайло Коцюбинський", "978-966-111-0002", 1911));
            library.AddBook(new Book("Лісова пісня", "Леся Українка", "978-966-111-0003", 1912));
            library.AddBook(new Book("Солодка Даруся", "Марія Матіос", "978-966-111-0004", 2004));
            library.AddBook(new Book("Записки українського самашедшого", "Ліна Костенко", "978-966-111-0005", 2010));

           
            library.AddBook(new Book("Ще одна версія Кобзаря", "Тарас Шевченко", "978-966-111-0001", 2020));
        }
        catch (BookAlreadyExistsException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
        
        var book = library.FindBookByISBN("978-966-111-0003");
        Console.WriteLine($"\nЗнайдено книгу: {book}");
        
        var notFound = library.FindBookByISBN("978-000-000-0000");
        if (notFound == null)
            Console.WriteLine("Книгу з таким ISBN не знайдено.");
        
        library.BorrowBook("978-966-111-0001", "Олена");
        library.BorrowBook("978-966-111-0002", "Іван");
        library.BorrowBook("978-966-111-0001", "Катерина"); // книга вже видана — потрапляє в чергу
        
        library.ReturnBook("978-966-111-0001");
        
        Console.WriteLine("\nДоступні книги:");
        foreach (var b in library.GetAvailableBooks())
            Console.WriteLine(b);
        
        Console.WriteLine();
        library.PrintBorrowedStack();
        
        Console.WriteLine();
        library.PrintWaitingList();
        
        Console.WriteLine();
        library.PrintUserBorrowing();
    }
}

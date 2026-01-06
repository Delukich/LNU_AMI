// В2: Статистика книг бібліотеки
//
//     Розробіть консольну програму, яка:
//
// Асинхронно читає XML-файл зі списком книг.
//
//     Аналізує книги за допомогою LINQ.
//
//     Виводить статистику та дозволяє створити новий XML-файл із відібраними результатами.
//
//     Вимоги до програми:
// Асинхронне зчитування:
//
// Завантажити XML-файл library.xml асинхронно.
//
//     Опрацювання LINQ-запитами:
//
// Вивести список усіх книг, відсортованих за роком публікації (від новіших до старіших).
//
//     Знайти автора з найбільшою кількістю сторінок у сумі серед його книг.
//
//     Підрахувати середню кількість сторінок у жанрі "Роман".
//
//     Збереження результату:
//
// Створити новий XML-файл recent_books.xml, який містить книги, видані після 1930 року.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;


class Program
{
    static async Task Main(string[] args)
    {
        string inputFilePath = @"C:\Програмування C#\Module_3\library.xml";
        string outputFilePath = @"C:\Програмування C#\Module_3\recent_books.xml";
        
        XDocument libraryXml;
        using (var stream = File.OpenRead(inputFilePath))
        {
            libraryXml = await XDocument.LoadAsync(stream, LoadOptions.None, default);
        }
        var books = libraryXml.Descendants("Book")
            .Select(book => new
            {
                Title = book.Element("Title")?.Value,
                Author = book.Element("Author")?.Value,
                Genre = book.Element("Genre")?.Value,
                Year = int.Parse(book.Element("Year")?.Value ?? "0"),
                Pages = int.Parse(book.Element("Pages")?.Value ?? "0")
            })
            .ToList();
        
        var sortedBooks = books.OrderByDescending(b => b.Year);
        Console.WriteLine("Книги відсортовані за роком публікації:");
        foreach (var book in sortedBooks)
        {
            Console.WriteLine($"{book.Title} ({book.Year})");
        }
        
        var authorWithMostPages = books
            .GroupBy(b => b.Author)
            .Select(g => new { Author = g.Key, TotalPages = g.Sum(b => b.Pages) })
            .OrderByDescending(a => a.TotalPages)
            .FirstOrDefault();
        Console.WriteLine($"\nАвтора з найбільшою кількістю сторінок: {authorWithMostPages?.Author} ({authorWithMostPages?.TotalPages} pages)");

        var averagePagesInGenre = books
            .Where(b => b.Genre == "Роман")
            .Average(b => b.Pages);
        Console.WriteLine($"\nСередня кількість сторінок у жанрі \"Роман\": {averagePagesInGenre}");
        
        var recentBooks = books.Where(b => b.Year > 1930);
        var recentBooksXml = new XDocument(
            new XElement("Книги",
                recentBooks.Select(b =>
                    new XElement("Book",
                        new XElement("Title", b.Title),
                        new XElement("Author", b.Author),
                        new XElement("Genre", b.Genre),
                        new XElement("Year", b.Year),
                        new XElement("Pages", b.Pages)
                    )
                )
            )
        ); 
        using (var fileStream = File.Create(outputFilePath))
        {
            await recentBooksXml.SaveAsync(fileStream, SaveOptions.None, default);
        }
    }
}
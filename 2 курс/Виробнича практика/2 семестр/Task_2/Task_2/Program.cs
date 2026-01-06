using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public abstract class Publication
{
    protected string title;
    protected int year;
    protected double price;

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public int Year
    {
        get { return year; }
        set { year = value >= 0 ? value : throw new ArgumentException("Year cannot be negative"); }
    }

    public double Price
    {
        get { return price; }
        set { price = value >= 0 ? value : throw new ArgumentException("Price cannot be negative"); }
    }

    public Publication()
    {
        title = "Unknown";
        year = 2023;
        price = 0.0;
    }

    public Publication(string title, int year, double price)
    {
        this.title = title;
        Year = year;
        Price = price;
    }

    public virtual string GetInfo()
    {
        return $"Title: {title}, Year: {year}, Price: {price}";
    }

    public abstract string GetTypeOfPublication();
}

public sealed class Book : Publication
{
    private string author;
    private int pages;

    public string Author
    {
        get { return author; }
        set { author = value; }
    }

    public int Pages
    {
        get { return pages; }
        set { pages = value >= 0 ? value : throw new ArgumentException("Pages cannot be negative"); }
    }

    public Book() : base()
    {
        author = "Unknown";
        pages = 0;
    }

    public Book(string title, int year, double price, string author, int pages) 
        : base(title, year, price)
    {
        this.author = author;
        Pages = pages;
    }

    public override string GetInfo()
    {
        return $"{base.GetInfo()}, Author: {author}, Pages: {pages}";
    }

    public override string GetTypeOfPublication()
    {
        return "Book";
    }
}

public class Magazine : Publication
{
    private int issueNumber;
    private string publisher;

    public int IssueNumber
    {
        get { return issueNumber; }
        set { issueNumber = value >= 0 ? value : throw new ArgumentException("Issue number cannot be negative"); }
    }

    public string Publisher
    {
        get { return publisher; }
        set { publisher = value; }
    }

    public Magazine() : base()
    {
        issueNumber = 0;
        publisher = "Unknown";
    }

    public Magazine(string title, int year, double price, int issueNumber, string publisher) 
        : base(title, year, price)
    {
        IssueNumber = issueNumber;
        this.publisher = publisher;
    }

    public override string GetInfo()
    {
        return $"{base.GetInfo()}, Issue: {issueNumber}, Publisher: {publisher}";
    }

    public override string GetTypeOfPublication()
    {
        return "Magazine";
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Publication> publications = new List<Publication>();

        string filePath = @"C:\Виробнича практика\Task_2\data.txt";

        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at path: {filePath}");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',');
                if (parts.Length < 5)
                {
                    Console.WriteLine($"Invalid line format: {line}");
                    continue;
                }

                try
                {
                    if (parts[0].Trim() == "Book" && parts.Length >= 6)
                    {
                        publications.Add(new Book(parts[1].Trim(), int.Parse(parts[2].Trim()), 
                            double.Parse(parts[3].Trim(), CultureInfo.InvariantCulture), parts[4].Trim(), 
                            int.Parse(parts[5].Trim())));
                    }
                    else if (parts[0].Trim() == "Magazine" && parts.Length >= 6)
                    {
                        publications.Add(new Magazine(parts[1].Trim(), int.Parse(parts[2].Trim()), 
                            double.Parse(parts[3].Trim(), CultureInfo.InvariantCulture), 
                            int.Parse(parts[4].Trim()), parts[5].Trim()));
                    }
                    else
                    {
                        Console.WriteLine($"Unknown publication type or incorrect format: {line}");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing line '{line}': {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error in data validation for line '{line}': {ex.Message}");
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading file at {filePath}: {ex.Message}");
            return;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access denied to file at {filePath}: {ex.Message}");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }

        if (publications.Count > 0)
        {
            publications.Sort((x, y) => x.Year.CompareTo(y.Year));

            Console.WriteLine("\nSorted Publications by Year:");
            foreach (var pub in publications)
            {
                Console.WriteLine(pub.GetInfo());

                
                if (pub is Book)
                {
                    Console.WriteLine("This is a book publication.");
                }

                
                Magazine magazine = pub as Magazine;
                if (magazine != null)
                {
                    Console.WriteLine("This is a magazine publication.");
                }
            }
        }
        else
        {
            Console.WriteLine("No valid publications loaded from file.");
        }
    }
}
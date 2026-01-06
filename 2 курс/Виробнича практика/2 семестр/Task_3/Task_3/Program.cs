using System;
    using System.Collections;
    using System.IO;
    using System.Globalization;

    public abstract class Publication
    {
        protected string title;
        protected int year;
        protected double price;
    
        public string Title { get => title; set => title = value; }
        public int Year { get => year; set => year = value >= 0 ? value : throw new ArgumentException("Year cannot be negative"); }
        public double Price { get => price; set => price = value >= 0 ? value : throw new ArgumentException("Price cannot be negative"); }
    
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
    
        public virtual string GetInfo() => $"Title: {title}, Year: {year}, Price: {price}";
        public abstract string GetTypeOfPublication();
    }

    public sealed class Book : Publication, IComparable<Book>
    {
        private string author;
        private int pages;
    
        public string Author { get => author; set => author = value; }
        public int Pages { get => pages; set => pages = value >= 0 ? value : throw new ArgumentException("Pages cannot be negative"); }
    
        public Book() : base()
        {
            author = "Unknown";
            pages = 0;
        }
    
        public Book(string title, int year, double price, string author, int pages) : base(title, year, price)
        {
            this.author = author;
            Pages = pages;
        }
    
        public override string GetInfo() => $"{base.GetInfo()}, Author: {author}, Pages: {pages}";
        public override string GetTypeOfPublication() => "Book";
    
        public int CompareTo(Book other)
        {
            if (other == null) return 1;
            return this.Pages.CompareTo(other.Pages);
        }
    }

    public class Magazine : Publication
    {
        private int issueNumber;
        private string publisher;
    
        public int IssueNumber { get => issueNumber; set => issueNumber = value >= 0 ? value : throw new ArgumentException("Issue number cannot be negative"); }
        public string Publisher { get => publisher; set => publisher = value; }
    
        public Magazine() : base()
        {
            issueNumber = 0;
            publisher = "Unknown";
        }
    
        public Magazine(string title, int year, double price, int issueNumber, string publisher) : base(title, year, price)
        {
            IssueNumber = issueNumber;
            this.publisher = publisher;
        }
    
        public override string GetInfo() => $"{base.GetInfo()}, Issue: {issueNumber}, Publisher: {publisher}";
        public override string GetTypeOfPublication() => "Magazine";
    }

    public class MagazineTitleComparer : IComparer<Magazine>
    {
        public int Compare(Magazine x, Magazine y)
        {
            if (x == null || y == null) return 0;
            return x.Title.CompareTo(y.Title);
        }
    }

    public class MagazineIssueComparer : IComparer<Magazine>
    {
        public int Compare(Magazine x, Magazine y)
        {
            if (x == null || y == null) return 0;
            return x.IssueNumber.CompareTo(y.IssueNumber);
        }
    }

    public class PublicationCollection : IEnumerable
    {
        private Publication[] publications;
        public int count;
    
        public PublicationCollection(int capacity)
        {
            publications = new Publication[capacity];
            count = 0;
        }
    
        public void Add(Publication publication)
        {
            if (count < publications.Length)
                publications[count++] = publication;
        }
    
        public void Sort()
        {
            Array.Sort(publications, 0, count, Comparer<Publication>.Create((a, b) => a.Year.CompareTo(b.Year)));
        }
    
        public Publication FindByTitle(string title)
        {
            for (int i = 0; i < count; i++)
                if (publications[i].Title == title)
                    return publications[i];
            return null;
        }
    
        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < count; i++)
                {
                    if (publications[i] is Book book)
                        writer.WriteLine($"Book, {book.Title}, {book.Year}, {book.Price}, {book.Author}, {book.Pages}");
                    else if (publications[i] is Magazine magazine)
                        writer.WriteLine($"Magazine, {magazine.Title}, {magazine.Year}, {magazine.Price}, {magazine.IssueNumber}, {magazine.Publisher}");
                }
            }
        }
    
        public void LoadFromFile(string filePath)
        {
            count = 0;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && count < publications.Length)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length < 6) continue;
    
                    try
                    {
                        if (parts[0].Trim() == "Book")
                        {
                            int yearIndex = parts.Length >= 6 && int.TryParse(parts[2].Trim(), out _) ? 2 : 3;
                            int priceIndex = yearIndex + 1;
                            int authorIndex = yearIndex == 2 ? 4 : 2;
                            int pagesIndex = parts.Length - 1;
    
                            publications[count++] = new Book(
                                parts[1].Trim(),
                                int.Parse(parts[yearIndex].Trim()),
                                double.Parse(parts[priceIndex].Trim(), CultureInfo.InvariantCulture),
                                parts[authorIndex].Trim(),
                                int.Parse(parts[pagesIndex].Trim())
                            );
                        }
                        else if (parts[0].Trim() == "Magazine")
                        {
                            publications[count++] = new Magazine(
                                parts[1].Trim(),
                                int.Parse(parts[2].Trim()),
                                double.Parse(parts[3].Trim(), CultureInfo.InvariantCulture),
                                int.Parse(parts[4].Trim()),
                                parts[5].Trim()
                            );
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing line '{line}': {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Validation error in line '{line}': {ex.Message}");
                    }
                }
            }
        }
    
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return publications[i];
        }
        
        public Publication[] Publications => publications;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            PublicationCollection collection = new PublicationCollection(5);
            
            collection.Add(new Book("Book X", 2023, 25.00, "Author X", 400));
            collection.Add(new Magazine("Mag Y", 2022, 7.99, 5, "Pub Y"));
            collection.Add(new Book("Book Z", 2021, 18.50, "Author Z", 200));
    
            string filePath = @"C:\Виробнича практика\Task_2\data.txt";
            bool running = true;
    
            while (running)
            {
                Console.WriteLine("\n=== Меню ===");
                Console.WriteLine("1. Показати колекцію");
                Console.WriteLine("2. Сортувати колекцію");
                Console.WriteLine("3. Шукати в колекції");
                Console.WriteLine("4. Зберегти в файл");
                Console.WriteLine("5. Завантажити з файлу");
                Console.WriteLine("6. Вийти");
                Console.Write("Виберіть опцію (1-6): ");
    
                string choice = Console.ReadLine();
    
                switch (choice)
                {
                    case "1":
                        ShowCollection(collection);
                        break;
    
                    case "2":
                        SortCollection(collection);
                        break;
    
                    case "3":
                        SearchCollection(collection);
                        break;
    
                    case "4":
                        try
                        {
                            collection.SaveToFile(filePath);
                            Console.WriteLine("Успішно збережено в файл!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка при збереженні: {ex.Message}");
                        }
                        break;
    
                    case "5":
                        try
                        {
                            collection.LoadFromFile(filePath);
                            Console.WriteLine("Успішно завантажено з файлу!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка при завантаженні: {ex.Message}");
                        }
                        break;
    
                    case "6":
                        running = false;
                        break;
    
                    default:
                        Console.WriteLine("Невірний вибір!");
                        break;
                }
            }
        }
    
        static void ShowCollection(PublicationCollection collection)
        {
            Console.WriteLine("\nПоточна колекція:");
            foreach (Publication pub in collection)
            {
                Console.WriteLine(pub.GetInfo());
            }
        }
    
        static void SortCollection(PublicationCollection collection)
        {
            Console.WriteLine("\nВиберіть критерій сортування:");
            Console.WriteLine("1. За роком");
            Console.WriteLine("2. За назвою");
            Console.WriteLine("3. За ціною");
            Console.Write("Ваш вибір (1-3): ");
    
            string sortChoice = Console.ReadLine();
    
            switch (sortChoice)
            {
                case "1":
                    collection.Sort();
                    Console.WriteLine("Відсортовано за роком!");
                    break;
    
                case "2":
                    Array.Sort(collection.Publications, 0, collection.count,
                        Comparer<Publication>.Create((a, b) => a.Title.CompareTo(b.Title)));
                    Console.WriteLine("Відсортовано за назвою!");
                    break;
    
                case "3":
                    Array.Sort(collection.Publications, 0, collection.count,
                        Comparer<Publication>.Create((a, b) => a.Price.CompareTo(b.Price)));
                    Console.WriteLine("Відсортовано за ціною!");
                    break;
    
                default:
                    Console.WriteLine("Невірний вибір, використано сортування за роком!");
                    collection.Sort();
                    break;
            }
    
            ShowCollection(collection);
        }
    
        static void SearchCollection(PublicationCollection collection)
        {
            Console.WriteLine("\nВиберіть критерій пошуку:");
            Console.WriteLine("1. За назвою");
            Console.WriteLine("2. За роком");
            Console.WriteLine("3. За типом (Book/Magazine)");
            Console.Write("Ваш вибір (1-3): ");
    
            string searchChoice = Console.ReadLine();
    
            switch (searchChoice)
            {
                case "1":
                    Console.Write("Введіть назву для пошуку: ");
                    string title = Console.ReadLine();
                    var foundByTitle = collection.FindByTitle(title);
                    Console.WriteLine(foundByTitle != null ? foundByTitle.GetInfo() : "Не знайдено!");
                    break;
    
                case "2":
                    Console.Write("Введіть рік для пошуку: ");
                    if (int.TryParse(Console.ReadLine(), out int year))
                    {
                        bool found = false;
                        foreach (Publication pub in collection)
                        {
                            if (pub.Year == year)
                            {
                                Console.WriteLine(pub.GetInfo());
                                found = true;
                            }
                        }
                        if (!found) Console.WriteLine("Публікацій за цим роком не знайдено!");
                    }
                    else
                    {
                        Console.WriteLine("Невірний формат року!");
                    }
                    break;
    
                case "3":
                    Console.Write("Введіть тип (Book/Magazine): ");
                    string type = Console.ReadLine().Trim().ToLower();
                    bool foundType = false;
                    foreach (Publication pub in collection)
                    {
                        if ((type == "book" && pub is Book) || (type == "magazine" && pub is Magazine))
                        {
                            Console.WriteLine(pub.GetInfo());
                            foundType = true;
                        }
                    }
                    if (!foundType) Console.WriteLine("Публікацій цього типу не знайдено!");
                    break;
    
                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }
        }
    }
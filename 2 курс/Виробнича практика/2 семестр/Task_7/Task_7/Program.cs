using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace PublicationsApp
{
    public class PublicationConverter : JsonConverter<Publication>
    {
        public override Publication Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var root = jsonDoc.RootElement;
                var type = root.GetProperty("Type").GetString();

                return type switch
                {
                    "Book" => JsonSerializer.Deserialize<Book>(root.GetRawText(), options),
                    "Magazine" => JsonSerializer.Deserialize<Magazine>(root.GetRawText(), options),
                    _ => throw new NotSupportedException($"Type '{type}' is not supported.")
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, Publication value, JsonSerializerOptions options)
        {
            var type = value.GetType().Name;
            var json = JsonSerializer.Serialize(value, value.GetType(), options);
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                writer.WriteStartObject();
                writer.WriteString("Type", type);
                foreach (var property in jsonDoc.RootElement.EnumerateObject())
                {
                    property.WriteTo(writer);
                }
                writer.WriteEndObject();
            }
        }
    }

    [Serializable]
    public abstract class Publication
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }

        public abstract void Display();
    }

    [Serializable]
    public class Book : Publication
    {
        public string Author { get; set; }

        public override void Display()
        {
            Console.WriteLine($"[Книга] Назва: {Title}, Автор: {Author}, Видавництво: {Publisher}, Рік: {Year}");
        }
    }

    [Serializable]
    public class Magazine : Publication
    {
        public int IssueNumber { get; set; }

        public override void Display()
        {
            Console.WriteLine($"[Журнал] Назва: {Title}, Номер: {IssueNumber}, Видавництво: {Publisher}, Рік: {Year}");
        }
    }

    class Program
    {
        static List<Publication> ReadPublicationsFromFile(string filePath)
        {
            var publications = new List<Publication>();
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts[0] == "Книга")
                    {
                        publications.Add(new Book
                        {
                            Title = parts[1],
                            Author = parts[2],
                            Publisher = parts[3],
                            Year = int.Parse(parts[4])
                        });
                    }
                    else if (parts[0] == "Журнал")
                    {
                        publications.Add(new Magazine
                        {
                            Title = parts[1],
                            IssueNumber = int.Parse(parts[2]),
                            Publisher = parts[3],
                            Year = int.Parse(parts[4])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при зчитуванні з файлу: {ex.Message}");
            }
            return publications;
        }

        static void SortByYear(List<Publication> publications)
        {
            publications.Sort((p1, p2) => p1.Year.CompareTo(p2.Year));
        }

        static void SerializeBinary(List<Publication> publications, string fileName)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new PublicationConverter() }
                };
                var json = JsonSerializer.Serialize(publications, options);
                File.WriteAllText(fileName, json);
                Console.WriteLine("JSON серіалізація виконана.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка серіалізації (JSON): {ex.Message}");
            }
        }

        static List<Publication> DeserializeBinary(string fileName)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Converters = { new PublicationConverter() }
                };
                var json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<List<Publication>>(json, options) ?? new List<Publication>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка десеріалізації (JSON): {ex.Message}");
                return new List<Publication>();
            }
        }

        static void SerializeXml(List<Publication> publications, string fileName)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Publication>), new[] { typeof(Book), typeof(Magazine) });
                using var fs = new FileStream(fileName, FileMode.Create);
                serializer.Serialize(fs, publications);
                Console.WriteLine("XML серіалізація виконана.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка серіалізації (XML): {ex.Message}");
            }
        }

        static List<Publication> DeserializeXml(string fileName)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Publication>), new[] { typeof(Book), typeof(Magazine) });
                using var fs = new FileStream(fileName, FileMode.Open);
                return (List<Publication>)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка десеріалізації (XML): {ex.Message}");
                return new List<Publication>();
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            Console.SetWindowSize(Math.Min(120, Console.LargestWindowWidth), Console.WindowHeight);

            string inputFile = @"C:\Виробнича практика\Task_7\publications.txt";
            string binaryFile = @"C:\Виробнича практика\Task_7\publications.json";
            string xmlFile = @"C:\Виробнича практика\Task_7\publications.xml";

            var publications = ReadPublicationsFromFile(inputFile);

            Console.WriteLine("Список з файлу:");
            publications.ForEach(p => p.Display());

            SortByYear(publications);

            Console.WriteLine("\nВідсортований список:");
            publications.ForEach(p => p.Display());

            SerializeBinary(publications, binaryFile);
            SerializeXml(publications, xmlFile);

            var binList = DeserializeBinary(binaryFile);
            var xmlList = DeserializeXml(xmlFile);

            Console.WriteLine("\nДесеріалізовано з JSON файлу:");
            binList.ForEach(p => p.Display());

            Console.WriteLine("\nДесеріалізовано з XML файлу:");
            xmlList.ForEach(p => p.Display());
        }
    }
}
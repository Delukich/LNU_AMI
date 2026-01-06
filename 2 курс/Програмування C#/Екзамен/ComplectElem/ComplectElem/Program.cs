using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/*застосування linq, xml i xunit
 • Розробити вказанi типи i статичнi методи, якi реалiзують завдання; статичнi методи, якi реалiзують завдання,
 розробити з використанням linq;
 • Перевiрити функцiональнiсть розроблених класiв i статичних методiв за допомогою xunit, використовуючи Test
 i Class fixtures
 • Виконати поставленi завдання на даних, якi заданi кiлькома xml-файлами, результати статичних методiв
 перевiрити xunit-тестами.
 1. Розробити засоби для облiку реалiзацiї виробiв певних категорiй з набору комплектуючих елементiв.
 Комплектуючий елемент характеризується iдентифiкацiйним номером, назвою i цiною.
 Категорiя характеризується iдентифiкацiйним номером, назвою категорiї.
 Вирiб характеризується заводським номером та iдентифiкацiйним номером категорiї.
 Про кожну операцiю встановлення компонента у вирiб робиться облiковий запис у форматi ==заводський номер
 виробу == дата == iдентифiкацiйний номер компонента == показник готовностi (“так” або “нi”)
 Iнформацiя про вироби, компоненти, категорiї та облiковi операцiї подана окремими xml-файлами.
 2. Вивести:
 (а)xml - файл, в якому для кожного виробу ( вказати його заводський номер i назву категорiї) пораховано
 сумарну вартiсть компонетiв;
(б)xml - файл, в якому отриману у попередньому пунктi iнформацiю доповнити кiлькiстю днiв, якi пiшли на
 виготовлення виробу.*/

namespace ProductionSystem
{
    public class Component
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Product
    {
        public int FactoryNumber { get; set; }
        public int CategoryId { get; set; }
    }

    public class Operation
    {
        public int ProductFactoryNumber { get; set; }
        public DateTime Date { get; set; }
        public int ComponentId { get; set; }
        public string IsCompleted { get; set; } // "так" або "ні"
    }

    public static class DataLoader
    {
        public static List<Component> LoadComponents(string path) =>
            XDocument.Load(path)
                     .Descendants("Component")
                     .Select(c => new Component
                     {
                         Id = (int)c.Element("Id"),
                         Name = (string)c.Element("Name"),
                         Price = (decimal)c.Element("Price")
                     })
                     .ToList();

        public static List<Category> LoadCategories(string path) =>
            XDocument.Load(path)
                     .Descendants("Category")
                     .Select(c => new Category
                     {
                         Id = (int)c.Element("Id"),
                         Name = (string)c.Element("Name")
                     })
                     .ToList();

        public static List<Product> LoadProducts(string path) =>
            XDocument.Load(path)
                     .Descendants("Product")
                     .Select(p => new Product
                     {
                         FactoryNumber = (int)p.Element("FactoryNumber"),
                         CategoryId = (int)p.Element("CategoryId")
                     })
                     .ToList();

        public static List<Operation> LoadOperations(string path) =>
            XDocument.Load(path)
                     .Descendants("Operation")
                     .Select(o => new Operation
                     {
                         ProductFactoryNumber = (int)o.Element("ProductFactoryNumber"),
                         Date = (DateTime)o.Element("Date"),
                         ComponentId = (int)o.Element("ComponentId"),
                         IsCompleted = (string)o.Element("IsCompleted")
                     })
                     .ToList();
    }

    public static class DataGenerator
    {
        public static void GenerateComponentsXml()
        {
            var components = new List<XElement>
            {
                new XElement("Component",
                    new XElement("Id", 1),
                    new XElement("Name", "Процесор"),
                    new XElement("Price", 150.50m)),
                new XElement("Component",
                    new XElement("Id", 2),
                    new XElement("Name", "Пам'ять"),
                    new XElement("Price", 75.25m)),
                new XElement("Component",
                    new XElement("Id", 3),
                    new XElement("Name", "Материнська плата"),
                    new XElement("Price", 200.00m))
            };

            var doc = new XDocument(new XElement("Components", components));
            doc.Save("components.xml");
            Console.WriteLine("components.xml has been generated.");
        }

        public static void GenerateCategoriesXml()
        {
            var categories = new List<XElement>
            {
                new XElement("Category",
                    new XElement("Id", 1),
                    new XElement("Name", "Ноутбуки")),
                new XElement("Category",
                    new XElement("Id", 2),
                    new XElement("Name", "Стаціонарні ПК"))
            };

            var doc = new XDocument(new XElement("Categories", categories));
            doc.Save("categories.xml");
            Console.WriteLine("categories.xml has been generated.");
        }

        public static void GenerateProductsXml()
        {
            var products = new List<XElement>
            {
                new XElement("Product",
                    new XElement("FactoryNumber", 1001),
                    new XElement("CategoryId", 1)),
                new XElement("Product",
                    new XElement("FactoryNumber", 1002),
                    new XElement("CategoryId", 1)),
                new XElement("Product",
                    new XElement("FactoryNumber", 1003),
                    new XElement("CategoryId", 2))
            };

            var doc = new XDocument(new XElement("Products", products));
            doc.Save("products.xml");
            Console.WriteLine("products.xml has been generated.");
        }

        public static void GenerateOperationsXml(string fileName, int set)
        {
            List<XElement> operations;
            if (set == 1)
            {
                operations = new List<XElement>
                {
                    new XElement("Operation",
                        new XElement("ProductFactoryNumber", 1001),
                        new XElement("Date", new DateTime(2025, 1, 10)),
                        new XElement("ComponentId", 1),
                        new XElement("IsCompleted", "так")),
                    new XElement("Operation",
                        new XElement("ProductFactoryNumber", 1001),
                        new XElement("Date", new DateTime(2025, 1, 12)),
                        new XElement("ComponentId", 2),
                        new XElement("IsCompleted", "так")),
                    new XElement("Operation",
                        new XElement("ProductFactoryNumber", 1002),
                        new XElement("Date", new DateTime(2025, 1, 15)),
                        new XElement("ComponentId", 3),
                        new XElement("IsCompleted", "ні"))
                };
            }
            else // set == 2
            {
                operations = new List<XElement>
                {
                    new XElement("Operation",
                        new XElement("ProductFactoryNumber", 1002),
                        new XElement("Date", new DateTime(2025, 1, 16)),
                        new XElement("ComponentId", 1),
                        new XElement("IsCompleted", "так")),
                    new XElement("Operation",
                        new XElement("ProductFactoryNumber", 1003),
                        new XElement("Date", new DateTime(2025, 1, 20)),
                        new XElement("ComponentId", 2),
                        new XElement("IsCompleted", "так"))
                };
            }

            var doc = new XDocument(new XElement("Operations", operations));
            doc.Save(fileName);
            Console.WriteLine($"{fileName} has been generated.");
        }

        public static void GenerateAllXmlFiles()
        {
            GenerateComponentsXml();
            GenerateCategoriesXml();
            GenerateProductsXml();
            GenerateOperationsXml("operations1.xml", 1);
            GenerateOperationsXml("operations2.xml", 2);
        }
    }

    public static class Tasks
    {
        public static XElement TaskA(List<Product> products, List<Category> categories, List<Component> components, List<Operation> operations)
        {
            var result = from p in products
                         join c in categories on p.CategoryId equals c.Id
                         join o in operations on p.FactoryNumber equals o.ProductFactoryNumber into productOperations
                         let totalPrice = (from o in productOperations
                                           join comp in components on o.ComponentId equals comp.Id
                                           where o.IsCompleted == "так"
                                           select comp.Price).Sum()
                         orderby p.FactoryNumber
                         select new XElement("Info",
                             new XElement("FactoryNumber", p.FactoryNumber),
                             new XElement("CategoryName", c.Name),
                             new XElement("TotalPrice", totalPrice)
                         );

            return new XElement("TaskA", result);
        }

        public static XElement TaskB(List<Product> products, List<Category> categories, List<Component> components, List<Operation> operations)
        {
            var result = from p in products
                         join c in categories on p.CategoryId equals c.Id
                         join o in operations on p.FactoryNumber equals o.ProductFactoryNumber into productOperations
                         let totalPrice = (from o in productOperations
                                           join comp in components on o.ComponentId equals comp.Id
                                           where o.IsCompleted == "так"
                                           select comp.Price).Sum()
                         let days = productOperations.Any()
                             ? (productOperations.Max(o => o.Date) - productOperations.Min(o => o.Date)).Days
                             : 0
                         orderby p.FactoryNumber
                         select new XElement("Info",
                             new XElement("FactoryNumber", p.FactoryNumber),
                             new XElement("CategoryName", c.Name),
                             new XElement("TotalPrice", totalPrice),
                             new XElement("DaysSpent", days)
                         );

            return new XElement("TaskB", result);
        }
    }

    class Program
    {
        static void Main()
        {
            DataGenerator.GenerateAllXmlFiles();
            var components = DataLoader.LoadComponents("components.xml");
            var categories = DataLoader.LoadCategories("categories.xml");
            var products = DataLoader.LoadProducts("products.xml");
            var operations = DataLoader.LoadOperations("operations1.xml")
                              .Concat(DataLoader.LoadOperations("operations2.xml"))
                              .ToList();

            var resultA = Tasks.TaskA(products, categories, components, operations);
            resultA.Save("taskA_result.xml");

            var resultB = Tasks.TaskB(products, categories, components, operations);
            resultB.Save("taskB_result.xml");

            Console.WriteLine("Звіт taskA_result.xml і taskB_result.xml збережено.");
        }
    }
}
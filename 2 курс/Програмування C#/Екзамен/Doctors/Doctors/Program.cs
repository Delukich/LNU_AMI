using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/*Розробити вказані типи і статичні методи, які реалізують завдання з використанням ling i повертають результат у вигляді об'єкта XElement;
Перевірити функціональність статичних методів за допомогою xunit, використовуючи Test i Class fixtures
Виконати поставлені завдання на даних, які задані кількома xml-файлами, результати завдань, які поверта-ють статичні методи, перевірити xunit-тестами.
1. Розробити типи для обліку роботи діагностичного центру.
Обстеження характеризується числовим ідентифікатором, ідентифікатором категорії обстеження та ідентифіка тором лікара.
Пацієнт характеризується числовим ідентифікатором і прізвищем.
Лікар характеризується числовим ідентифікатором і прізвищем.
Категорія обстеження характеризується числовим ідентифікатором, назвою та вартістю одного обстеження.
Інформація про обстеження містить дату, числовий ідентифікатор обстеження та ідентифікатор пацієнта.
Інформацію про проведені обстеження подано кількома (не менше 2) окремими хті-файлами.
2. Отримати:
(а)об'єкт типу XElement де для кожної категорії обстежень вказано переліки лікарів, які проводили об-стеження по цій категорії, надаючи також для кожного лікаря кількість обстежень по цій категорії; цей результат також вивести у xml-файл; перелік впорядкувати за назвою обстеження та прізвищами у лексико-графічному порядку;
(6) об'єкт типу XElement де у кожному місяці для кожного лікаря вказати, яка сумарна вартість виконани ним обстежень у кожній з категорій; отриманий результат також вивести у xml-файл; переліки впорядкувати за порядком місяців, прізвищ лікарів та спаданням згаданої вартості обстежень.
*/
namespace Center
{
    public class Reiw
    {
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public int IdDoctor { get; set; }
    }

    public class Pacient
    {
        public int Id { get; set; }
        public string LastName { get; set; }
    }

    public class Doctor
    {
        public int Id { get; set; }
        public string LastName { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdPacient { get; set; }
        public decimal Cost { get; set; }
    }

    public class Info
    {
        public DateTime Date { get; set; }
        public int IdReiw { get; set; }
        public int IdPacient { get; set; }
    }

    public static class DataLoader
    {
        public static List<Reiw> LoadReiws(string path) =>
            XDocument.Load(path)
                     .Descendants("Reiw")
                     .Select(r => new Reiw
                     {
                         Id = (int)r.Element("Id"),
                         IdCategory = (int)r.Element("IdCategory"),
                         IdDoctor = (int)r.Element("IdDoctor")
                     })
                     .ToList();

        public static List<Pacient> LoadPacients(string path) =>
            XDocument.Load(path)
                     .Descendants("Pacient")
                     .Select(p => new Pacient
                     {
                         Id = (int)p.Element("Id"),
                         LastName = (string)p.Element("LastName")
                     })
                     .ToList();

        public static List<Doctor> LoadDoctors(string path) =>
            XDocument.Load(path)
                     .Descendants("Doctor")
                     .Select(d => new Doctor
                     {
                         Id = (int)d.Element("Id"),
                         LastName = (string)d.Element("LastName")
                     })
                     .ToList();

        public static List<Category> LoadCategorys(string path) =>
            XDocument.Load(path)
                     .Descendants("Category")
                     .Select(c => new Category
                     {
                         Id = (int)c.Element("Id"),
                         Name = (string)c.Element("Name"),
                         IdPacient = (int)c.Element("IdPacient"),
                         Cost = (decimal?)c.Element("Cost") ?? 0m
                     })
                     .ToList();

        public static List<Info> LoadInfos(string path) =>
            XDocument.Load(path)
                     .Descendants("info")
                     .Select(i => new Info
                     {
                         Date = (DateTime)i.Element("Date"),
                         IdReiw = (int)i.Element("IdReiw"),
                         IdPacient = (int)i.Element("IdPacient")
                     })
                     .ToList();
    }

    public static class DataGenerator
    {
        public static void GenerateReiwsXml()
        {
            var reiws = new List<XElement>
            {
                new XElement("Reiw", new XElement("Id", 1), new XElement("IdCategory", 3), new XElement("IdDoctor", 1)),
                new XElement("Reiw", new XElement("Id", 2), new XElement("IdCategory", 2), new XElement("IdDoctor", 1)),
                new XElement("Reiw", new XElement("Id", 3), new XElement("IdCategory", 1), new XElement("IdDoctor", 3))
            };

            new XDocument(new XElement("reiws", reiws)).Save("reiws.xml");
        }

        public static void GeneratePacientsXml()
        {
            var pacients = new List<XElement>
            {
                new XElement("Pacient", new XElement("Id", 1), new XElement("LastName", "Lukianchuk")),
                new XElement("Pacient", new XElement("Id", 2), new XElement("LastName", "Patlylo")),
                new XElement("Pacient", new XElement("Id", 3), new XElement("LastName", "Denysuk"))
            };

            new XDocument(new XElement("pacients", pacients)).Save("pacients.xml");
        }

        public static void GenerateDoctorsXml()
        {
            var doctors = new List<XElement>
            {
                new XElement("Doctor", new XElement("Id", 1), new XElement("LastName", "Smith")),
                new XElement("Doctor", new XElement("Id", 2), new XElement("LastName", "Jones")),
                new XElement("Doctor", new XElement("Id", 3), new XElement("LastName", "Taylor"))
            };

            new XDocument(new XElement("doctors", doctors)).Save("doctors.xml");
        }

        public static void GenerateCategorysXml()
        {
            var categories = new List<XElement>
            {
                new XElement("Category", new XElement("Id", 1), new XElement("Name", "Teeth"), new XElement("IdPacient", 3), new XElement("Cost", 500)),
                new XElement("Category", new XElement("Id", 2), new XElement("Name", "Body"), new XElement("IdPacient", 2), new XElement("Cost", 300)),
                new XElement("Category", new XElement("Id", 3), new XElement("Name", "Leg"), new XElement("IdPacient", 3), new XElement("Cost", 700))
            };

            new XDocument(new XElement("categorys", categories)).Save("categorys.xml");
        }

        public static void GenerateExamResultsXml(string fileName, int set)
        {
            var infos = new List<XElement>
            {
                new XElement("info", new XElement("Date", new DateTime(2023, 06, 09)), new XElement("IdReiw", 3), new XElement("IdPacient", 2)),
                new XElement("info", new XElement("Date", new DateTime(2025, 08, 07)), new XElement("IdReiw", 2), new XElement("IdPacient", 2)),
                new XElement("info", new XElement("Date", new DateTime(2025, 10, 10)), new XElement("IdReiw", 1), new XElement("IdPacient", 3))
            };

            new XDocument(new XElement("infos", infos)).Save(fileName);
        }

        public static void GenerateAllXmlFiles()
        {
            GenerateReiwsXml();
            GeneratePacientsXml();
            GenerateDoctorsXml();
            GenerateCategorysXml();
            GenerateExamResultsXml("infos1.xml", 1);
            GenerateExamResultsXml("infos2.xml", 2);
        }
    }

    public static class Tasks
    {
        public static XElement TaskA(List<Reiw> reiws, List<Pacient> pacients, List<Category> categorys, List<Doctor> doctors, List<Info> infos)
        {
            var query = from info in infos
                        join reiw in reiws on info.IdReiw equals reiw.Id
                        join category in categorys on reiw.IdCategory equals category.Id
                        join doctor in doctors on reiw.IdDoctor equals doctor.Id
                        group doctor by new { category.Name, doctor.LastName } into g
                        orderby g.Key.Name, g.Key.LastName
                        group g by g.Key.Name into groupedByCategory
                        select new XElement("Category",
                            new XAttribute("name", groupedByCategory.Key),
                            from doctorGroup in groupedByCategory
                            select new XElement("Doctor",
                                new XAttribute("name", doctorGroup.Key.LastName),
                                new XAttribute("count", doctorGroup.Count())
                            )
                        );

            return new XElement("Categories", query);
        }

        public static XElement TaskB(List<Reiw> reiws, List<Category> categories, List<Doctor> doctors, List<Info> infos)
        {
            var query = from info in infos
                        join reiw in reiws on info.IdReiw equals reiw.Id
                        join category in categories on reiw.IdCategory equals category.Id
                        join doctor in doctors on reiw.IdDoctor equals doctor.Id
                        group category.Cost by new { Year = info.Date.Year, Month = info.Date.Month, Doctor = doctor, Category = category } into g
                        let totalCost = g.Sum()
                        orderby g.Key.Year, g.Key.Month, g.Key.Doctor.LastName, totalCost descending
                        group new { g.Key.Doctor, g.Key.Category, TotalCost = totalCost } by new { g.Key.Year, g.Key.Month } into gByMonth
                        select new XElement("Month",
                            new XAttribute("year", gByMonth.Key.Year),
                            new XAttribute("month", gByMonth.Key.Month),
                            from entry in gByMonth
                            select new XElement("Doctor",
                                new XAttribute("name", entry.Doctor.LastName),
                                new XElement("Category",
                                    new XAttribute("name", entry.Category.Name),
                                    new XAttribute("totalCost", entry.TotalCost)
                                )
                            )
                        );

            return new XElement("MonthlyReports", query);
        }
    }

    class Program
    {
        static void Main()
        {
            DataGenerator.GenerateAllXmlFiles();

            var reiws = DataLoader.LoadReiws("reiws.xml");
            var pacients = DataLoader.LoadPacients("pacients.xml");
            var doctors = DataLoader.LoadDoctors("doctors.xml");
            var categorys = DataLoader.LoadCategorys("categorys.xml");
            var infos = DataLoader.LoadInfos("infos1.xml").Concat(DataLoader.LoadInfos("infos2.xml")).ToList();

            var resultA = Tasks.TaskA(reiws, pacients, categorys, doctors, infos);
            resultA.Save("taskA_result.xml");

            var resultB = Tasks.TaskB(reiws, categorys, doctors, infos);
            resultB.Save("taskB_result.xml");

            Console.WriteLine("Звіти збережено: taskA_result.xml, taskB_result.xml");
        }
    }
}

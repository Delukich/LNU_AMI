using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/*застосування xml, linq i xunit 
Розробити вказані типи і статичні методи, які реалізують завдання; статичні методи, які реалізують завдання, розробити з використанням linq;
Перевірити функціональність розроблених класів і статичних методів за допомогою xunit, використовуючи Test i Class fixtures 
Виконати поставлені завдання на даних, які задані кількома xml-файлами, результати завдань перевірити xunit-тестами. 
1. Розробити типи для підведення підсумків екзаменаційної сесії. 
Студент характеризується номером, прізвищем (string) та ідентифікатором групи. 
Група характеризується числовим ідентифікатором та назвою. 
Предмет характеризується числовим ідентифікатором, назвою та кількістю кредитів. Предмет вважається зда-ним і відповідна кількість кредитів зараховується студентові, якщо він отримав на екзамені більше 50 б. 
Результат екзамену містить номер студента, ідентифікатор предмета та кількість балів (від 0 до 100). Результати екзаменів подано кількома (не менше 2) окремими xml-файлами. 
2. Вивести: 
(a)xml - файл, де для кожної групи вказано список студентів із сумарною кількістю кредитів, зарахованих студентові за усі здані екзамени; переліки впорядкувати за назвою групи та прізвищем студента у лексико-графічному порядку без повторень;
(6) xml - файл, де для кожного предмета вказано студентів, які отримали найкращі результати у своїй групі; перелік впорядкувати за назвою предмета та прізвищем студента у лексико-графічному порядку без повто-рень.
*/
namespace ExamSystem
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
    }

    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
    }

    public class ExamResult
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Score { get; set; }
    }

    public static class DataLoader
    {
        public static List<Student> LoadStudents(string path) =>
            XDocument.Load(path)
                     .Descendants("Student")
                     .Select(s => new Student
                     {
                         Id = (int)s.Element("Id"),
                         LastName = (string)s.Element("LastName"),
                         GroupId = (int)s.Element("GroupId")
                     })
                     .ToList();

        public static List<Group> LoadGroups(string path) =>
            XDocument.Load(path)
                     .Descendants("Group")
                     .Select(g => new Group
                     {
                         Id = (int)g.Element("Id"),
                         Name = (string)g.Element("Name")
                     })
                     .ToList();

        public static List<Subject> LoadSubjects(string path) =>
            XDocument.Load(path)
                     .Descendants("Subject")
                     .Select(s => new Subject
                     {
                         Id = (int)s.Element("Id"),
                         Name = (string)s.Element("Name"),
                         Credits = (int)s.Element("Credits")
                     })
                     .ToList();

        public static List<ExamResult> LoadExamResults(string path) =>
            XDocument.Load(path)
                     .Descendants("Result")
                     .Select(r => new ExamResult
                     {
                         StudentId = (int)r.Element("StudentId"),
                         SubjectId = (int)r.Element("SubjectId"),
                         Score = (int)r.Element("Score")
                     })
                     .ToList();
    }

    public static class DataGenerator
    {
        public static void GenerateStudentsXml()
        {
            var students = new List<XElement>
            {
                new XElement("Student",
                    new XElement("Id", 1),
                    new XElement("LastName", "Коваль"),
                    new XElement("GroupId", 10)),
                new XElement("Student",
                    new XElement("Id", 2),
                    new XElement("LastName", "Шевченко"),
                    new XElement("GroupId", 10)),
                new XElement("Student",
                    new XElement("Id", 3),
                    new XElement("LastName", "Петренко"),
                    new XElement("GroupId", 20))
            };

            var doc = new XDocument(new XElement("Students", students));
            doc.Save("students.xml");
            Console.WriteLine("students.xml has been generated.");
        }

        public static void GenerateGroupsXml()
        {
            var groups = new List<XElement>
            {
                new XElement("Group",
                    new XElement("Id", 10),
                    new XElement("Name", "Група А")),
                new XElement("Group",
                    new XElement("Id", 20),
                    new XElement("Name", "Група Б"))
            };

            var doc = new XDocument(new XElement("Groups", groups));
            doc.Save("groups.xml");
            Console.WriteLine("groups.xml has been generated.");
        }

        public static void GenerateSubjectsXml()
        {
            var subjects = new List<XElement>
            {
                new XElement("Subject",
                    new XElement("Id", 101),
                    new XElement("Name", "Математика"),
                    new XElement("Credits", 5)),
                new XElement("Subject",
                    new XElement("Id", 102),
                    new XElement("Name", "Програмування"),
                    new XElement("Credits", 4))
            };

            var doc = new XDocument(new XElement("Subjects", subjects));
            doc.Save("subjects.xml");
            Console.WriteLine("subjects.xml has been generated.");
        }

        public static void GenerateExamResultsXml(string fileName, int set)
        {
            List<XElement> results;
            if (set == 1)
            {
                results = new List<XElement>
                {
                    new XElement("Result",
                        new XElement("StudentId", 1),
                        new XElement("SubjectId", 101),
                        new XElement("Score", 75)),
                    new XElement("Result",
                        new XElement("StudentId", 2),
                        new XElement("SubjectId", 102),
                        new XElement("Score", 90))
                };
            }
            else // set == 2
            {
                results = new List<XElement>
                {
                    new XElement("Result",
                        new XElement("StudentId", 1),
                        new XElement("SubjectId", 102),
                        new XElement("Score", 60)),
                    new XElement("Result",
                        new XElement("StudentId", 3),
                        new XElement("SubjectId", 101),
                        new XElement("Score", 45))
                };
            }

            var doc = new XDocument(new XElement("ExamResults", results));
            doc.Save(fileName);
            Console.WriteLine($"{fileName} has been generated.");
        }

        public static void GenerateAllXmlFiles()
        {
            GenerateStudentsXml();
            GenerateGroupsXml();
            GenerateSubjectsXml();
            GenerateExamResultsXml("exams1.xml", 1);
            GenerateExamResultsXml("exams2.xml", 2);
        }
    }

    public static class Tasks
    {
        public static XElement TaskA(
            List<Student> students,
            List<Group> groups,
            List<Subject> subjects,
            List<ExamResult> results)
        {
            var passed = results.Where(r => r.Score > 50);

            var studentCredits = from r in passed
                                 join s in students on r.StudentId equals s.Id
                                 join subj in subjects on r.SubjectId equals subj.Id
                                 group subj.Credits by new { s.Id, s.LastName, s.GroupId } into g
                                 select new
                                 {
                                     g.Key.Id,
                                     g.Key.LastName,
                                     g.Key.GroupId,
                                     TotalCredits = g.Sum()
                                 };

            var query = from sc in studentCredits
                        join gr in groups on sc.GroupId equals gr.Id
                        orderby gr.Name, sc.LastName
                        group new XElement("Student",
                            new XElement("Surname", sc.LastName),
                            new XElement("Credits", sc.TotalCredits))
                        by gr.Name into g
                        select new XElement("Group",
                            new XAttribute("Name", g.Key),
                            g);

            return new XElement("CreditsByGroup", query);
        }

        public static XElement TaskB(
            List<Student> students,
            List<Group> groups,
            List<Subject> subjects,
            List<ExamResult> results)
        {
            var query = from subj in subjects
                        join r in results on subj.Id equals r.SubjectId
                        join s in students on r.StudentId equals s.Id
                        group new { s.LastName, s.GroupId, r.Score } by new { subj.Id, subj.Name, s.GroupId } into g
                        let maxScore = g.Max(x => x.Score)
                        from top in g.Where(x => x.Score == maxScore)
                        join gr in groups on g.Key.GroupId equals gr.Id
                        group new XElement("Student",
                            new XElement("Surname", top.LastName),
                            new XElement("Group", gr.Name))
                        by g.Key.Name into resultGroup
                        orderby resultGroup.Key
                        select new XElement("Subject",
                            new XAttribute("Name", resultGroup.Key),
                            resultGroup);

            return new XElement("TopStudentsBySubject", query);
        }
    }

    class Program
    {
        static void Main()
        {
            DataGenerator.GenerateAllXmlFiles();
            var students = DataLoader.LoadStudents("students.xml");
            var groups = DataLoader.LoadGroups("groups.xml");
            var subjects = DataLoader.LoadSubjects("subjects.xml");
            var examResults = DataLoader.LoadExamResults("exams1.xml")
                              .Concat(DataLoader.LoadExamResults("exams2.xml"))
                              .ToList();

            var resultA = Tasks.TaskA(students, groups, subjects, examResults);
            resultA.Save("taskA_result.xml");

            var resultB = Tasks.TaskB(students, groups, subjects, examResults);
            resultB.Save("taskB_result.xml");

            Console.WriteLine("Звіт taskA_result.xml і taskB_result.xml збережено.");
        }
    }
}
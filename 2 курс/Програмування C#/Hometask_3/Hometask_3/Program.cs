using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int CourseId { get; set; }
        public double Grade { get; set; }
    }

    class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructor { get; set; }
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        var students = new List<Student>
        {
            new Student { Id = 1, Name = "Іван", Age = 20, CourseId = 1, Grade = 85 },
            new Student { Id = 2, Name = "Олена", Age = 22, CourseId = 2, Grade = 70 },
            new Student { Id = 3, Name = "Богдан", Age = 19, CourseId = 1, Grade = 92 },
            new Student { Id = 4, Name = "Анна", Age = 21, CourseId = 3, Grade = 78 },
            new Student { Id = 5, Name = "Михайло", Age = 18, CourseId = 2, Grade = 95 },
            new Student { Id = 6, Name = "Софія", Age = 23, CourseId = 1, Grade = 60 },
            new Student { Id = 7, Name = "Дмитро", Age = 25, CourseId = 3, Grade = 88 },
            new Student { Id = 8, Name = "Єва", Age = 17, CourseId = 2, Grade = 75 },
            new Student { Id = 9, Name = "Андрій", Age = 24, CourseId = 1, Grade = 82 },
            new Student { Id = 10, Name = "Ольга", Age = 20, CourseId = 3, Grade = 65 }
        };

        var courses = new List<Course>
        {
            new Course { Id = 1, Title = "Programming", Instructor = "Доц. Мельник" },
            new Course { Id = 2, Title = "Database", Instructor = "Проф. Іваненко" },
            new Course { Id = 3, Title = "Data Structures", Instructor = "Доц. Петренко" }
        };
        
        Console.WriteLine("1. Студенти з оцінкою більше 80:");
        var highGrades = students.Where(s => s.Grade > 80);
        foreach (var s in highGrades)
            Console.WriteLine($"{s.Name} - {s.Grade}");
        Console.WriteLine();
        
        Console.WriteLine("2. Студенти за спаданням оцінки:");
        var sorted = students.OrderByDescending(s => s.Grade);
        foreach (var s in sorted)
            Console.WriteLine($"{s.Name} - {s.Grade}");
        Console.WriteLine();
        
        Console.WriteLine("3. Студенти на курсі 'Programming':");
        var programmingCourseId = courses.First(c => c.Title == "Programming").Id;
        var progStudents = students.Where(s => s.CourseId == programmingCourseId);
        foreach (var s in progStudents)
            Console.WriteLine(s.Name);
        Console.WriteLine();
        
        Console.WriteLine("4. Групування за віком:");
        var groupedByAge = students.GroupBy(s => s.Age);
        foreach (var group in groupedByAge)
        {
            Console.WriteLine($"Вік: {group.Key}");
            foreach (var s in group)
                Console.WriteLine($"- {s.Name}");
        }
        Console.WriteLine();
        
        Console.WriteLine($"{"Студент",-10}  {"Курс",-20}  Викладач");
        Console.WriteLine(new string('-', 50));
        var joined = from s in students
                     join c in courses on s.CourseId equals c.Id
                     select new { s.Name, c.Title, c.Instructor };
        foreach (var item in joined)
            Console.WriteLine($"{item.Name, -10}  {item.Title, -20}  {item.Instructor}");
        Console.WriteLine();
        
        Console.WriteLine("6. Середня оцінка:");
        var avgGrade = students.Average(s => s.Grade);
        Console.WriteLine($"Середня оцінка: {avgGrade:F2}");
        Console.WriteLine();
        
        Console.WriteLine("7. Студенти з оцінкою нижче 60:");
        bool hasFailing = students.Any(s => s.Grade < 60);
        Console.WriteLine(hasFailing ? "Так, є студенти з оцінкою нижче 60." : "Немає студентів з оцінкою нижче 60");
        Console.WriteLine();
        
        Console.WriteLine("8. Курс з найбільшою кількістю студентів:");
        var mostPopularCourse = students
            .GroupBy(s => s.CourseId)
            .OrderByDescending(g => g.Count())
            .First().Key;
        var courseTitle = courses.First(c => c.Id == mostPopularCourse).Title;
        Console.WriteLine($"Найпопулярніший курс: {courseTitle}");
        Console.WriteLine();
        
        Console.WriteLine("9. Топ-3 студенти за оцінкою:");
        var top3 = students.OrderByDescending(s => s.Grade).Take(3);
        foreach (var s in top3)
            Console.WriteLine($"{s.Name} - {s.Grade}");
        Console.WriteLine();
        
        Console.WriteLine("10. Анонімні об'єкти:");
        var anonymous = from s in students
                        join c in courses on s.CourseId equals c.Id
                        select new
                        {
                            StudentName = s.Name,
                            Course = c.Title,
                            Performance = s.Grade >= 90 ? "Excellent" :
                                          s.Grade >= 75 ? "Good" :
                                          s.Grade >= 60 ? "Average" : "Fail"
                        };

        Console.WriteLine($"{"Ім'я",-10}  {"Курс",-20}  Оцінка");
        Console.WriteLine(new string('-', 50));
        foreach (var item in anonymous)
            Console.WriteLine($"{item.StudentName,-10}  {item.Course,-20}  {item.Performance}");
        Console.WriteLine();
        
        Console.WriteLine("11. Пошук студента за ім'ям:");
        string input = Console.ReadLine();
        var searchResults = students.Where(s => s.Name.Contains(input, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine("Результати пошуку:");
        foreach (var s in searchResults)
            Console.WriteLine($"{s.Name} - {s.Grade}");
        Console.WriteLine();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using ExamSystem;

namespace ExamSystem.Tests
{
    public class TaskFixture : IDisposable
    {
        public List<Student> Students { get; }
        public List<Group> Groups { get; }
        public List<Subject> Subjects { get; }
        public List<ExamResult> ExamResults { get; }

        public TaskFixture()
        {
            Students = new List<Student>
            {
                new Student { Id = 1, LastName = "Коваль", GroupId = 10 },
                new Student { Id = 2, LastName = "Шевченко", GroupId = 10 },
                new Student { Id = 3, LastName = "Петренко", GroupId = 20 },
                new Student { Id = 4, LastName = "Іваненко", GroupId = 20 }
            };

            Groups = new List<Group>
            {
                new Group { Id = 10, Name = "Група А" },
                new Group { Id = 20, Name = "Група Б" }
            };

            Subjects = new List<Subject>
            {
                new Subject { Id = 101, Name = "Математика", Credits = 5 },
                new Subject { Id = 102, Name = "Програмування", Credits = 4 },
                new Subject { Id = 103, Name = "Фізика", Credits = 3 }
            };

            ExamResults = new List<ExamResult>
            {
                new ExamResult { StudentId = 1, SubjectId = 101, Score = 75 },
                new ExamResult { StudentId = 2, SubjectId = 102, Score = 90 },
                new ExamResult { StudentId = 1, SubjectId = 102, Score = 60 },
                new ExamResult { StudentId = 3, SubjectId = 101, Score = 45 },
                new ExamResult { StudentId = 4, SubjectId = 101, Score = 95 },
                new ExamResult { StudentId = 4, SubjectId = 103, Score = 80 }
            };
        }

        public void Dispose() { }
    }

    public class TasksTests : IClassFixture<TaskFixture>
    {
        private readonly TaskFixture _fixture;

        public TasksTests(TaskFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public void TaskA_ShouldCalculateCorrectCreditsPerStudent()
        {
            // Act
            var result = Tasks.TaskA(_fixture.Students, _fixture.Groups, _fixture.Subjects, _fixture.ExamResults);
            var groupAStudents = result.Elements("Group").First().Elements("Student").ToList();
            var groupBStudents = result.Elements("Group").Last().Elements("Student").ToList();

            // Assert
            // Group A
            Assert.Equal(2, groupAStudents.Count);

            var koval = groupAStudents.First(s => s.Element("Surname").Value == "Коваль");
            Assert.Equal("9", koval.Element("Credits").Value); // 5 (Math) + 4 (Programming)

            var shevchenko = groupAStudents.First(s => s.Element("Surname").Value == "Шевченко");
            Assert.Equal("4", shevchenko.Element("Credits").Value); // 4 (Programming)

            // Group B
            Assert.Single(groupBStudents);

            var ivanenko = groupBStudents.First(s => s.Element("Surname").Value == "Іваненко");
            Assert.Equal("8", ivanenko.Element("Credits").Value); // 5 (Math) + 3 (Physics)
        }
     
    }
}
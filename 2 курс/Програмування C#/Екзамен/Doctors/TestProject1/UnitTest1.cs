using Center;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Center.Tests
{
    public class TaskFixture : IDisposable
    {
        public List<Reiw> Reiws { get; }
        public List<Pacient> Pacients { get; }
        public List<Doctor> Doctors { get; }
        public List<Category> Categories { get; }
        public List<Info> Infos { get; }

        public TaskFixture()
        {
            // Ініціалізація тестових даних
            Reiws = new List<Reiw>
            {
                new Reiw { Id = 1, IdCategory = 1, IdDoctor = 1 },
                new Reiw { Id = 2, IdCategory = 1, IdDoctor = 1 },
                new Reiw { Id = 3, IdCategory = 2, IdDoctor = 2 },
                new Reiw { Id = 4, IdCategory = 2, IdDoctor = 3 }
            };

            Pacients = new List<Pacient>
            {
                new Pacient { Id = 1, LastName = "Ivanov" },
                new Pacient { Id = 2, LastName = "Petrov" }
            };

            Doctors = new List<Doctor>
            {
                new Doctor { Id = 1, LastName = "Smith" },
                new Doctor { Id = 2, LastName = "Brown" },
                new Doctor { Id = 3, LastName = "Taylor" }
            };

            Categories = new List<Category>
            {
                new Category { Id = 1, Name = "X-Ray", IdPacient = 1 },
                new Category { Id = 2, Name = "MRI", IdPacient = 2 }
            };

            Infos = new List<Info>
            {
                new Info { IdReiw = 1, IdPacient = 1, Date = new DateTime(2025, 6, 1) },
                new Info { IdReiw = 2, IdPacient = 1, Date = new DateTime(2025, 6, 15) },
                new Info { IdReiw = 3, IdPacient = 2, Date = new DateTime(2025, 6, 10) },
                new Info { IdReiw = 4, IdPacient = 2, Date = new DateTime(2025, 7, 10) }
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
        public void TaskA_ShouldReturnDoctorsGroupedByCategoryWithCount()
        {
            // Act
            var result = Tasks.TaskA(_fixture.Reiws, _fixture.Pacients, _fixture.Categories, _fixture.Doctors, _fixture.Infos);
            var categories = result.Elements("Category").ToList();

            // Assert
            Assert.Equal(2, categories.Count);

            var xray = categories.First(c => c.Attribute("name").Value == "X-Ray");
            var xrayDoctors = xray.Elements("Doctor").ToList();
            Assert.Single(xrayDoctors);
            Assert.Equal("Smith", xrayDoctors[0].Attribute("name").Value);
            Assert.Equal("2", xrayDoctors[0].Attribute("count").Value);

            var mri = categories.First(c => c.Attribute("name").Value == "MRI");
            var mriDoctors = mri.Elements("Doctor").ToList();
            Assert.Equal(2, mriDoctors.Count);
        }
    }
}

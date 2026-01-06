using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace StudentManagement
{
    public class Form1 : Form
    {
        private List<Student> students = new List<Student>();
        private string filePath = @"C:\Виробнича практика\Task_9\students.txt";
        private DataGridView dataGridView;
        private MenuStrip menuStrip;
        private FlowLayoutPanel controlPanel;

        public Form1()
        {
            InitializeControls();
            LoadDataFromFile();
        }

        private void InitializeControls()
        {
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(dataGridView);

            menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Завантажити", null, (sender, e) => LoadDataFromFile());
            fileMenu.DropDownItems.Add("Зберегти", null, (sender, e) => SaveDataToFile());
            ToolStripMenuItem editMenu = new ToolStripMenuItem("Редагувати");
            editMenu.DropDownItems.Add("Додати студента", null, (sender, e) => AddStudent());
            editMenu.DropDownItems.Add("Пошук", null, (sender, e) => SearchStudent());
            editMenu.DropDownItems.Add("Сортувати за прізвищем", null, (sender, e) => SortByName());
            editMenu.DropDownItems.Add("Сортувати за середнім балом", null, (sender, e) => SortByGrade());
            menuStrip.Items.AddRange(new[] { fileMenu, editMenu });
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            controlPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true
            };
            Button addButton = new Button { Text = "Додати" };
            addButton.Click += (sender, e) => AddStudent();
            Button searchButton = new Button { Text = "Пошук" };
            searchButton.Click += (sender, e) => SearchStudent();
            controlPanel.Controls.AddRange(new Control[] { addButton, searchButton });
            Controls.Add(controlPanel);
        }

        private void LoadDataFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Файл {filePath} не знайдено. Створіть файл і додайте дані у форматі: Прізвище,Група,СереднійБал.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                students.Clear();
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split(',');
                    if (parts.Length != 3)
                    {
                        MessageBox.Show($"Неправильний формат даних у рядку {i + 1}: {line}. Очікується: Прізвище,Група,СереднійБал.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    if (ValidateStudentData(parts[0], parts[1], parts[2], out Student student))
                    {
                        students.Add(student);
                    }
                    else
                    {
                        MessageBox.Show($"Некоректні дані у рядку {i + 1}: {line}. Перевірте введені дані.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                RefreshGrid();
                MessageBox.Show($"Дані успішно зчитано з файлу. Кількість записів: {students.Count}.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зчитуванні файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveDataToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Student student in students)
                    {
                        writer.WriteLine($"{student.Name},{student.Group},{student.AverageGrade.ToString(CultureInfo.InvariantCulture)}");
                    }
                }
                MessageBox.Show("Дані збережено успішно!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddStudent()
        {
            Form addForm = new Form { Text = "Додати студента", Size = new System.Drawing.Size(300, 200) };
            TextBox nameBox = new TextBox { Top = 20, Left = 100, Width = 150 };
            TextBox groupBox = new TextBox { Top = 50, Left = 100, Width = 150 };
            TextBox gradeBox = new TextBox { Top = 80, Left = 100, Width = 150 };
            Label nameLabel = new Label { Text = "Прізвище:", Top = 20, Left = 20 };
            Label groupLabel = new Label { Text = "Група:", Top = 50, Left = 20 };
            Label gradeLabel = new Label { Text = "Середній бал:", Top = 80, Left = 20 };
            Button saveButton = new Button { Text = "Зберегти", Top = 110, Left = 100 };
            saveButton.Click += (sender, e) =>
            {
                if (ValidateStudentData(nameBox.Text, groupBox.Text, gradeBox.Text, out Student student))
                {
                    students.Add(student);
                    RefreshGrid();
                    addForm.Close();
                }
            };
            addForm.Controls.AddRange(new Control[] { nameBox, groupBox, gradeBox, nameLabel, groupLabel, gradeLabel, saveButton });
            addForm.ShowDialog();
        }

        private bool ValidateStudentData(string name, string group, string grade, out Student student)
        {
            student = null;
            if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit))
            {
                MessageBox.Show("Прізвище не може бути порожнім або містити цифри.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(group) || !group.All(c => char.IsLetterOrDigit(c)))
            {
                MessageBox.Show("Група повинна містити лише літери та цифри.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!double.TryParse(grade, NumberStyles.Any, CultureInfo.InvariantCulture, out double avgGrade) || avgGrade < 0 || avgGrade > 100)
            {
                MessageBox.Show("Середній бал повинен бути числом від 0 до 100.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            student = new Student { Name = name.Trim(), Group = group.Trim(), AverageGrade = avgGrade };
            return true;
        }

        private void SearchStudent()
        {
            Form searchForm = new Form { Text = "Пошук студента", Size = new System.Drawing.Size(300, 150) };
            TextBox searchBox = new TextBox { Top = 20, Left = 100, Width = 150 };
            Label searchLabel = new Label { Text = "Прізвище:", Top = 20, Left = 20 };
            Button searchButton = new Button { Text = "Шукати", Top = 50, Left = 100 };
            searchButton.Click += (sender, e) =>
            {
                string searchName = searchBox.Text.Trim();
                var result = students.Where(st => st.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (result.Any())
                {
                    dataGridView.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Студента не знайдено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                searchForm.Close();
            };
            searchForm.Controls.AddRange(new Control[] { searchBox, searchLabel, searchButton });
            searchForm.ShowDialog();
        }

        private void SortByName()
        {
            students = students.OrderBy(st => st.Name).ToList();
            RefreshGrid();
        }

        private void SortByGrade()
        {
            students = students.OrderByDescending(st => st.AverageGrade).ToList();
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dataGridView.DataSource = null;
            dataGridView.DataSource = students;
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public double AverageGrade { get; set; }
    }
}
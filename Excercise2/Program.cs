using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace Excercise2
{
    internal class Program
    {
        /*
        18. Обчислити середній бал всіх студентів по всіх предметах. Вивести прізвища студентів,
        середній бал яких більший, чим загальний середній бал.
        */
        struct Student
        {
            public string LastName, FirstName, Patronymic, Gender;
            public DateTime BirthDate;
            public string MathMark, PhysicsMark, ITMark;
            public int Scholarship;

            public double GetAverage()
            {
                double m = MathMark == "-" ? 2 : int.Parse(MathMark);
                double p = PhysicsMark == "-" ? 2 : int.Parse(PhysicsMark);
                double i = ITMark == "-" ? 2 : int.Parse(ITMark);
                return (m + p + i) / 3.0;
            }
            public Student(string lastName, string firstName, string patronymic, string gender,
                   DateTime birthDate, string mathMark, string physicsMark, string itMark, int scholarship)
            {
                LastName = lastName;
                FirstName = firstName;
                Patronymic = patronymic;
                Gender = gender;
                BirthDate = birthDate;
                MathMark = mathMark;
                PhysicsMark = physicsMark;
                ITMark = itMark;
                Scholarship = scholarship;
            }
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<Student> students = ReadStudentsFromFile("input.txt");
            double total = students.Average(s => s.GetAverage());

            Console.WriteLine($"Загальний середній бал: {total:F2}");
            Console.Write("Студенти, середній бал яких вищий за середній загальний: ");
            foreach (var student in students)
            {
                if (student.GetAverage() > total)
                {
                    Console.Write(student.LastName+ " ");
                }
            }
        }

        static List<Student> ReadStudentsFromFile(string filePath)
        {
            var students = new List<Student>();
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            int lineNumber = 0;

            foreach (string line in lines)
            {
                lineNumber++;
                try
                {
                    string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 9)
                        throw new Exception("Недостатньо полів.");

                    string lastName = parts[0], firstName = parts[1], patronymic = parts[2], gender = parts[3], birthStr = parts[4],
                           math = parts[5], physics = parts[6], it = parts[7], scholarshipStr = parts[8];

                    //Iм'я
                    string namePattern = @"^[a-zA-Zа-яА-ЯҐґЄєІіЇїʼ'\-]+$";
                    if (!Regex.IsMatch(lastName, namePattern))
                        throw new Exception("Некоректне прізвище.");
                    if (!Regex.IsMatch(firstName, namePattern))
                        throw new Exception("Некоректне ім’я.");
                    if (!Regex.IsMatch(patronymic, namePattern))
                        throw new Exception("Некоректне по батькові.");

                    //Стать
                    if (!"MmМмЧчFfЖж".Contains(gender[0]))
                        throw new Exception("Некоректне значення статі.");
                    
                    //Час
                    DateTime birthDate = DateTime.ParseExact(birthStr, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    // Оцінки
                    ValidateMark(math, "математики");
                    ValidateMark(physics, "фізики");
                    ValidateMark(it, "інформатики");

                    //Стипендія
                    if (!int.TryParse(scholarshipStr, out int scholarship) || (scholarship != 0 && (scholarship < 1234 || scholarship > 4321)))
                        throw new Exception("Некоректна стипендія.");

                    students.Add(new Student(lastName, firstName, patronymic, gender, birthDate, math, physics, it, scholarship));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌Помилка у рядку {lineNumber}: {ex.Message} Тому дані з нього не враховуємо❌");
                }
            }

            return students;
        }
        static void ValidateMark(string mark, string subject)
        {
            if (mark != "-" && (!int.TryParse(mark, out int m) || m < 2 || m > 5))
                throw new Exception($"Некоректна оцінка з {subject}.");
        }
    }
}

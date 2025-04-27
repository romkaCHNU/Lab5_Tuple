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
            public string LastName, FirstName, Patronymic;
            public bool Gender;//Male => true, Female => false
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
            public Student(string lastName, string firstName, string patronymic, bool gender,
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
            Exercise2(students);
            Console.WriteLine("\n" + new string('-',100));
            ExtraExercise1(students);
            Console.WriteLine(new string('-', 100));
            ExtraExercise2(students);
        }
        static void Exercise2(List<Student> students)
        {
            double total = students.Average(s => s.GetAverage());

            Console.WriteLine($"Загальний середній бал: {total:F2}");
            bool AtLeastOne = false;
            
            foreach (var student in students)
            {
                if (student.GetAverage() > total)
                {
                    if (!AtLeastOne)
                    {
                        Console.Write("Студенти, середній бал яких вищий за середній: ");
                        Console.Write(student.LastName);
                        AtLeastOne = true;
                    }
                    else
                    {
                        Console.Write(", " + student.LastName);
                    }
                }
            }
            if (!AtLeastOne)
                Console.WriteLine($"Не знайдено жодного студента, середній бал якого вищий за середній загальний");
        }
        /*
        26.     Вивести прізвища, імена, по батькові студенток жіночої статі, які мають середній бал строго 
        більший, чим середній бал серед студентів чоловічої статі. Ретельно продумати, в яких випадках слід 
        виводити повідомлення про особливі ситуації, які можуть виникати для деяких нетипових вхідних даних.
        */
        static void ExtraExercise1(List<Student> students)
        {
            double MaleTotal = students.Where(s => s.Gender == true).Average(s => s.GetAverage());
            bool AtLeastOne = false;
            foreach (var student in students.Where(s => s.Gender == false))
            {
                if (student.GetAverage() > MaleTotal)
                {
                    Console.WriteLine($"Студентка {student.LastName} має середній бал {student.GetAverage()} вищий за середній бал студентів чоловіків {MaleTotal:F3}");
                    AtLeastOne = true;
                }
            }
            if (!AtLeastOne)
                Console.WriteLine($"Не знайдено жодної студентки з середнім балом вищим за середній бал студентів чоловіків {MaleTotal:F3}");
        }
        /*
        27. Знайти і, якщо є, вивести, погрупувавши, дані про студентів, народжених в один
          день одного року, а також (окремо, теж погрупувавши і теж якщо такі є) студентів
                  (можливо, різних років народження), що мають день народження в один день.
         */
        static void ExtraExercise2(List<Student> students)
        {
            var sameDayAndYear = students.GroupBy(s => s.BirthDate).Where(g => g.Count() > 1);

            var sameDay = students.GroupBy(s => new { s.BirthDate.Day, s.BirthDate.Month }).Where(g => g.Count() > 1);

            Console.WriteLine("\nСтуденти, що народилися в один день і рік:");
            
            if(sameDayAndYear.Count() != 0)
                PrintFullDateGroups(sameDayAndYear);
            else
                Console.WriteLine("Таких нема");

                Console.WriteLine("\n\nСтуденти, що народилися в один день:");
            if (sameDay.Count() != 0)
                PrintDayMonthGroups(sameDay);
            else
                Console.WriteLine("Таких нема");
        }

        static void PrintFullDateGroups(IEnumerable<IGrouping<DateTime, Student>> groups)
        {
            int i = 1;
            foreach (var group in groups)
            {
                Console.WriteLine($" \n{i}. Дата: {group.Key:dd.MM.yyyy}");
                foreach (var student in group)
                    Console.WriteLine($"  -{student.LastName}");
                i++;
            }
        }

        static void PrintDayMonthGroups(IEnumerable<IGrouping<dynamic, Student>> groups)
        {
            int i = 1;
            foreach (var group in groups)
            {
                Console.WriteLine($" \n{i}. День і місяць: {group.Key.Day:D2}.{group.Key.Month:D2}");
                foreach (var student in group)
                    Console.WriteLine($"  -{student.LastName}");
                i++;
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

                    string lastName = parts[0], firstName = parts[1], patronymic = parts[2], genderStr = parts[3], birthStr = parts[4],
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
                    bool gender;
                    if ("MmМмЧчFfЖж".Contains(genderStr[0]))
                        gender = genderStr[0] == 'M' || genderStr[0] == 'М' || genderStr[0] == 'Ч'|| genderStr[0] == 'м' || genderStr[0] == 'm'|| genderStr[0] == 'ч';
                    else
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticTask1
{
    class Task5
    {
        static void Main(string[] args)
        {
            // создаём список работников и заполняем его
            var employees = new List<Employee>
            {
                new Employee("Иванов Иван Иванович"),
                new Employee("Петров Петр Петрович"),
                new Employee("Юлина Юлия Юлиановна"),
                new Employee("Сидоров Сидор Сидорович"),
                new Employee("Павлов Павел Павлович"),
                new Employee("Георгиев Георг Георгиевич")
            };
            // создаём список рабочих дней без выходных и заполняем его
            var availableWorkingDaysOfWeekWithoutWeekends = new List<string>
            {
                DayOfWeek.Monday.ToString(),
                DayOfWeek.Tuesday.ToString(),
                DayOfWeek.Wednesday.ToString(),
                DayOfWeek.Thursday.ToString(),
                DayOfWeek.Friday.ToString()
            };
            // создаём объект планировщика отпусков и передаём ему список работников и список рабочих дней
            var scheduler = new VacationScheduler(employees, availableWorkingDaysOfWeekWithoutWeekends);
            scheduler.ScheduleVacations();
            scheduler.PrintVacations();
        }
    }
    class Employee
    {
        public string Name { get; set; }
        public List<DateTime> Vacations { get; set; }
        // конструктор класса, в качестве параметра принимает ФИО работника
        public Employee(string name)
        {
            Name = name;
            Vacations = new List<DateTime>();
        }
    }
    // лпределяем класс VacationScheduler, который планирует отпуск для заданных сотрудников
    class VacationScheduler
    {
        private List<Employee> Employees;
        private List<string> AvailableWorkingDaysOfWeekWithoutWeekends; 
        private List<DateTime> Vacations;
        private int AllVacationCount;
        // конструктор класса, в качестве параметров принимает список работников и список рабочих дней без выходных
        public VacationScheduler(List<Employee> employees, List<string> availableWorkingDaysOfWeekWithoutWeekends)
        {
            Employees = employees;
            AvailableWorkingDaysOfWeekWithoutWeekends = availableWorkingDaysOfWeekWithoutWeekends;
            Vacations = new List<DateTime>();
            AllVacationCount = 0;
        }
        // метод для планирования отпусков
        public void ScheduleVacations()
        {            
            foreach (var employee in Employees)
            {
                Random gen = new Random();

                // определяем даты начала и окончания отпуска
                DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime end = new DateTime(DateTime.Today.Year, 12, 31);
                // количество отпусков, которые может иметь сотрудник
                int vacationCount = 28;
                // цикл до тех пор, пока все количество отпускных не будет использовано сотрудником
                while (vacationCount > 0)
                {
                    int range = (end - start).Days;

                    // выбираем случайную дату начала отпуска
                    var startDate = start.AddDays(gen.Next(range));

                    // проверяем, будний ли это день, а не выходной
                    if (AvailableWorkingDaysOfWeekWithoutWeekends.Contains(startDate.DayOfWeek.ToString()))
                    {
                        // определяем возможную продолжительность отпуска
                        string[] vacationSteps = { "7", "14" };

                        // случайным образом выбираем продолжительность отпуска
                        int vacIndex = gen.Next(vacationSteps.Length);

                        // определяем дату окончания и разницу между датами начала и окончания отпуска
                        var endDate = new DateTime(DateTime.Now.Year, 12, 31);
                        float difference = 0;

                        // установить дату окончания и разницу в зависимости от продолжительности отпуска
                        if (vacationSteps[vacIndex] == "7")
                        {
                            endDate = startDate.AddDays(7);
                            difference = 7;
                        }
                        if (vacationSteps[vacIndex] == "14")
                        {
                            endDate = startDate.AddDays(14);
                            difference = 14;
                        }                        if (vacationCount <= 7)
                        {
                            endDate = startDate.AddDays(7);
                            difference = 7;
                        }
                        
                        bool CanCreateVacation = false;
                        bool existStart = false;
                        bool existEnd = false;

                        // проверяем, не накладываются ли существующие отпуска друг на друга
                        if (!Vacations.Any(element => element >= startDate && element <= endDate))
                        {
                            if (!Vacations.Any(element => element.AddDays(3) >= startDate && element.AddDays(3) <= endDate))
                            {
                                existStart = employee.Vacations.Any(element => element.AddMonths(1) >= startDate && element <= endDate); existEnd = employee.Vacations.Any(element => element.AddMonths(-1) <= endDate && element >= startDate);

                                if (!existStart && !existEnd)
                                    CanCreateVacation = true;
                            }
                        }

                        //назначаем дни отпуска сотруднику, если они еще не заняты и нет наложения с существующими отпусками
                        if (CanCreateVacation)
                        {
                            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                            {
                                Vacations.Add(dt);
                                employee.Vacations.Add(dt);
                            }
                            AllVacationCount++;
                            vacationCount -= (int)difference;
                        }
                    }
                }
            }
        }
        // вывод отпусков
        public void PrintVacations()
        {
            foreach (var employee in Employees)
            {
                Console.WriteLine("Дни отпуска для " + employee.Name + " : ");
                foreach (var vacationDay in employee.Vacations)
                {
                    Console.WriteLine(vacationDay.ToString());
                }
            }
        }
    }
}
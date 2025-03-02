using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

//Чтобы было меньше вопросов, и чтобы работу было проще проверить и понять, прописываю комментарии

public class Student // Данные каждого ученика
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Rating { get; set; }
}

public class StudentService //класс в котором прописана логика добавления/просмотра и сортировки рейтинга учеников
{
    private string _filePath; // путь до json - файла с рейтингом
    private List<Student> _students; //в этой коллекции будет прописана основная информация об учениках

    public StudentService(string filePath)
    {
        _filePath = filePath;
        LoadData();
    }

    private void LoadData() //метод в котором проверяется есть ли файл в системе, если нет, то создаёт новый
    {
        _students = File.Exists(_filePath) ? JsonConvert.DeserializeObject<List<Student>>(File.ReadAllText(_filePath)) ?? new List<Student>() : new List<Student>(); //для удобства и компактности использовал linq запросы
    }

    public void SaveData() //После изменения коллекции сохраняет данные в json файл
    {
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(_students, Formatting.Indented));
    }

    public void DisplayStudents() // Выводим каждый объект(ученика) в консоль
    {
        foreach (var student in _students)
            Console.WriteLine($"ID: {student.Id}, ФИО: {student.FullName}, Рейтинг: {student.Rating}");
    }

    public void SortByRating(bool ascending = true) // Сортируем рейтинг как по уменьшении так и по возрастании
    {
        _students = ascending ? _students.OrderBy(s => s.Rating).ToList() : _students.OrderByDescending(s => s.Rating).ToList();
        SaveData();
    }

    public void SortByName() // Сортируем рейтинг по фамилии
    {
        _students = _students.OrderBy(s => s.FullName).ToList();
        SaveData();
        
    }

    public void AddStudent(Student student) // добавляем в коллекцией объект(ученика)
    {
        _students.Add(student);
        SaveData();
    }
}
    

class Program
{
    private static StudentService _studentService = new StudentService("students.json"); // Название json файла

    static void Main(string[] args)
    {
        Menu();
    }

    private static void Menu()
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Меню команд"); //Меню команд
            Console.ResetColor();
            Console.WriteLine("1. Просмотреть рейтинговый лист.");
            Console.WriteLine("2. Добавить нового участника.");
            Console.WriteLine("3. Сортировать по баллу в порядке уменьш.");
            Console.WriteLine("4. Сортировать по баллу в порядке возр.");
            Console.WriteLine("5. Сортировать по ФИО.");
            Console.WriteLine("0. Выйти");
            Console.Write("Выберите команду: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "0": 
                    return;
                case "1":
                    ShowStudents();
                    break;
                case "2": 
                    AddStudent(); 
                    break;
                case "3":
                    SortingRatingMin();
                    break;
                case "4":
                    SortingRatingMax();
                    break;
                case "5":
                    SortingNames(); 
                    break;
                case "":
                    break;
                default: // Если пользватель введёт не ту команду, то просим ввести команду заново
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Введите корректную команду!");
                    Console.ResetColor();
                    Console.WriteLine("Чтобы продолжить, нажмите любую клавишу...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }

    private static void ShowStudents() //Для удобства прописываем методы класса в отдельные функции. 
                                       // Так при дальнейшей разработке будет проще
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Рейтинг учеников проекта");
        Console.ResetColor();
        Console.WriteLine();
        _studentService.DisplayStudents();
        Console.ReadKey();
    }

    private static void SortingRatingMin()
    {
        _studentService.SortByRating(false);
        FinishedSort();
    }
    private static void SortingRatingMax()
    {
        _studentService.SortByRating(true);
        FinishedSort();
    }

    private static void SortingNames()
    {
        _studentService.SortByName();
        FinishedSort(); 
    }

    private static void AddStudent()
    {
        try
        {
            Console.Clear();
            Console.Write("Введите ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("ID");
            Console.ResetColor();
            Console.Write(": ");
            int id = Convert.ToInt32(Console.ReadLine());
            
            Console.Write("Введите ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("ФИО");
            Console.ResetColor();
            Console.Write(": ");
            string name = Console.ReadLine();
            
            Console.Write("Введите ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("рейтинговый балл");
            Console.ResetColor();
            Console.Write(": ");
            int rating = Convert.ToInt32(Console.ReadLine());

            _studentService.AddStudent(new Student { Id = id, FullName = name, Rating = rating });
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка ввода данных!");
            Console.ResetColor();
        }
    }

    private static void ShowError() 
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ошибка! Проверьте вводимые данные.");
        Console.ResetColor();
        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }

    private static void FinishedSort()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Сортировка выполнена!");
        Console.ResetColor();
        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }
}

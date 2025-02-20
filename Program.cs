using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.Json;
using Newtonsoft.Json;

namespace Base_KOD;


public class Student{
    public int ID{get; set;}
    public string Name{get; set;}
    public int Points{get; set;}

    public Student( string name, int points){
        Name = name;
        Points = points;
    }

    public void SetId(int id){

    }



    public override string ToString()
    {
        return $"|{ID}| {Name} {Points}";
    }

}


class Program
{
    public static void Menu(){
        bool OpenClose = true;
        while(OpenClose){
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Меню команд");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. Просмотреть рейтинговый лист");
            Console.WriteLine("2. Добавить нового участника");
            Console.WriteLine("3. Удалить участника");
            Console.WriteLine("4. Сортировать рейтинг по баллу");
            Console.WriteLine("5. Сортировать рейтинг по ФИО");
            Console.WriteLine("0. Выйти");
            Console.WriteLine();
            Console.Write("Введите номер команды: ");
            string firstChoiceSTR = Console.ReadLine();
            int firstChoice = 0;
            try{
                firstChoice = int.Parse(firstChoiceSTR);
            }
            catch(FormatException){
                Console.WriteLine("Нет такой команды");
            }
            switch (firstChoice){
                case 0:
                    OpenClose = false;
                    break;

                case 1:
                var everyStudent = ReadAllFromRating();
                    foreach(var stud in everyStudent){
                        Console.WriteLine(stud);
                    }
                    break;
                case 2:
                    Console.Write("Введите ФИО: ");
                    string Fio = Console.ReadLine();
                    Console.WriteLine("Введите баллы: ");
                    int points = Convert.ToInt32(Console.ReadLine());
                    Student newStudent = new Student(Fio, points); 
                    AddToRating(newStudent);
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Введите корректную команду");
                    Console.ResetColor();
                    Console.WriteLine("Чтобы продолжить, нажмите любую клавишу...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }

    public void AddToRating(Student student){
        List<Student> sudents = ReadAllFromRating();

        int LastId = sudents.Last().ID;

        student.SetId(LastId + 1);

        sudents.Add(student);

        string serialStudents = JsonConvert.SerializeObject(sudents);
        File.WriteAllText(DBFilePath, serialStudents);
    }

    public void AddToRating(List<Student> students){
        string serialStudents = JsonConvert.SerializeObject(students);
        File.WriteAllText(DBFilePath, serialStudents);
    }

    public void DeleteStudent(int id){
        List<Student> sudents = ReadAllFromRating();
        Student StudForDel = sudents.FirstOrDefault(u => u.ID == id);
        if (StudForDel != null){
            sudents.Remove(StudForDel);
        }
    }

    public static List<Student> ReadAllFromRating(){
        string JSon = File.ReadAllText(DBFilePath);
        List<Student> currStudents = JsonConvert.DeserializeObject<List<Student>>(JSon);
        return currStudents;
    }
    static string DBFilePath{get; set;}
    static void Main(string[] args)
    {
        string fileDBname = "rating list";
        string FileFolderPath = Path.GetTempPath();
        DBFilePath = FileFolderPath + fileDBname;
        Console.WriteLine(FileFolderPath);
        Menu();
    }
}

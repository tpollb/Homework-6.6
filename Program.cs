using System;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Homework_6._6
{
    class Program
    {
        
        /// <summary>
        /// Метод, считывающий данные из файла, разделённые символом #. Возвращает двумерный массив данных из слов.
        /// </summary>
        /// <param name="filepath">Путь к файлу</param>
        /// <returns></returns>
        static public string[,] ReadFile(string filepath){
            string[,] FalseResult = new string[1, 1];

            if (File.Exists(filepath))
            {
                //количество строк в файле
                int count = File.ReadAllLines(filepath).Length;
                int RowsNumber = 7;
                //возвращаемый результат, матрица слов.
                string[,] result = new string[count, 7];
                //массив строк размерностью count
                string[] arr = new string[count];

                int i = default;
                int j = default;

                using (StreamReader sr = new StreamReader(filepath, System.Text.Encoding.Default))
                {
                    string line;

                    //читаем файл до конца
                    while ((line = sr.ReadLine()) != null)
                    {
                        //получили массив строк
                        arr[i] = line;
                        i++;
                    }
                    //заполняем результат. Двумерный массив из слов)
                    for (i = 0; i < count; i++)
                    {
                        for (j = 0; j < RowsNumber; j++)
                        {
                            string[] words = arr[i].Split(new char[] { '#' });
                            result[i, j] = words[j];
                        }
                    }
                    sr.Close();
                }
                return result;
            } else
            {
                return FalseResult;
            }
        }

        /// <summary>
        /// метод вывода двумерного массива на экран
        /// </summary>
        /// <param name="Source"></param>
        static public void DisplayFile(string[,] Source)
        {
            for (int i = 0; i < Source.GetLength(0); i++)
            {
                for (int j = 0; j < Source.GetLength(1); j++)
                {
                    Console.Write($"{Source[i,j]}\t");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Метод записи строки в файл
        /// </summary>
        /// <param name="filepath">путь к файлу</param>
        /// <param name="Source">строка для записи</param>
        static public void WriteFile(string filepath, string Source)
        {
            using (StreamWriter sw = new StreamWriter(filepath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine($"{Source}");
                sw.Close();
            }
        }

        /// <summary>
        /// Метод геренрации ID. Считывает последний из файла базы данных и увеличивает на 1.
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        static public int IdGenetrate(string path)
        {
            
            string[] str = File.ReadAllLines(path);
            if (str.Length == 0)
            {
                return 1;
            } else
            {
                String last = File.ReadLines(path).Last();
                string[] lastArr = last.Split(new char[] { '#' });
                int id = int.Parse(lastArr[0]);
                id++;
                return id;
            }
        }

        /// <summary>
        /// Метод создания файла, если он не существует
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        static public void CreateFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        static public void CreateDirectory(string path)
        {
            try
            {
                //проверка на существование
                if (Directory.Exists(path))
                {
                    return;
                }

                //пытаемся создать директорию
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось создать директорию: {0}", e.ToString());
            }
        }


        static void Main(string[] args)
        {
            //определение переменных
            string path = @"C:\123\db.txt";
            string directory = @"C:\123";
            bool flag = default;
            string input;
            int FirstInput = 3;
            int id;
            char key;

            Console.SetWindowSize(180, 30);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("6.6 Домашняя работа\nПрограмма справочник «Сотрудники».\n");
            Console.ResetColor();

            //При запуске программы должен быть выбор. введём 1 — вывести данные на экран; введём 2 — заполнить данные и добавить новую запись в конец файла.
            while (flag == false || FirstInput < 1 || FirstInput > 2)
            {
                Console.WriteLine("Введите 1, если хотите вывести данные на экран\nВведите 2, если хотите заполнить данные и добавить новую запись в конец файла.");
                input = Console.ReadLine();
                flag = int.TryParse(input, out FirstInput);
            }

            //если 1 выводим данные
            if (FirstInput == 1)
            {   //контроль наличия файла для вывода
                if (!File.Exists(path))
                {
                    Console.WriteLine("Файл базы данных отсутствует");
                }
                else {
                    Console.ResetColor();
                    DisplayFile(ReadFile(path)); //вывод содержимого файла
                }
                
                                
            } else //если 2 начинаем вводить данные
            {
                CreateDirectory(directory);
                CreateFile(path);

                do
                {
                    id = IdGenetrate(path);
                    string note = id.ToString() + "#";
                    Console.WriteLine($"ID: {id}");

                    string now = DateTime.Now.ToShortDateString();
                    Console.WriteLine($"Дата записи: {now}");
                    note += $"{now} ";

                    now = DateTime.Now.ToShortTimeString();
                    Console.WriteLine($"Время записи: {now}");
                    note += $"{now}#";

                    Console.WriteLine($"Введите ФИО: ");
                    note += $"{Console.ReadLine()}#";

                    //проверка возраста
                    flag = default;
                    int age = default;
                    while (flag == false || age < 1 || age > 120)
                    {
                        Console.WriteLine("Введите возраст (1 - 120): ");
                        input = Console.ReadLine();
                        flag = int.TryParse(input, out age);
                    }
                    now = age.ToString();
                    note += $"{now}#";

                    //проверка роста
                    flag = default;
                    int height = default;
                    while (flag == false || height < 10 || height > 230)
                    {
                        Console.WriteLine("Введите рост (10 - 230): ");
                        input = Console.ReadLine();
                        flag = int.TryParse(input, out height);
                    }
                    now = height.ToString();
                    //Console.WriteLine($"Рост: {now}");
                    note += $"{now}#";

                    //проверка даты рождения
                    DateTime dob;
                    string date;
                    do
                    {
                        Console.WriteLine("Введите дату рождения в формате дд.ММ.гггг (день.месяц.год):");
                        date = Console.ReadLine();
                    }
                    while (!DateTime.TryParseExact(date, "dd.MM.yyyy", null, DateTimeStyles.None, out dob));
                    now = dob.ToShortDateString();
                    note += $"{now}#";

                    Console.WriteLine($"Введите место рождения: ");
                    note += $"{Console.ReadLine()}\t";

                    WriteFile(path, note);
                    Console.Write("Продолжить y/n");
                    key = Console.ReadKey(true).KeyChar;

                } while (char.ToLower(key) == 'y');
            }

            

            Console.ReadKey();
        }
    }
}

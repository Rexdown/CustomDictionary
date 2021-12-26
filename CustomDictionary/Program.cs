using System;
using System.Collections.Generic;
using CustomDictionary;

namespace CustomDictionary {
    class Program
    {
        static void Main(string[] args)
        {
            CustomDictionary<string, int> dict = new CustomDictionary<string, int>(5);

            Console.WriteLine("Добавляем 3 новых элемента");
            
            dict.Add("Audi", 1);
            dict.Add("Mersrdes", 2);
            dict.Add("BMW", 3);

            foreach (var item in dict)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            
            Console.WriteLine("Удаляем один элемент и добавлем еще 2 элемента");

            dict.Remove("Mersrdes");
            dict.Add("Kia", 2);
            dict.Add("Lada", 4);
            
            foreach (var item in dict)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            
            Console.WriteLine("Получаем занение по ключу");

            Console.WriteLine(dict["Kia"]);
            Console.WriteLine(dict["Lada"]);
            Console.WriteLine(dict["BMW"]);
            
            Console.WriteLine("Проверяем существование по ключу");
            
            Console.WriteLine(dict.ContainsKey("Shevrole"));
            Console.WriteLine(dict.ContainsKey("Audi"));
            
            Console.WriteLine("Зададим два новых элемента");
            
            dict["Porshe"] = 8;
            dict["Tesla"] = 10;
            
            Console.WriteLine(dict["Porshe"]);
            Console.WriteLine(dict["Porshe"]);
            
            Console.WriteLine("Выведем сначала все ключи и затем все значения");

            foreach (var item in (List<string>) dict.Keys)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine("\n");
            
            foreach (var item in (List<int>) dict.Values)
            {
                Console.Write($"{item} ");
            }
        }
    }
}
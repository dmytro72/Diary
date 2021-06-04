using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DiaryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string obj = File.ReadAllText("data.json") ?? "[]";
            SortedSet<int> tasks = JsonSerializer.Deserialize<SortedSet<int>>("[]");
            Console.WriteLine($"{tasks.Count}  {obj}");
        }
    }
}

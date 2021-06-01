using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiaryWPFTest.Models
{

    public class Diary
    {
        private SortedSet<Task> tasks;

        public Diary()
        {
            this.tasks = new SortedSet<Task>();
        }

        public void ToFile(string filename)
        {
            File.WriteAllText(filename, JsonSerializer.Serialize(this.tasks));
        }

        public static Diary FromFile(string filename)
        {
            Diary result = new Diary();
            SortedSet<Task> tasks = JsonSerializer.Deserialize<SortedSet<Task>>(File.ReadAllText(filename));
            result.tasks = tasks;
            return result;
        }

        public void AddTask(Task task)
        {
            if (!this.IsOverlapBefore(task) && !this.IsOverlapAfter(task))
            {
                this.tasks.Add(task);
            }
        }

        public bool IsBefore(DateTime start)
        {
            return this.tasks.Count > 0 && this.tasks.Min.Start < start;
        }

        public bool IsAfter(DateTime end)
        {
            return this.tasks.Count > 0 && this.tasks.Max.Start > end;
        }

        public void DeleteTask(Task task)
        {
            this.tasks.Remove(task); 
        }

        public bool IsOverlapBefore(Task task)
        {
            if (this.IsBefore(task.Start))
            {
                Task previous = this.tasks.Where(x => x.Start < task.Start).Last();
                if (previous.End > task.Start)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOverlapAfter(Task task)
        {
            if (this.IsAfter(task.End))
            {
                Task next = this.tasks.Where(x => x.Start > task.Start).First();
                if (task.End > next.Start)
                {
                    return true;
                }
            }
            return false;
        }

        public Task NextTask(DateTime time)
        {
            return this.tasks.Where(x => x.Start > time).First();
        }

        public IEnumerable<Task> TasksAtDay(int year, int month, int day)
        {
            // Need check for validity
            DateTime start = new DateTime(year, month, day, 0, 0, 0);
            DateTime end = start + new TimeSpan(24, 0, 0);
            
            return this.tasks.Where(x => x.Start >= start && x.Start <= end);
        }

        public void PrintTasks()
        {
            foreach (Task task in this.tasks)
            {
                Console.WriteLine($"{task.Start} {task.End} {task.Name} {task.Place}");
            }
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return this.tasks.GetEnumerator();
        }
    }
}
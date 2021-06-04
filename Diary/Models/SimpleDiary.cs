using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace Diary.Models
{

    public class SimpleDiary
    {
        private SortedSet<Task> tasks;

        public SimpleDiary()
        {
            this.tasks = new SortedSet<Task>();
        }

        public void ToFile(string filename)
        {
            File.WriteAllText(filename, JsonSerializer.Serialize(this.tasks));
        }

        public static SimpleDiary FromFile(string filename)
        {
            SimpleDiary result = new SimpleDiary();
            if (File.Exists(filename))
            {
                SortedSet<Task> tasks = JsonSerializer.Deserialize<SortedSet<Task>>(File.ReadAllText(filename));
                result.tasks = tasks;
            }
            return result;
        }

        public void AddTask(Task task)
        {
            this.tasks.Add(task);
        }

        public bool IsBefore(DateTime start)
        {
            return this.tasks.Count > 0 && this.tasks.Min.Start <= start;
        }

        public bool IsAfter(DateTime start)
        {
            return this.tasks.Count > 0 && this.tasks.Max.Start > start;
        }

        public void DeleteTask(Task task)
        {
            this.tasks.Remove(task);
        }

        public bool IsOverlapBefore(DateTime start)
        {
            if (this.IsBefore(start))
            {
                Task previous = this.tasks.Where(x => x.Start <= start).Last();
                if (previous.Start == start || previous.End > start)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOverlapAfter(DateTime start, DateTime end)
        {
            if (this.IsAfter(start))
            {
                Task next = this.tasks.Where(x => x.Start > start).First();
                if (next.Start < end)
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

        public IEnumerable<Task> TasksAtDay(DateTime day)
        {
            DateTime nextDay = day + new TimeSpan(24, 0, 0);

            return this.tasks.Where(x => x.Start >= day && x.Start < nextDay);
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

using System;

namespace Diary.Models
{
    public class Task : IComparable<Task>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }

        public Task(DateTime start, DateTime end, string place, string name)
        {
            Start = start;
            End = end;
            Place = place;
            Name = name;
        }

        public int CompareTo(Task other)
        {
            return this.Start.CompareTo(other.Start);
        }

        public override string ToString()
        {
            return $"{Start:dd.MM.yyyy HH:mm} - {End:HH:mm} {Name} => {Place}";
        }

    }
}

using Diary.Infrastructure.Commands;
using Diary.Models;
using Diary.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public const string FILENAME = "data.json";
        #region Property

        #region DayProperty

        private DateTime _day = DateTime.Now;

        public DateTime Day
        {
            get => _day;
            set => Set(ref _day, value);
        }

        #endregion

        #region StartHoursProperty

        private string _startHours = $"{DateTime.Now:HH}";

        public string StartHours
        {
            get => _startHours;
            set => Set(ref _startHours, value);
        }

        #endregion

        #region StartMinutesProperty

        private string _startMinutes = $"{DateTime.Now:mm}";

        public string StartMinutes
        {
            get => _startMinutes;
            set => Set(ref _startMinutes, value);
        }

        #endregion

        #region DurationHoursProperty

        private string _durationHours = "00";

        public string DurationHours
        {
            get => _durationHours;
            set => Set(ref _durationHours, value);
        }

        #endregion

        #region DurationMinutesProperty

        private string _durationMinutes = "05";

        public string DurationMinutes
        {
            get => _durationMinutes;
            set => Set(ref _durationMinutes, value);
        }

        #endregion

        #region PlaceProperty

        private string _place = "Home";

        public string Place
        {
            get => _place;
            set => Set(ref _place, value);
        }

        #endregion

        #region TaskNameProperty

        private string _taskName;

        public string TaskName
        {
            get => _taskName;
            set => Set(ref _taskName, value);
        }

        #endregion

        #region NewTaskProperty

        private Task _newTask;

        public Task NewTask
        {
            get => _newTask;
            set => Set(ref _newTask, value);
        }

        #endregion

        #region TasksForDay

        private ObservableCollection<Task> _tasksForDay;

        public ObservableCollection<Task> TasksForDay
        {
            get => _tasksForDay;
            set => Set(ref _tasksForDay, value);
        }

        #endregion

        #region SelectedTaskProperty

        private Task _selectedTask;

        public Task SelectedTask
        {
            get => _selectedTask;
            set => Set(ref _selectedTask, value);
        }

        #endregion

        #endregion

        #region Commands

        #region CloseCommand

        public ICommand CloseApplicationCommand
        {
            get;
        }

        private bool CanCloseApplicationCommand(object p) => true;

        private void OnCloseApplicationCommand(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region CreateNewTaskCommand

        public ICommand CreateNewTaskCommand
        {
            get;
        }

        private bool CanCreateNewTaskCommand(object p)
        {
            TimeSpan start = new TimeSpan(Int32.Parse(StartHours), Int32.Parse(StartMinutes), 0);
            TimeSpan duration = new TimeSpan(Int32.Parse(DurationHours), Int32.Parse(DurationMinutes), 0);
            DateTime end = Day + start + duration;
            return end > DateTime.Now;
        }

        private void OnCreateNewTaskCommand(object p)
        {
            DateTime start = Day + new TimeSpan(Int32.Parse(StartHours), Int32.Parse(StartMinutes), 0);
            DateTime end = Day + new TimeSpan(Int32.Parse(DurationHours), Int32.Parse(DurationMinutes), 0);
            NewTask = new Task(start, end, Place, TaskName);
        }

        #endregion

        #region ViewTasksForDay

        public ICommand ViewTasksForDayCommand
        {
            get;
        }

        private bool CanViewTasksForDayCommand(object p) => true;

        private void OnViewTasksForDayCommand(object p)
        {
            TasksForDay = new ObservableCollection<Task>(diary.TasksAtDay(Day));
        }

        #endregion

        #region AddTaskCommand

        public ICommand AddTaskCommand
        {
            get;
        }

        private bool CanAddTaskCommand(object p)
        {
            DateTime start = Day + new TimeSpan(Int32.Parse(StartHours), Int32.Parse(StartMinutes), 0);
            DateTime end = start + new TimeSpan(Int32.Parse(DurationHours), Int32.Parse(DurationMinutes), 0);
            return (end > DateTime.Now)
                && (!diary.IsOverlapBefore(start))
                && (!diary.IsOverlapAfter(start, end));

        }

        private void OnAddTaskCommand(object p)
        {
            DateTime start = Day + new TimeSpan(Int32.Parse(StartHours), Int32.Parse(StartMinutes), 0);
            DateTime end = start + new TimeSpan(Int32.Parse(DurationHours), Int32.Parse(DurationMinutes), 0);
            NewTask = new Task(start, end, Place, TaskName);
            diary.AddTask(NewTask);
            TasksForDay.Add(NewTask);
        }

        #endregion

        #region DeleteTaskCommand

        public ICommand DeleteTaskCommand
        {
            get;
        }

        private bool CanDeleteTaskCommand(object p) => p is Task task && TasksForDay.Contains(task);

        private void OnDeleteTaskCommand(object p)
        {
            if (!(p is Task task)) return;
            var task_index = TasksForDay.IndexOf(task);
            TasksForDay.Remove(task);
            diary.DeleteTask(task);
            if (task_index < TasksForDay.Count)
            {
                SelectedTask = TasksForDay[task_index];
            }
            else
            {
                SelectedTask = null;
            }
        }

        #endregion

        #region SaveFileCommand

        public ICommand SaveFileCommand
        {
            get;
        }

        private bool CanSaveFileCommand(object p) => true;

        private void OnSaveFileCommand(object p)
        {
            diary.ToFile(FILENAME);
        }

        #endregion

        #region MoveTaskCommand

        public ICommand MoveTaskCommand
        {
            get;
        }

        private bool CanMoveTaskCommand(object p) => p is Task task && TasksForDay.Contains(task);

        private void OnMoveTaskCommand(object p)
        {
            if (!(p is Task task)) return;
            var task_index = TasksForDay.IndexOf(task);
            TasksForDay.Remove(task);
            diary.DeleteTask(task);
            StartHours = $"{task.Start:HH}";
            StartMinutes = $"{task.Start:mm}";
            TimeSpan duration = task.End - task.Start;
            DurationHours = $"{duration:hh}";
            DurationMinutes = $"{duration:mm}";
            Place = task.Place;
            TaskName = task.Name;
            if (task_index < TasksForDay.Count)
            {
                SelectedTask = TasksForDay[task_index];
            }
            else
            {
                SelectedTask = null;
            }
            
        }

        #endregion

        #endregion

        private SimpleDiary diary;

        public MainWindowViewModel()
        {
            #region Commands

            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommand, CanCloseApplicationCommand);
            CreateNewTaskCommand = new ActionCommand(OnCreateNewTaskCommand, CanCreateNewTaskCommand);
            ViewTasksForDayCommand = new ActionCommand(OnViewTasksForDayCommand, CanViewTasksForDayCommand);
            AddTaskCommand = new ActionCommand(OnAddTaskCommand, CanAddTaskCommand);
            DeleteTaskCommand = new ActionCommand(OnDeleteTaskCommand, CanDeleteTaskCommand);
            SaveFileCommand = new ActionCommand(OnSaveFileCommand, CanSaveFileCommand);
            MoveTaskCommand = new ActionCommand(OnMoveTaskCommand, CanMoveTaskCommand);

            #endregion

            diary = SimpleDiary.FromFile(FILENAME);
            TasksForDay = new ObservableCollection<Task>(diary.TasksAtDay(DateTime.Today));
        }
    }
}

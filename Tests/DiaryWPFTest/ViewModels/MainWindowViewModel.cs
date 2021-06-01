using DiaryWPFTest.Infrastructure.Commands;
using DiaryWPFTest.Models;
using DiaryWPFTest.ViewModels.Base;
using System;
using System.Windows;
using System.Windows.Input;

namespace DiaryWPFTest.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Property

        #region WindowTitleProperty

        private string _Title = "Анализ статистики";

        public string Title
        {
            get => _Title;
            //set
            //{
            //if (Equals(_Title, value))
            //{
            //    return;
            //}
            //_Title = value;
            //OnPropertyChanged();
            //    Set(ref _Title, value);
            //}
            set => Set(ref _Title, value);
        }

        #endregion

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

        #endregion

        public MainWindowViewModel()
        {
            #region Commands

            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommand, CanCloseApplicationCommand);
            CreateNewTaskCommand = new ActionCommand(OnCreateNewTaskCommand, CanCreateNewTaskCommand);

            #endregion

        }
    }
}

using DiaryWPFTest.ViewModels.Base;
using System;

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

        #region StartHourProperty

        private string _startHour = $"{DateTime.Now:HH}";

        public string StartHour
        {
            get => _startHour;
            set => Set(ref _startHour, value);
        }

        #endregion

        #region StartMinuteProperty

        private string _startMinute = $"{DateTime.Now:mm}";

        public string StartMinute
        {
            get => _startMinute;
            set => Set(ref _startMinute, value);
        }

        #endregion

        #endregion
    }
}

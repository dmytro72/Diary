using DiaryWPFTest.ViewModels.Base;

namespace DiaryWPFTest.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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
    }
}

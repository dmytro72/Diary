using Diary.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Diary
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TasksCollectionFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            if (!(e.Item is Task task)) return;
            if (task.Place is null) return;
            if (task.Name is null) return;

            var filter_text = TaskPlaceFilterText.Text;
            if (filter_text.Length == 0) return;

            if (task.Place.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if (task.Name.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
        }

        private void OnTasksFilterTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text_box = (TextBox)sender;
            var collection = (CollectionViewSource)text_box.FindResource("TasksCollection");
            collection.View.Refresh();
        }
    }
}

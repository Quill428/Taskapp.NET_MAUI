using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Models
{

    //public ObservableCollection<Notes> Tasks { get; set; }

    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Name { get; set; }


        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));// assuming Category implements INotifyPropertyChanged
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private int _taskCount;
        public int TaskCount
        {
            get => _taskCount;
            set
            {
                if (_taskCount != value)
                {
                    _taskCount = value;
                    OnPropertyChanged(nameof(TaskCount));
                }
            }
        }

        //private string _taskCountText;
        //    public string TaskCountText
        //    {
        //        get => _taskCountText;
        //        set
        //        {
        //            if (_taskCountText != value)
        //            {
        //                _taskCountText = value;
        //                OnPropertyChanged();
        //            }
        //        }
        //    }

        //    public CategoryViewModel()
        //    {
        //        Tasks = new ObservableCollection<Notes>();
        //        Tasks.CollectionChanged += (s, e) =>
        //        {
        //            UpdateTaskCount();
        //        };
        //        UpdateTaskCount();
        //    }

        //    private void UpdateTaskCount()
        //    {
        //        TaskCountText = $"Tasks: {Tasks.Count}";
        //    }

        //    public event PropertyChangedEventHandler PropertyChanged;
        //    protected void OnPropertyChanged([CallerMemberName] string name = null)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //    }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Models
{

    public class NotesGroup : ObservableCollection<Notes>, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int TaskCount { get; set; }
        private double progress;
        public double Progress
        {
            get => progress;
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Progress)));
                }

            }
        }
        //    get
        //    {
        //        if (Count == 0) return 0;
        //        return (double)this.Count(n => n.IsCompleted) / Count; // 0.0 - 1.0
        //    }
        //}

            //public string ProgressText => $"{(int)(Progress * 100)}%";



        public NotesGroup(string categoryName, IEnumerable<Notes> notes) : base(notes)
        {
            CategoryName = categoryName;
        }
    }
 
}

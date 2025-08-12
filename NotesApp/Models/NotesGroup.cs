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
        //Assigns a random static colour from the following list 
        private static readonly Dictionary<string, string> CategoryColorMap = new();
        private static readonly List<string> CategoryColors = new()
        {
            "#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7",
            "#DDA0DD", "#98D8C8", "#F7DC6F", "#BB8FCE", "#85C1E9",
            "#F8C471", "#82E0AA", "#F1948A", "#85C1E9", "#D2B4DE"
        };

        private static int colorIndex = 0;

        public int Id { get; set; }
        public string CategoryName { get; set; }

        // Task count properties
        public int TotalTasks => Count;
        public int CompletedTasks => this.Count(n => n.IsCompleted);
        public int RemainingTasks => TotalTasks - CompletedTasks;

        // Progress calculation (0.0 to 1.0 for ProgressBar)
        private double progress;
        public double Progress
        {
            get => progress;
            set
            {
                if (Math.Abs(progress - value) > 0.001) // Use small epsilon for double comparison
                {
                    progress = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Progress)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProgressPercentage)));
                }
            }
        }

        // Progress as percentage for display (0 to 100)
        public int ProgressPercentage => (int)(Progress * 100);

        // Color for this category
        public string CategoryColor { get; private set; }

        // Task count text for display
        public string TaskCountText => $"{CompletedTasks}/{TotalTasks} tasks";
        public string RemainingTasksText => $"{RemainingTasks} remaining";

        public NotesGroup(string categoryName, IEnumerable<Notes> notes) : base(notes)
        {
            CategoryName = categoryName;

            // Assign a consistent colour to this category
            if (!CategoryColorMap.ContainsKey(categoryName))
            {
                //assigning the colour
                CategoryColorMap[categoryName] = CategoryColors[colorIndex % CategoryColors.Count];
                colorIndex++;
            }
            CategoryColor = CategoryColorMap[categoryName];

            // Calculate initial progress
            UpdateProgress();
        }

        // Method to recalculate progress and notify UI
        public void UpdateProgress()
        {
            if (Count == 0)
            {
                Progress = 0;
            }
            else
            {
                // Progress as decimal (0.0 to 1.0)
                Progress = (double)CompletedTasks / TotalTasks;
            }

            // Notify UI of all property changes
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TotalTasks)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(CompletedTasks)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(RemainingTasks)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TaskCountText)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(RemainingTasksText)));

            Console.WriteLine($"Category: {CategoryName}, Total: {TotalTasks}, Completed: {CompletedTasks}, Progress: {Progress:F2} ({ProgressPercentage}%)");
        }
    }
}

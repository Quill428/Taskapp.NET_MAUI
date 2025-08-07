using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NotesApp.Models
{
    public class NotesGroup : ObservableCollection<Notes>
    {
        public string CategoryName { get; set; }

        public NotesGroup(string categoryName, IEnumerable<Notes> notes) : base(notes)
        {
            CategoryName = categoryName;
        }
    }
 
}

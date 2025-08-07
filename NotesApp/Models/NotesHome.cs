using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NotesApp.Models;

public class NotesHome
{
    public ObservableCollection<NotesGroup> GroupedNotes { get; set; } = new();
    public NotesHome()
    {

    }
    
    public async Task LoadNotesAsync()
    {

        GroupedNotes.Clear();

        var notes = await App.Database.GetNotesAsync(); //Load notes from the DataBase

        var grouped = notes
            .GroupBy(n => string.IsNullOrEmpty(n.Category) ? "Uncategorized" : n.Category)
            .OrderBy(g => g.Key)
            .Select(g => new NotesGroup(g.Key, g.OrderByDescending(n => n.Date)));

        foreach (var group in grouped)
            GroupedNotes.Add(group);

        foreach (var note in notes)
        {
            Console.WriteLine($"Loaded from DB: {note.Filename} - {note.Category}");
        }
    }
}

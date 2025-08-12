using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NotesApp.Models;

public class NotesHome
{
    public ObservableCollection<NotesGroup> GroupedNotes { get; set; } = new();
    public event PropertyChangedEventHandler PropertyChanged;
    public NotesHome()
    {

    }

    public async Task LoadNotesAsync()
    {

        GroupedNotes.Clear();

        var notes = await App.Database.GetNotesAsync(); //Load notes from the DataBase

        //try
        //{
        //    // Filter out notes with missing files
        //    var existingNotes = notes.Where(n => File.Exists(n.Filename)).ToList();

        //    // Delete stale notes from DB
        //    var staleNotes = notes.Where(n => !File.Exists(n.Filename)).ToList();

        //    foreach (var staleNote in staleNotes)
        //    {
        //        try
        //        {
        //            await App.Database.DeleteNoteAsync(staleNote);
        //            Console.WriteLine($"Deleted stale note from DB: {staleNote.Filename}");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error deleting stale note: {ex.Message}");
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error overal stale note: {ex.Message}");
        //}

        var grouped = notes
            .GroupBy(n => string.IsNullOrEmpty(n.Category) ? "Uncategorized" : n.Category)
            .OrderBy(g => g.Key)
            .Select(g => new NotesGroup(g.Key, g.OrderByDescending(n => n.Date)));

        foreach (var group in grouped)
            GroupedNotes.Add(group);

        //foreach (var note in notes)
        //{
        //    Console.WriteLine($"Loaded from DB: {note.Filename} - {note.Category}");
        //}

    }
    public async Task UpdateCategoryProgressAsync(int categoryId)
    {
        var progress = await App.Database.GetCategoryProgressAsync(categoryId);
        var category = GroupedNotes.FirstOrDefault(c => c.Id == categoryId);
        if (category != null)
        {
            category.Progress = progress;
        }
    }
}

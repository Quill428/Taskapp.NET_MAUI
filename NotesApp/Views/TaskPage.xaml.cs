namespace NotesApp.Views;
using NotesApp.Models;  // to access Notes class

[QueryProperty(nameof(ItemId), nameof(ItemId))]
[QueryProperty(nameof(NoteId), nameof(NoteId))]
public partial class TaskPage : ContentPage
{
    private List<string> categories = new() { "Work", "School", "Personal" }; //need to change this to ones that exist


    public string NoteId
    {
        set
        {
            if (int.TryParse(value, out var id))
                _ = LoadNoteByIdAsync(id);
        }
    }

    private string itemId;
    public string ItemId
    {
        get => itemId;
        set
        {
            itemId = value;
            LoadNote(value); // Make sure this method loads the note
        }
    }


    public TaskPage()
    {
        InitializeComponent();

        //string appDataPath = FileSystem.AppDataDirectory;
        //string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

        //LoadNote(Path.Combine(appDataPath, randomFileName));

        //CategoryPicker.ItemsSource = categories;
    }

    //private int noteId;
    //public int NoteId
    //{
    //    get => noteId;
    //    set
    //    {
    //        noteId = value;
    //        LoadNoteById(value);
    //    }
    //}

    private async void LoadNote(string fileName)
    {
        var note = await App.Database.GetNoteByFilenameAsync(fileName); //will check the database

        if (note == null)
        {
            // New note if not found
            note = new Models.Notes
            {
                Filename = fileName,
                Category = "Uncategorized",
                Date = DateTime.Now
            };
        }

        BindingContext = note;

        TextEditor.Text = note.Text;
        NewCategoryEntry.Text = note.Category;
    }
    //if (File.Exists(fileName))
    //{
    //    var lines = File.ReadAllLines(fileName);
    //    if (lines.Length >= 2)
    //    {
    //        noteModel.Category = lines[0];
    //        noteModel.Text = string.Join(Environment.NewLine, lines.Skip(1));
    //    }
    //    else
    //    {
    //        noteModel.Text = File.ReadAllText(fileName);
    //        noteModel.Category = "Uncategorized";
    //    }
    //noteModel.Date = File.GetLastWriteTime(fileName);

    //if (!string.IsNullOrWhiteSpace(noteModel.Category) && categories.Contains(noteModel.Category))
    //CategoryPicker.SelectedItem = noteModel.Category;    

    private async void SaveButton_pressed(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is Notes note)
            {
                note.Text = TextEditor.Text;
                note.Category = NewCategoryEntry.Text?.Trim() ?? "Uncategorized";
                note.Date = DateTime.Now;

                // Ensure filename exists for new notes
                if (string.IsNullOrEmpty(note.Filename))
                {
                    string appDataPath = FileSystem.AppDataDirectory;
                    note.Filename = Path.Combine(appDataPath, $"{Path.GetRandomFileName()}.notes.txt");
                }

                // Save to database
                var result = await App.Database.SaveNoteAsync(note);

                // Debug logging
                Console.WriteLine($"Save result: {result}");
                Console.WriteLine($"Note ID after save: {note.Id}");
                Console.WriteLine($"Note Text: {note.Text?.Substring(0, Math.Min(50, note.Text?.Length ?? 0))}");
                Console.WriteLine($"Note Category: {note.Category}");
                Console.WriteLine($"Note Filename: {note.Filename}");

                // Verify the save worked by trying to load it back
                var savedNote = await App.Database.GetNoteByIdAsync(note.Id);
                if (savedNote != null)
                {
                    Console.WriteLine("✓ Note successfully saved and can be retrieved!");
                    await DisplayAlert("Success", "Task saved successfully!", "OK");
                }
                else
                {
                    Console.WriteLine("✗ Note save failed - cannot retrieve saved note");
                    await DisplayAlert("Error", "Failed to save task properly", "OK");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Save error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    //            // Use consistent database reference - choose either App.Database OR App.NotesRepo
    //            await App.Database.SaveNoteAsync(note);

    //            // Debug: Check if save worked
    //            Console.WriteLine($"Saved note: {note.Id}, Category: {note.Category}, Text: {note.Text?.Substring(0, Math.Min(50, note.Text?.Length ?? 0))}");

    //            await DisplayAlert("Success", "Task saved successfully!", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
    //        return; // Don't navigate if save failed
    //    }
    //    await Shell.Current.GoToAsync("..");
    //}

    private void AddCategory_Clicked(object sender, EventArgs e)
    {
        var newCat = NewCategoryEntry.Text?.Trim();
        if (!string.IsNullOrWhiteSpace(newCat) && !categories.Contains(newCat))
        {
            categories.Add(newCat);
            //CategoryPicker.ItemsSource = null;
            //CategoryPicker.ItemsSource = categories;
            //CategoryPicker.SelectedItem = newCat;
            NewCategoryEntry.Text = string.Empty;
        }
    }

    private async void DeleteButton_pressed(object sender, EventArgs e)
    {
        if (BindingContext is Models.Notes note)
        {
            // asks for confirmation
            bool confirm = await DisplayAlert("Delete", "Are you sure you want to delete this note?", "Yes", "No");
            if (!confirm) return;

            // Delete from SQLite database
            try
            {
                if (note.Id != 0)
                {
                    var result = await App.Database.DeleteNoteAsync(note);
                    Console.WriteLine($"Delete result: {result} rows affected");

                    // Optional: delete file only if you continue to maintain files
                    if (!string.IsNullOrEmpty(note.Filename) && File.Exists(note.Filename))
                        File.Delete(note.Filename);

                    await DisplayAlert("Success", "Note deleted successfully!", "OK");
                }
                else
                {
                    Console.WriteLine("Cannot delete note with ID 0");
                    await DisplayAlert("Error", "Cannot delete unsaved note", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete error: {ex.Message}");
                await DisplayAlert("Error deleting note", ex.Message, "OK");
                return; // Don't navigate if delete failed
            }

            // Clear the UI and return
            TextEditor.Text = string.Empty;
            await Shell.Current.GoToAsync("..");
        }
    }

    private async Task LoadNoteByIdAsync(int id)
    {
        Models.Notes note;

        if (id == 0) // New note
        {
            // Create a completely new note
            string appDataPath = FileSystem.AppDataDirectory;
            string randomFileName = Path.Combine(appDataPath, $"{Path.GetRandomFileName()}.notes.txt");

            note = new Models.Notes
            {
                Id = 0, // This will trigger INSERT in SaveNoteAsync
                Filename = randomFileName,
                Date = DateTime.Now,
                Category = "Uncategorized",
                Text = string.Empty
            };
            Console.WriteLine("Created new note for editing");
        }
        else
        {
            // Load existing note
            note = await App.Database.GetNoteByIdAsync(id);
            if (note == null)
            {
                await DisplayAlert("Error", "Note not found!", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }
            Console.WriteLine($"Loaded existing note: ID {note.Id}");
        }

        BindingContext = note;
        TextEditor.Text = note.Text ?? string.Empty;
        NewCategoryEntry.Text = note.Category ?? "Uncategorized";
    }

}
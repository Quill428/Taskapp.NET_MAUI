namespace NotesApp.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class TaskPage : ContentPage
{
    private List<string> categories = new() { "Work", "School", "Personal" };

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

        string appDataPath = FileSystem.AppDataDirectory;
        string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

        LoadNote(Path.Combine(appDataPath, randomFileName));

        CategoryPicker.ItemsSource = categories;
    }

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
        if (BindingContext is Models.Notes note)
        {
            note.Text = TextEditor.Text;
            note.Category = NewCategoryEntry.Text?.Trim() ?? "Uncategorized";
            //note.Save();'
            note.Date = DateTime.Now;

            await App.Database.SaveNoteAsync(note);
        }
        await Shell.Current.GoToAsync("..");
    }

    private void AddCategory_Clicked(object sender, EventArgs e)
    {
        var newCat = NewCategoryEntry.Text?.Trim();
        if (!string.IsNullOrWhiteSpace(newCat) && !categories.Contains(newCat))
        {
            categories.Add(newCat);
            CategoryPicker.ItemsSource = null;
            CategoryPicker.ItemsSource = categories;
            CategoryPicker.SelectedItem = newCat;
            NewCategoryEntry.Text = string.Empty;
        }
    }

    private async void DeleteButton_pressed(object sender, EventArgs e)
	{
        if (BindingContext is Models.Notes note)
        {
            // asks for confirmation
            //bool confirm = await DisplayAlert("Delete", "Are you sure you want to delete this note?", "Yes", "No");
            //if (!confirm) return;

            // Delete from SQLite database
            if (note.Id != 0)
            {
                await App.NotesRepo.DeleteNoteAsync(note);
            }

            // Safely delete the file
            if (!string.IsNullOrEmpty(note.Filename) && File.Exists(note.Filename))
            {
                File.Delete(note.Filename);
            }

            // Clear the UI and return
            TextEditor.Text = string.Empty;
            await Shell.Current.GoToAsync("..");
        }
	}



}
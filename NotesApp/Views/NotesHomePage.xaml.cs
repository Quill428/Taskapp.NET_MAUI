namespace NotesApp.Views;

//using Android.Webkit;
using NotesApp.Models;
//using ObjCBindings;
using System;
using System.Linq;
using System.Threading.Tasks;


public partial class NotesHomePage : ContentPage
{
    public NotesHomePage()
    {
        InitializeComponent();

        BindingContext = new NotesHome();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //await ((NotesHome)BindingContext).LoadNotesAsync();
        try
        {
            if (BindingContext is NotesHome notesHome)
            {
                await notesHome.LoadNotesAsync();
                Console.WriteLine($"GroupedNotes count: {notesHome.GroupedNotes.Count}");

                // Debug: Print all notes
                foreach (var group in notesHome.GroupedNotes)
                {
                    Console.WriteLine($"Category: {group.CategoryName}, Tasks: {group.Count()}");
                    foreach (var note in group)
                    {
                        Console.WriteLine($"  - {note.Text?.Substring(0, Math.Min(30, note.Text?.Length ?? 0))} (ID: {note.Id})");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load notes: {ex.Message}", "OK");
        }
    }


    private async void Add_Clicked(object sender, EventArgs e)
    {
        // Create a new note with a temporary ID to distinguish it as new
        await Shell.Current.GoToAsync($"{nameof(TaskPage)}?NoteId=0");
        //await Shell.Current.GoToAsync(nameof(TaskPage));
    }

    private async void ResetPrivacy_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Reset Privacy Policy",
            "This will reset the privacy policy acceptance for testing. The app will restart and show the privacy policy again.",
            "Yes", "No");

        if (confirm)
        {
            App.ResetPrivacyPolicyAcceptance();
            await DisplayAlert("Reset Complete", "App will restart to show privacy policy.", "OK");

            // Restart the app to show privacy policy
            Application.Current.MainPage = new NavigationPage(new Views.PrivacyPolicyPage());
        }
    }

    private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Gets the model of the notes
            var note = (Models.Notes)e.CurrentSelection[0];

            // Should navigate to "NotePage?ItemId=path\on\device\XYZ.notes.txt"
            await Shell.Current.GoToAsync($"{nameof(TaskPage)}?NoteId={note.Id}");
            //($"{nameof(TaskPage)}?{nameof(TaskPage.ItemId)}={note.Filename}");

            // Unselect the UI
            notesCollection.SelectedItem = null;
        }

    }
    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if ((sender as CheckBox)?.BindingContext is Models.Notes note)
        {
            note.IsCompleted = e.Value;
            await App.Database.SaveNoteAsync(note);

            var viewModel = (NotesHome)BindingContext;
            var category = viewModel.GroupedNotes.FirstOrDefault(c => c.CategoryName == note.Category);

            if (category != null)
            {
                category.UpdateProgress(); // This will recalculate and notify UI
            }

            //var viewModel = (NotesHome)BindingContext;
            //foreach (var category in viewModel.GroupedNotes)
            //{
            //    var notesInCategory = category.ToList();
            //    category.TaskCount = notesInCategory.Count;
            //    if (notesInCategory.Count > 0)
            //    {
            //        category.Progress = (double)notesInCategory.Count(n => n.IsCompleted) / notesInCategory.Count * 100;
            //    }
            //    else
            //    {
            //        category.Progress = 0;
            //    }
            //}
            //await viewModel.UpdateCategoryProgressAsync(note.CategoryId);

        }
        //progress = (number of completed tasks in category / total tasks in category) *100


    }
    private async Task UpdateCategoryProgressAsync(int categoryId)
    {
        var progress = await App.Database.GetCategoryProgressAsync(categoryId);
        var viewModel = (NotesHome)BindingContext;
        var category = viewModel.GroupedNotes.FirstOrDefault(c => c.Id == categoryId);
        if (category != null)
        {
            category.Progress = progress;
        }
    }


    //private async void DeleteCategoryButton_Clicked(object sender, EventArgs e)
    //{
    //    if (BindingContext is Category category)
    /// <summary>
    ///   {
    /// </summary>
    //        bool confirm = await DisplayAlert("Delete Category?",
    //            $"This will delete all notes in '{category.Name}'. Continue?",
    //            "Yes", "No");

    //        if (confirm)
    //        {
    //            await App.NotesRepo.DeleteCategoryAsync(category.Id);
    //            await LoadCategories(); // refresh UI
    //        }
    //    }
    //}
    //private async void DeleteCategoryButton_Clicked(object sender, EventArgs e)
    //{
    //    if (sender is Button button && button.CommandParameter is string categoryName)
    //    {
    //        bool confirm = await DisplayAlert(
    //            "Delete Category",
    //            $"Are you sure you want to delete '{categoryName}' and all its notes?",
    //            "Yes", "No");

    //        if (confirm)
    //        {
    //            // Delete all notes in this category
    //            var notesInCategory = await App.NotesRepo.GetNotesByCategoryAsync(categoryName);
    //            foreach (var note in notesInCategory)
    //            {
    //                await App.NotesRepo.DeleteNoteAsync(note);
    //            }

    //            // Reload the notes list
    //            await ((NotesHome)BindingContext).LoadNotesAsync();
    //        }
    //    }
    //}
    private async void DeleteCategoryButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is NotesGroup group)
        {
            bool confirm = await DisplayAlert("Delete Category",
                $"Delete '{group.CategoryName}' and all its notes?",
                "Yes", "No");
            if (!confirm) return;

            try
            {
                // Delete all notes in DB for that category
                var result = await App.Database.DeleteNotesByCategoryAsync(group.CategoryName);
                Console.WriteLine($"Deleted {result} notes from category '{group.CategoryName}'");

                // If you have a separate Categories table, optionally delete category record as well
                // await App.Database.DeleteCategoryByNameAsync(group.CategoryName);

                await DisplayAlert("Success", $"Deleted category '{group.CategoryName}' and all its notes!", "OK");

                // Refresh UI
                await ((NotesHome)BindingContext).LoadNotesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete category error: {ex.Message}");
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}




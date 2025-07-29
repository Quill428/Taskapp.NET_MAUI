namespace NotesApp.Notes_App;

public partial class NotesHomePage : ContentPage
{
	public NotesHomePage()
	{
		InitializeComponent();
	}

	private void NewNotePage_Clicked(object sender, EventArgs e)
	{
		Navigation.PushModalAsync(new TestTaskPage1());
    }
}




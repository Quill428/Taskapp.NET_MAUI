namespace NotesApp.Notes_App;

public partial class TestTaskPage1 : ContentPage
{
	string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");
	public TestTaskPage1()
	{
		InitializeComponent();

		if (File.Exists(_fileName))
			TextEditor.Text = File.ReadAllText(_fileName);
	}

	private void SaveButton_pressed(object sender, EventArgs e)
	{
		File.WriteAllText(_fileName, TextEditor.Text);
	}

    private void DeleteButton_pressed(object sender, EventArgs e)
	{
		if (File.Exists(_fileName))
			File.Delete(_fileName);

		TextEditor.Text = string.Empty;
	}


}
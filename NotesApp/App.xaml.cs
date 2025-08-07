using NotesApp.Data;
using NotesApp.Views;

namespace NotesApp
{
    public partial class App : Application
    {
        static NotesDatabase database;

        public static NotesDatabase NotesRepo { get; private set; }

        public static NotesDatabase Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(FileSystem.AppDataDirectory, "NotesApp.db3");
                    database = new NotesDatabase(dbPath);
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            // Initialize NotesRepo
            NotesRepo = new NotesDatabase(Path.Combine(FileSystem.AppDataDirectory, "notes.db3"));



            MainPage = new AppShell();

            //MainPage = new NotesHomePage();
        }


        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}
using NotesApp.Data;
using NotesApp.Views;

namespace NotesApp
{
    public partial class App : Application
    {
        static NotesDatabase database;

        //public static NotesDatabase NotesRepo { get; private set; }

        public static NotesDatabase Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Notes.db3"); //was NotesApp.db3
                    database = new NotesDatabase(dbPath);
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
           
            bool privacyPolicyAccepted = Preferences.Get("PrivacyPolicyAccepted", false);

            if (privacyPolicyAccepted)
            {
                // User has already accepted, go to main app
                MainPage = new AppShell();
            }
            else
            {
                // First time user, show privacy policy
                MainPage = new NavigationPage(new PrivacyPolicyPage());
            }
        }
        // Optional: Method to reset privacy policy acceptance (for testing)
        public static void ResetPrivacyPolicyAcceptance()
        {
            Preferences.Remove("PrivacyPolicyAccepted");
            Preferences.Remove("PrivacyPolicyAcceptedDate");
        }


        //MainPage = new NotesHomePage();
    }


        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    
}
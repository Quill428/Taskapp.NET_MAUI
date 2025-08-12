namespace NotesApp.Views;

public partial class PrivacyPolicyPage : ContentPage
{
    public PrivacyPolicyPage()
    {
        InitializeComponent();
    }

    private void ConsentCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Enable/disable the continue button based on checkbox state
        ContinueButton.IsEnabled = e.Value;
        ContinueButton.Opacity = e.Value ? 1.0 : 0.5;

        if (e.Value)
        {
            // Add a subtle animation when enabled
            ContinueButton.ScaleTo(1.05, 100).ContinueWith(t =>
                ContinueButton.ScaleTo(1.0, 100));
        }
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Save that the user has accepted the privacy policy
            Preferences.Set("PrivacyPolicyAccepted", true);
            Preferences.Set("PrivacyPolicyAcceptedDate", DateTime.Now.ToString());

            // Show a brief confirmation
            await DisplayAlert("Welcome!", "Thank you for reading our privacy policy. Enjoy using NotesApp!", "Let's Go!");

            // Navigate to the main app (AppShell)
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to continue: {ex.Message}", "OK");
        }
    }
}

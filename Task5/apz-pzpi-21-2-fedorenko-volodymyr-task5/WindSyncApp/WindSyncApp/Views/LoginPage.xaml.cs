using WindSyncApp.Services;

namespace WindSyncApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly IApiService _apiService;

    public LoginPage(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        bool success = await _apiService.LoginAsync(EmailEntry.Text, PasswordEntry.Text);
        if (success)
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password", "OK");
        }
    }
}
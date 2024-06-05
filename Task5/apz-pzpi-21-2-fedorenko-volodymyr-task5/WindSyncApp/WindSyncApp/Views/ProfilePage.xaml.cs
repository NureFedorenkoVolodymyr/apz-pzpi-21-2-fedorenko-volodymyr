using System.Globalization;
using WindSyncApp.Services;
using WindSyncApp.Utils;

namespace WindSyncApp.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ApiService _apiService;

    public ProfilePage()
    {
        InitializeComponent();
        _apiService = new ApiService(new HttpClient());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var user = await _apiService.GetUserProfileAsync();
        UsernameLabel.Text = user.Username;
        EmailLabel.Text = user.Email;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Handle logout
        SecureStorage.Remove(Constants.AuthToken);
        Application.Current.MainPage = new LoginPage(_apiService);
        //await Navigation.PopToRootAsync();
    }

    private void OnSetEnglishLanguage(object sender, EventArgs e)
    {
        ChangeLanguage("en-US");
    }

    private void OnSetUkrainianLanguage(object sender, EventArgs e)
    {
        ChangeLanguage("uk-UA");
    }

    private void ChangeLanguage(string culture)
    {
        var ci = new CultureInfo(culture);
        LocalizationService.SetLocale(ci);

        (App.Current as App).MainPage.Dispatcher.Dispatch(() =>
        {
            // there some LoadLang method;
            (App.Current as App).MainPage = new AppShell();//REQUIRE RUN MAIN THREAD
        });
    }
}
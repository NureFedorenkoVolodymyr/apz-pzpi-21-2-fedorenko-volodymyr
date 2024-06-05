using WindSyncApp.Services;
using WindSyncApp.Views;

namespace WindSyncApp
{
    public partial class App : Application
    {
        private readonly IApiService _apiService;

        public App(IApiService apiService)
        {
            InitializeComponent();

            _apiService = apiService;

            LocalizationService.SetLocale(LocalizationService.GetCurrentCultureInfo());

            if (_apiService.IsAuthenticated())
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage(apiService));
            }
        }
    }
}

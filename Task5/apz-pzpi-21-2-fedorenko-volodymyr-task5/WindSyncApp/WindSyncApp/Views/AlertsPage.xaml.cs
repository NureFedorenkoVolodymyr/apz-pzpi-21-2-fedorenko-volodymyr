using System.Collections.ObjectModel;
using System.ComponentModel;
using WindSyncApp.Models;
using WindSyncApp.Services;

namespace WindSyncApp.Views;

public partial class AlertsPage : ContentPage, INotifyPropertyChanged
{
    private ApiService _apiService;
    public ObservableCollection<Alert> Alerts { get; } = new ObservableCollection<Alert>();
    public Command RefreshCommand { get; }

    private bool _isRefreshing;
    public event PropertyChangedEventHandler PropertyChanged;

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }

    public AlertsPage()
    {
        InitializeComponent();
        _apiService = new ApiService(new HttpClient());

        RefreshCommand = new Command(async () =>
        {
            IsRefreshing = true;
            await LoadAlerts();
            IsRefreshing = false;
        });
        BindingContext = this;
    }

    //public TurbinesPage(ApiService apiService)
    //{
    //    InitializeComponent();

    //    _apiService = apiService;

    //    RefreshCommand = new Command(async () => await LoadTurbines());
    //    BindingContext = this;
    //}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAlerts();
    }

    private async Task LoadAlerts()
    {
        Alerts.Clear();
        var alerts = await _apiService.GetMyAlertsAsync();

        foreach (var alert in alerts)
        {
            Alerts.Add(alert);
        }
    }

    //private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (e.CurrentSelection.FirstOrDefault() is Alert selectedAlert)
    //    {
    //        //Navigation.PushAsync(new TurbineDetailsPage(selectedTurbine));
    //    }
    //}

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
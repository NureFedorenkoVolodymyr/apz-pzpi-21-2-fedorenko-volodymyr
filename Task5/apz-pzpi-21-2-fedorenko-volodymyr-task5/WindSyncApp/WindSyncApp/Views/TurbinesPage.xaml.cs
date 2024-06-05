using System.Collections.ObjectModel;
using System.ComponentModel;
using WindSyncApp.Models;
using WindSyncApp.Services;

namespace WindSyncApp.Views;

public partial class TurbinesPage : ContentPage, INotifyPropertyChanged
{
    private ApiService _apiService;
    public ObservableCollection<Turbine> Turbines { get; } = new ObservableCollection<Turbine>();
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

    public TurbinesPage()
    {
        InitializeComponent();
        _apiService = new ApiService(new HttpClient());

        RefreshCommand = new Command(async () =>
        {
            IsRefreshing = true;
            await LoadTurbines();
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
        await LoadTurbines();
    }

    private async Task LoadTurbines()
    {
        Turbines.Clear();
        var turbines = await _apiService.GetMyTurbinesAsync();

        foreach (var turbine in turbines)
        {
            Turbines.Add(turbine);
        }
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Turbine selectedTurbine)
        {
            Navigation.PushAsync(new TurbineDetailsPage(selectedTurbine));
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindSyncApp.Models;
using WindSyncApp.Services;

namespace WindSyncApp.Views;

public partial class TurbineDetailsPage : ContentPage, INotifyPropertyChanged
{
    private readonly IApiService _apiService;
    private Turbine Turbine;
    private ObservableCollection<TurbineData> _turbineData;

    public ObservableCollection<TurbineData> TurbineData
    {
        get => _turbineData;
        set
        {
            _turbineData = value;
            OnPropertyChanged();
        }
    }

    public TurbineDetailsPage(Turbine turbine)
    {
        InitializeComponent();
        BindingContext = this;
        Turbine = turbine;

        _apiService = new ApiService(new HttpClient());
        TurbineData = new ObservableCollection<TurbineData>();

        SetTurbineLabels();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    protected async Task LoadData()
    {
        var end = DateTime.UtcNow;
        var start = end.AddMonths(-2);

        var turbineData = await _apiService.GetTurbineDataAsync(Turbine.Id, start, end);

        TurbineData.Clear();
        foreach (var data in turbineData)
        {
            TurbineData.Add(data);
        }
    }

    private void SetTurbineLabels()
    {
        IdLabel.Text = Turbine.Id.ToString();
        StatusLabel.Text = Turbine.Status.ToString();
        RadiusLabel.Text = Turbine.TurbineRadius.ToString();
        SweptAreaLabel.Text = Turbine.SweptArea.ToString();
        LatitudeLabel.Text = Turbine.Latitude.ToString();
        LongitudeLabel.Text = Turbine.Longitude.ToString();
        AltitudeLabel.Text = Turbine.Altitude.ToString();
        EfficiencyLabel.Text = Turbine.Efficiency.ToString();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
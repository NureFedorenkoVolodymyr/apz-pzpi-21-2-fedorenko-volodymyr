using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WindSyncApp.Models;
using WindSyncApp.Utils;

namespace WindSyncApp.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseAddress;
    private readonly string _url;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5006" : "http://localhost:5006";
        _url = $"{_baseAddress}/api";

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        if (!CheckInternetConnection())
            return false;

        try
        {
            var json = JsonSerializer.Serialize(new { email, password }, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_url}/auth/login", content);

            if (!CheckHttpRequestSuccess(response, nameof(LoginAsync)))
                return false;

            var token = await response.Content.ReadAsStringAsync();
            await SecureStorage.SetAsync(Constants.AuthToken, token);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return false;
    }

    public bool IsAuthenticated()
    {
        //SecureStorage.RemoveAll();

        var token = SecureStorage.GetAsync(Constants.AuthToken).Result;
        return !string.IsNullOrEmpty(token);
    }

    public async Task<List<Turbine>> GetMyTurbinesAsync()
    {
        var turbines = new List<Turbine>();

        if (!CheckInternetConnection())
            return turbines;

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAuthToken());

            var response = await _httpClient.GetAsync($"{_url}/turbines/my");

            if (!CheckHttpRequestSuccess(response, nameof(GetMyTurbinesAsync)))
                return turbines;

            var content = await response.Content.ReadAsStringAsync();
            turbines = JsonSerializer.Deserialize<List<Turbine>>(content, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return turbines;
    }

    public async Task<List<TurbineData>> GetTurbineDataAsync(int turbineId, DateTime start, DateTime end)
    {
        var turbineData = new List<TurbineData>();
        if (!CheckInternetConnection())
            return turbineData;

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAuthToken());
            var response = await _httpClient.GetAsync($"{_url}/turbines/{turbineId}/data?start={start:O}&end={end:O}");

            if (!CheckHttpRequestSuccess(response, nameof(GetTurbineDataAsync)))
                return turbineData;

            var content = await response.Content.ReadAsStringAsync();
            turbineData = JsonSerializer.Deserialize<List<TurbineData>>(content, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return turbineData;
    }

    public async Task<UserInfo> GetUserProfileAsync()
    {
        var user = new UserInfo();

        if (!CheckInternetConnection())
            return user;

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAuthToken());

            var response = await _httpClient.GetAsync($"{_url}/auth");

            if (!CheckHttpRequestSuccess(response, nameof(GetUserProfileAsync)))
                return user;

            var content = await response.Content.ReadAsStringAsync();
            user = JsonSerializer.Deserialize<UserInfo>(content, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return user;
    }

    public async Task<List<Alert>> GetMyAlertsAsync()
    {
        var alerts = new List<Alert>();

        if (!CheckInternetConnection())
            return alerts;

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAuthToken());

            var response = await _httpClient.GetAsync($"{_url}/alerts");

            if (!CheckHttpRequestSuccess(response, nameof(GetMyAlertsAsync)))
                return alerts;

            var content = await response.Content.ReadAsStringAsync();
            alerts = JsonSerializer.Deserialize<List<Alert>>(content, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return alerts;
    }

    private async Task<string> GetAuthToken()
    {
        return await SecureStorage.GetAsync(Constants.AuthToken);
    }

    private bool CheckInternetConnection()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("---> No internet access.");
            return false;
        }

        return true;
    }

    private void HandleException(Exception ex)
    {
        Debug.WriteLine($"---> Exception occured: {ex.Message}.");
    }

    private bool CheckHttpRequestSuccess(HttpResponseMessage response, string methodName)
    {
        if (response.IsSuccessStatusCode)
        {
            Debug.WriteLine($"---> {methodName} success.");
            return true;
        }
        else
        {
            Debug.WriteLine($"---> {methodName} error: {response.StatusCode}.");
            return false;
        }
    }
}
using System.Globalization;

namespace WindSyncApp.Services;

public static class LocalizationService
{
    public static void SetLocale(CultureInfo ci)
    {
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
    }

    public static CultureInfo GetCurrentCultureInfo()
    {
        var netLanguage = "en";
        if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
        {
            var ci = CultureInfo.CurrentCulture;
            netLanguage = ci.Name.Replace("_", "-");
        }
        return new CultureInfo(netLanguage);
    }
}

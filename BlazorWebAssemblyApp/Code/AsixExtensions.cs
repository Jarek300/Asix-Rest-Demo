namespace Asix;

public partial class AsixRestClient
{
    public readonly static string AlarmDomainName = "Fabryka_EVO";
    public readonly static string RestServerUrl = "http://asport.askom.pl/asix";
    public readonly static string RestServerUrlHttps = "https://asport.askom.pl/asix";

    public static AsixRestClient Create()
    {
        HttpClient httpClient = new HttpClient();

        AsixRestClient asixRestClient = new AsixRestClient(httpClient);
        asixRestClient.BaseUrl = RestServerUrl;
        return asixRestClient;
    }


    public static bool IsQualityGood(int aQuality)
    {
        return (aQuality & 0xC0) == 0xC0;
    }


    /// <summary>
    /// Funkcja pozwala sprawdzić czy jakość zmiennej jest niepewna
    /// </summary>
    public static bool IsQualityUncertain(int aQuality)
    {
        return (aQuality & 0xC0) == 0x40;
    }


    /// <summary>
    /// Funkcja pozwala sprawdzić czy jakość zmiennej jest zła
    /// </summary>
    public static bool IsQualityBad(int aQuality)
    {
        return (aQuality & 0xC0) == 0;
    }
}


public partial class AggregateRange
{
    public AggregateRange(string aName, string aAggregate, string aPeriodLength, string aRefreshInterval)
    {
        Name = aName;
        Aggregate = aAggregate;
        PeriodLength = aPeriodLength;
        RefreshInterval = aRefreshInterval;
    }
}

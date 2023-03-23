namespace Asix;

public partial class AsixRestClient
{
    public readonly static string AlarmDomainName = "Fabryka_EVO";

    public static AsixRestClient Create()
    {
        HttpClient httpClient = new HttpClient();
        AsixRestClient asixRestClient = new AsixRestClient(httpClient);
        asixRestClient.BaseUrl = "http://fab2/asix";
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

using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Asix;

public partial class AsixRestClient
{
    public readonly static string AlarmDomainName = "Fabryka_EVO";
    public readonly static string RestServerUrl = "http://fab2/asix";
    public readonly static string RestServerUrlHttps = "https://fab2/asix";

    public static AsixRestClient Create()
    {
        HttpClient httpClient = new HttpClient();

        AsixRestClient asixRestClient = new AsixRestClient(httpClient);
        asixRestClient.BaseUrl = RestServerUrl;
        return asixRestClient;
    }


    public static AsixRestClient Create(string aUserName, string aPassword)
    {
        HttpClientHandler httpClientHandler = new();
        httpClientHandler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

        HttpClient httpClient = new(httpClientHandler);

        // Add the basic authentication header to the HTTP request
        string authInfo = $"{aUserName}:{aPassword}";
        string encodedAuthInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedAuthInfo);

        AsixRestClient asixRestClient = new AsixRestClient(httpClient);
        asixRestClient.BaseUrl = RestServerUrlHttps;
        return asixRestClient;
    }


    static bool ServerCertificateCustomValidation(
        HttpRequestMessage aHttpRequestMessage,
        X509Certificate2? aX509Certificate2,
        X509Chain? aX509Chain,
        SslPolicyErrors aSslPolicyErrors)
    {
        if (aSslPolicyErrors == SslPolicyErrors.None)
            return true;

        if (aX509Certificate2 != null)
            return true;

        // w ogóle nie ma certyfikatu - zgłaszamy błąd
        return false;
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

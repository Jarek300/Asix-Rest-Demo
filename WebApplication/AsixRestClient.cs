using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;

#pragma warning disable 0649


namespace WebApplication
{

    /// <summary>
    /// Typ wyniku zwracanego przez funkcję ReadVariableAttributes
    /// </summary>
    public class VariableAttributes
    {
        /// <summary>
        /// Nazwa zmiennej
        /// </summary>
        public string id = "";

        /// <summary>
        /// Wynik odczytu wartości atrybutu zmiennej
        /// </summary>
        public bool readSucceeded = false;


        /// <summary>
        /// Tekstowy opis błędu odczytu
        /// </summary>
        public string readStatusString;


        /// <summary>
        /// Lista nazw czytanych atrybutów
        /// </summary>
        public List<string> mAttributeNameList;


        /// <summary>
        /// Lista nazwa wartości atrybutów
        /// </summary>
        public List<string> mAttributeValueList;
    };


    public class ServerAttributes
    {
        public bool readSucceeded = false;
        public string readStatusString;

        public List<string> mAttributeNameList;
    };


    public class VariableState
    {
        public string id = "";
        public bool readSucceeded = false;
        public string readStatusString;
        public DateTime timeStamp = DateTime.UtcNow;
        public uint quality = 0;
        public object value = null;


        public bool IsQualityGood()
        {
            return (quality & 0xC0) == 0xC0;
        }


        public bool IsQualityUncertain()
        {
            return (quality & 0xC0) == 0x40;
        }


        public bool IsQualityBad()
        {
            return (quality & 0xC0) == 0;
        }
    };



    public class VariableRawSample
    {
        public DateTime t; // – początek interwału, dla którego wyliczono wartość agregatu,
        public uint q;     // – jakość próbki,
        public double v;   // – wartość próbki; wartość jest zawsze typu liczbowego.

        public bool IsQualityGood()
        {
            return (q & 0xC0) == 0xC0;
        }


        public bool IsQualityUncertain()
        {
            return (q & 0xC0) == 0x40;
        }


        public bool IsQualityBad()
        {
            return (q & 0xC0) == 0;
        }
    };



    public class VariableRawArchive
    {
        public string id;  // – nazwa zmiennej i nazwa agregatu oddzielone ukośnikiem,

        public bool readSucceeded; // – wynik odczytu wartości historycznych zmiennej – wartość typu bool,
        public string readStatusString; // – tekstowy opis błędu odczytu,

        public bool moreDataAvailable;  // w podanym okresie znajduje się więcej próbek niż podano w parametrze maxNumberOfSamples,
        public DateTime periodStartTime; // – początek okresu, dla którego pobrano dane,
        public DateTime periodEndTime; // – koniec okresu, dla którego pobrano dane,

        public VariableRawSample[] samples; // – tablica obiektów wartości 
    };





    public class VariableAggregateSample
    {
        public DateTime t; // – początek interwału, dla którego wyliczono wartość agregatu,
        public DateTime e; // – koniec interwału, dla którego wyliczono wartość agregatu,
        public uint q;     // – jakość próbki,
        public double v;   // – wartość próbki; wartość jest zawsze typu liczbowego.

        public bool IsQualityGood()
        {
            return (q & 0xC0) == 0xC0;
        }


        public bool IsQualityUncertain()
        {
            return (q & 0xC0) == 0x40;
        }


        public bool IsQualityBad()
        {
            return (q & 0xC0) == 0;
        }

    };


    public class VariableAggregateArchive
    {
        public string id;  // – nazwa zmiennej i nazwa agregatu oddzielone ukośnikiem,

        public bool readSucceeded; // – wynik odczytu wartości historycznych zmiennej – wartość typu bool,
        public string readStatusString; // – tekstowy opis błędu odczytu,

        public DateTime periodStartTime; // – początek okresu, dla którego pobrano dane,
        public DateTime periodEndTime; // – koniec okresu, dla którego pobrano dane,

        public VariableAggregateSample[] samples; // – tablica obiektów wartości archiwalnych.
    };



    public class AlarmState
    {
        public string id;  // – identyfikator alarmu(domena i nazwa alarmu oddzielone ukośnikiem),

        public bool readSucceeded; // – wynik odczytu stany alarmu – wartość typu bool,

        public string readStatusString;    // – tekstowy opis błędu odczytu,

        public bool active;    // – czy alarm jest aktualnie aktywny,

        public string stateDescription;    // – opis stanu alarmu,

        public bool ackAvailabe;   // – czy dostępna jest informacja o potwierdzeniu alarmu,
        public bool ack;           // – czy alarm jest potwierdzony.
    };


    public class HistAlarmState
    {
        public string name; // – nazwa alarmu,

        public string description; // – opis alarmu,

        public string priority; // – priorytet alarmu,

        public bool startOnly; // – wartość true, jeśli alarm ma tylko początek.

        public DateTime startTime; // – data początku alarmu,

        public DateTime startDetectTime; // – data detekcji początku alarmu,

        public DateTime? endTime; // – data końca alarmu; null jeśli alarm się nie zakończył,

        public DateTime? endDetectTime; // – data detekcji końca alarmu; null jeśli alarm się nie zakończył,

        public bool ack; // – czy alarm jest potwierdzony,

        public string ackUser; // – nazwa użytkownika, który potwierdził alarm,

        public string ackStation; // – nazwa komputera, na którym potwierdzono alarm,

        public DateTime? ackTime; // – data potwierdzenia alarmu,

        public string ackNote; // – notatka potwierdzenia alarmu.
    }


    public class HistAlarmArchive
    {
        public bool readSucceeded; // – wynik odczytu z archiwum alarmów – wartość typu bool,

        public string readStatusString; // – tekstowy opis błędu odczytu,

        public bool moreDataAvailable; // – w podanym okresie znajduje się więcej alarmów niż podano w parametrze limit,

        public HistAlarmState[] alarms; // – tablica obiektów wartości archiwalnych alarmów.
    }


    /// <summary>
    /// Klasa pośrednicząca w odczycie danych procesowych z serwera REST aplikacji Asix.Evo.
    /// Zawiera wszystkie funkcją realizujące odczyt danych procesowych.
    /// </summary>
    public class AsixRestClient
    {
        string mServerBaseAddress = "http://asport.askom.com.pl";
        HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="aServerBaseAddress">Adres komputera, na którym jest uruchomiony serwer REST. 
        /// Jeśli parametr zostanie pominięty to użyty zostanie serwer demonstracyjny asport.askom.com.pl</param>
        public AsixRestClient(string aServerBaseAddress = "")
        {
            if (!string.IsNullOrEmpty(aServerBaseAddress))
                mServerBaseAddress = aServerBaseAddress;

            httpClient.BaseAddress = new Uri(mServerBaseAddress);
        }



        /// <summary>
        /// Odczyt wartości atrybutów zmiennej.
        /// </summary>
        /// <param name="aVariableName">Nazwa zmiennej</param>
        /// <param name="aAttributeNameList">Lista nazw atrybutów zmiennej</param>
        /// <returns></returns>
        public VariableAttributes ReadVariableAttributes(string aVariableName, List<string> aAttributeNameList)
        {
            String uri = "/asix/v1/variable/attribute";
            uri = QueryHelpers.AddQueryString(uri, "name", aVariableName);

            foreach (var i in aAttributeNameList)
                uri = QueryHelpers.AddQueryString(uri, "attribute", i);


            VariableAttributes variableAttributes = new VariableAttributes();
            variableAttributes.id = aVariableName;
            variableAttributes.mAttributeNameList = aAttributeNameList;


            HttpResponseMessage response = httpClient.GetAsync(uri).Result;
            if (!response.IsSuccessStatusCode)
            {
                variableAttributes.readSucceeded = false;
                variableAttributes.readStatusString = "Błąd http odczytu atrybutów: " + response.StatusCode.ToString();
                return variableAttributes;
            }             

                
            List<List<string>> content = response.Content.ReadAsAsync<List<List<string>>>().Result;                
            variableAttributes.mAttributeValueList = content[0];
            if (variableAttributes.mAttributeValueList == null)
            {
                variableAttributes.readSucceeded = false;
                variableAttributes.readStatusString = "Błąd odczytu atrybutów : nieznana zmienna";
                return variableAttributes;
            }


            variableAttributes.readSucceeded = true;
            return variableAttributes;
        }





        /// <summary>
        /// Odczyt nazw dostępnych atrybutów zmiennych.
        /// </summary>
        /// <returns></returns>
        public ServerAttributes ReadAttributeNames()
        {
            ServerAttributes serverAttributes = new ServerAttributes();

            try
            {
                HttpResponseMessage response = httpClient.GetAsync("/asix/v1/server/variable/attribute").Result;
                if (!response.IsSuccessStatusCode)
                {
                    serverAttributes.readSucceeded = false;
                    serverAttributes.readStatusString = "Błąd http odczytu nazw atrybutów: " + response.StatusCode.ToString();
                    return serverAttributes;
                }             

                
                serverAttributes.readSucceeded = true;
                serverAttributes.mAttributeNameList = response.Content.ReadAsAsync<List<string>>().Result;
                return serverAttributes;
            }
            catch (Exception e)
            {
                serverAttributes.readSucceeded = false;
                serverAttributes.readStatusString = "Błąd: " + e.Message;
                return serverAttributes;
            }
        }



        /// <summary>
        /// Odczyt wartości bieżącej zmiennej
        /// </summary>
        /// <param name="aVariableName">Nazwa zmiennej aplikacji Asix.Evo</param>
        /// <returns></returns>
        public VariableState ReadVariableState(string aVariableName)
        {
            try
            {
                String uri = "/asix/v1/variable/value";
                uri = QueryHelpers.AddQueryString(uri, "name", aVariableName);


                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    VariableState variableState = new VariableState();
                    variableState.id = aVariableName;
                    variableState.readSucceeded = false;
                    variableState.readStatusString = "Błąd http: " + response.StatusCode.ToString();
                    return variableState;
                }             


                List<VariableState> variableStateList = response.Content.ReadAsAsync<List<VariableState>>().Result;
                return variableStateList[0];
            }
            catch (Exception e)
            {
                VariableState variableState = new VariableState();
                variableState.id = aVariableName;
                variableState.readSucceeded = false;
                variableState.readStatusString = "Błąd: " + e.Message;
                return variableState;
            }
        }




        /// <summary>
        /// Odczyt wartości bieżącej agregatu zmiennej
        /// </summary>
        /// <param name="aVariableName">Nazwa zmiennej aplikacji Asix.Evo</param>
        /// <param name="aAggregate">Nazwa agregatu</param>
        /// <param name="aPeriodLength">Długość okres, za który wyliczany jest agregat; długość należy podać w formacie czasu OPC.</param>
        /// <param name="aRefershInterval">Okres wyliczania agregatu; długość należy podać w formacie czasu OPC.</param>
        /// <returns></returns>
        public VariableState ReadVariableAggregate(string aVariableName, string aAggregate, string aPeriodLength, string aRefershInterval = "60s")
        {
            try
            {
                String uri = "/asix/v1/variable/aggregate";
                uri = QueryHelpers.AddQueryString(uri, "name", aVariableName);
                uri = QueryHelpers.AddQueryString(uri, "aggregate", aAggregate);
                uri = QueryHelpers.AddQueryString(uri, "periodLength", aPeriodLength);
                uri = QueryHelpers.AddQueryString(uri, "refershInterval", aRefershInterval);


                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    VariableState variableState = new VariableState();
                    variableState.id = aVariableName;
                    variableState.readSucceeded = false;
                    variableState.readStatusString = "Błąd http: " + response.StatusCode.ToString();
                    return variableState;
                }             


                List<VariableState> variableStateList = response.Content.ReadAsAsync<List<VariableState>>().Result;
                return variableStateList[0];
            }
            catch (Exception e)
            {
                VariableState variableState = new VariableState();
                variableState.id = aVariableName;
                variableState.readSucceeded = false;
                variableState.readStatusString = "Błąd: " + e.Message;
                return variableState;
            }
        }



        /// <summary>
        /// Odczyt surowych wartości historycznych zmiennej.
        /// </summary>
        /// <param name="aVariableName">Nazwa zmiennej</param>
        /// <param name="aPeriodStartOpc">Początek okresu, z którego pobierane są dane; datę należy podać w formacie OPC.</param>
        /// <param name="aPeriodLengthOpc">Długość okresu, z którego pobierane są dane; długość należy podać w formacie OPC.</param>
        /// <param name="aArchiveType">Opcjonalny parametr określający typu archiwum, z którego należy pobrać wartości archiwalne.</param>
        /// <param name="aMaxNumberOfSamples">Opcjonalny parametr określający ile maksymalnie próbek może zwrócić funkcja.</param>
        /// <returns></returns>
        public VariableRawArchive ReadVariableRawArchive(string aVariableName, string aPeriodStartOpc, string aPeriodLengthOpc, string aArchiveType = "", int aMaxNumberOfSamples = 0)
        {
            try
            {
                String uri = "/asix/v1/variable/archive/raw";
                uri = QueryHelpers.AddQueryString(uri, "name", aVariableName);
                uri = QueryHelpers.AddQueryString(uri, "periodStart", aPeriodStartOpc);
                uri = QueryHelpers.AddQueryString(uri, "periodLength", aPeriodLengthOpc);
                if (!string.IsNullOrEmpty(aArchiveType))
                    uri = QueryHelpers.AddQueryString(uri, "archiveType", aArchiveType);
                if (aMaxNumberOfSamples > 0)
                    uri = QueryHelpers.AddQueryString(uri, "maxNumberOfSamples", aMaxNumberOfSamples.ToString());
                

                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    VariableRawArchive variableRawArchive = new VariableRawArchive();
                    variableRawArchive.id = aVariableName;
                    variableRawArchive.readSucceeded = false;
                    variableRawArchive.readStatusString = "Błąd http: " + response.StatusCode.ToString();
                    return variableRawArchive;
                }             


                return response.Content.ReadAsAsync<VariableRawArchive>().Result;
            }
            catch (Exception e)
            {
                VariableRawArchive variableRawArchive = new VariableRawArchive();
                variableRawArchive.id = aVariableName;
                variableRawArchive.readSucceeded = false;
                variableRawArchive.readStatusString = "Błąd: " + e.Message;
                return variableRawArchive;
            }
        }



        /// <summary>
        /// Odczyt zagregowanych wartości historycznych zmiennej.
        /// </summary>
        /// <param name="aVariableName">Nazwa zmiennej aplikacji Asix.Evo</param>
        /// <param name="aAggregate">Nazwa agregatu</param>
        /// <param name="aPeriodStartOpc">Początek okresu, z którego pobierane są dane; datę należy podać w formacie OPC.</param>
        /// <param name="aPeriodLengthOpc">Długość okres, za który wyliczany jest agregat; długość należy podać w formacie czasu OPC.</param>
        /// <param name="aResampleIntervalOpc">Długość interwału agregatu; długość należy podać w formacie OPC.</param>
        /// <param name="aArchiveType">Opcjonalny parametr określający typu archiwum, z którego należy pobrać wartości archiwalne.</param>
        /// <returns></returns>
        public VariableAggregateArchive ReadVariableAggregateArchive(string aVariableName, string aAggregate, 
            string aPeriodStartOpc, string aPeriodLengthOpc, string aResampleIntervalOpc, string aArchiveType = "")
        {
            try
            {
                String uri = "/asix/v1/variable/archive/processed";
                uri = QueryHelpers.AddQueryString(uri, "name", aVariableName);
                uri = QueryHelpers.AddQueryString(uri, "aggregate", aAggregate);
                uri = QueryHelpers.AddQueryString(uri, "periodStart", aPeriodStartOpc);
                uri = QueryHelpers.AddQueryString(uri, "periodLength", aPeriodLengthOpc);
                uri = QueryHelpers.AddQueryString(uri, "resampleInterval", aResampleIntervalOpc);
                if (!string.IsNullOrEmpty(aArchiveType))
                    uri = QueryHelpers.AddQueryString(uri, "archiveType", aArchiveType);


                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    VariableAggregateArchive variableAggregateArchive = new VariableAggregateArchive();
                    variableAggregateArchive.id = aVariableName + "/" + aAggregate;
                    variableAggregateArchive.readSucceeded = false;
                    variableAggregateArchive.readStatusString = "Błąd http: " + response.StatusCode.ToString();
                    return variableAggregateArchive;
                }             


                return response.Content.ReadAsAsync<VariableAggregateArchive>().Result;
            }
            catch (Exception e)
            {
                VariableAggregateArchive variableAggregateArchive = new VariableAggregateArchive();
                variableAggregateArchive.id = aVariableName + "/" + aAggregate;
                variableAggregateArchive.readSucceeded = false;
                variableAggregateArchive.readStatusString = "Błąd: " + e.Message;
                return variableAggregateArchive;
            }
        }



        /// <summary>
        /// Odczyt bieżącego stan alarmu. Dane pobierane są z logu alarmów aktywnych.
        /// </summary>
        /// <param name="aDomainName">Nazwa domeny alarmów</param>
        /// <param name="aAlarmName">Nazwa alarmu</param>
        /// <returns></returns>
        public AlarmState ReadAlarmState(string aDomainName, string aAlarmName)
        {
            try
            {
                String uri = "/asix/v1/alarm/state";
                uri = QueryHelpers.AddQueryString(uri, "domain", aDomainName);
                uri = QueryHelpers.AddQueryString(uri, "name", aAlarmName);


                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    AlarmState alarmState = new AlarmState();
                    alarmState.id = aDomainName + "/" + aAlarmName;
                    alarmState.readSucceeded = false;
                    alarmState.readStatusString = "Błąd http: " + response.StatusCode.ToString();
                    return alarmState;
                }             

                List<AlarmState> alarmStateList = response.Content.ReadAsAsync<List<AlarmState>>().Result;
                return alarmStateList[0];
            }
            catch (Exception e)
            {
                AlarmState alarmState = new AlarmState();
                alarmState.id = aDomainName + "/" + aAlarmName;
                alarmState.readSucceeded = false;
                alarmState.readStatusString = "Błąd: " + e.Message;
                return alarmState;
            }
        }


        class ServerError
        {
            public string Message;
        }



        /// <summary>
        /// Odczyt wartości historycznych alarmów. Do realizacji zapytania konieczne jest włączenie na serwerze aplikacji Asix.Evo, w opcjach domeny alarmów, archiwum SQL.
        /// </summary>
        /// <param name="aDomainName">Nazwa domeny alarmów</param>
        /// <param name="aPeriodStart">Początek okresu, w którym należy wyszukiwać alarmy; datę należy podać w formacie ISO8601 lub w lokalnym formacie czasu serwera.</param>
        /// <param name="aPeriodLength">Długość okresu, w którym należy wyszukiwać alarmy; długość okresu należy podać w formacie .NET.</param>
        /// <returns></returns>
        public HistAlarmArchive ReadHistAlarmArchive(string aDomainName, DateTime aPeriodStart, TimeSpan aPeriodLength)
        {
            try
            {
                String uri = "/asix/v1/alarm/archive";
                uri = QueryHelpers.AddQueryString(uri, "domain", aDomainName);
                uri = QueryHelpers.AddQueryString(uri, "periodStart", aPeriodStart.ToString("o"));
                uri = QueryHelpers.AddQueryString(uri, "periodLength", aPeriodLength.ToString());


                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        ServerError serverError = response.Content.ReadAsAsync<ServerError>().Result;

                        HistAlarmArchive histAlarmArchive = new HistAlarmArchive();
                        histAlarmArchive.readSucceeded = false;
                        histAlarmArchive.readStatusString = "Błąd: " + serverError.Message;
                        return histAlarmArchive;
                    }
                    catch (Exception)
                    {
                        HistAlarmArchive histAlarmArchive = new HistAlarmArchive();
                        histAlarmArchive.readSucceeded = false;
                        histAlarmArchive.readStatusString = "Błąd http: " + response.StatusCode.ToString();
                        return histAlarmArchive;
                    }
                }

                return response.Content.ReadAsAsync<HistAlarmArchive>().Result;
            }
            catch (Exception e)
            {
                HistAlarmArchive histAlarmArchive = new HistAlarmArchive();
                histAlarmArchive.readSucceeded = false;
                histAlarmArchive.readStatusString = "Błąd: " + e.Message;
                return histAlarmArchive;
            }
        }
    }
}

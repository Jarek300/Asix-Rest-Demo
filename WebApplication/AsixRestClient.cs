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



    /// <summary>
    /// Typ wyniku zwracanego przez funkcję ReadAttributeNames
    /// </summary>
    public class ServerAttributes
    {
        /// <summary>
        /// Wynik odczytu nazw atrybutu
        /// </summary>
        public bool readSucceeded = false;

        /// <summary>
        /// Tekstowy opis błędu odczytu
        /// </summary>
        public string readStatusString;

        /// <summary>
        /// Lista wartości atrybutów
        /// </summary>
        public List<string> mAttributeNameList;
    };


    /// <summary>
    /// Typ wyniku zwracanego przez funkcję ReadVariableState, reprezentuje stan jednej zmiennej
    /// </summary>
    public class VariableState
    {
        /// <summary>
        /// Nazwa zmiennej
        /// </summary>
        public string id = "";

        /// <summary>
        /// Wynik odczytu stanu zmiennej
        /// </summary>
        public bool readSucceeded = false;

        /// <summary>
        /// Tekstowy opis błędu odczytu,
        /// </summary>
        public string readStatusString;

        /// <summary>
        /// Stempel czasu wartości zmiennej
        /// </summary>
        public DateTime timeStamp = DateTime.UtcNow;

        /// <summary>
        /// Jakość OPC wartości zmiennej
        /// </summary>
        public uint quality = 0;

        /// <summary>
        /// Wartość zmiennej, typ pola zależy do typu zmiennej w aplikacji systemu Asix
        /// </summary>
        public object value = null;


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest dobra
        /// </summary>
        public bool IsQualityGood()
        {
            return (quality & 0xC0) == 0xC0;
        }


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest niepewna
        /// </summary>
        public bool IsQualityUncertain()
        {
            return (quality & 0xC0) == 0x40;
        }


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest zła
        /// </summary>
        public bool IsQualityBad()
        {
            return (quality & 0xC0) == 0;
        }
    };



    /// <summary>
    /// Reprezentuje stan jednej zarchiwizowanje próbki zmiennej
    /// </summary>
    public class VariableRawSample
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime t; // – początek interwału, dla którego wyliczono wartość agregatu,

        /// <summary>
        /// 
        /// </summary>
        public uint q;     // – jakość próbki,

        /// <summary>
        /// 
        /// </summary>
        public double v;   // – wartość próbki; wartość jest zawsze typu liczbowego.

        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest dobra
        /// </summary>
        public bool IsQualityGood()
        {
            return (q & 0xC0) == 0xC0;
        }


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest niepewna
        /// </summary>
        public bool IsQualityUncertain()
        {
            return (q & 0xC0) == 0x40;
        }


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest zła
        /// </summary>
        public bool IsQualityBad()
        {
            return (q & 0xC0) == 0;
        }
    };



    /// <summary>
    /// Typ wyniku zwracanego przez funkcję ReadVariableRawArchive, reprezentuje fragment archiwum wartości surowych jednej zmiennej
    /// </summary>
    public class VariableRawArchive
    {
        /// <summary>
        /// Nazwa zmiennej i nazwa agregatu oddzielone ukośnikiem
        /// </summary>
        public string id;

        /// <summary>
        /// Wynik odczytu wartości historycznych zmiennej
        /// </summary>
        public bool readSucceeded;

        /// <summary>
        /// Tekstowy opis błędu odczytu
        /// </summary>
        public string readStatusString; 

        /// <summary>
        /// Flata informująca, że w podanym okresie znajduje się więcej próbek niż podano w parametrze maxNumberOfSamples,
        /// </summary>
        public bool moreDataAvailable;  

        /// <summary>
        /// Początek okresu, dla którego pobrano dane,
        /// </summary>
        public DateTime periodStartTime; 

        /// <summary>
        /// Koniec okresu, dla którego pobrano dane
        /// </summary>
        public DateTime periodEndTime; 

        /// <summary>
        /// Tablica wartości archiwalnych
        /// </summary>
        public VariableRawSample[] samples;
    };




    /// <summary>
    /// Reprezentuje stan jednej zagregowanej próbki zmiennej
    /// </summary>
    public class VariableAggregateSample
    {
        /// <summary>
        /// Początek interwału, dla którego wyliczono wartość agregatu,
        /// </summary>
        public DateTime t;

        /// <summary>
        /// Koniec interwału, dla którego wyliczono wartość agregatu
        /// </summary>
        public DateTime e;

        /// <summary>
        /// Jakość próbki
        /// </summary>
        public uint q;     

        /// <summary>
        /// Wartość próbki; wartość jest zawsze typu liczbowego
        /// </summary>
        public double v;  


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest dobra
        /// </summary>
        public bool IsQualityGood()
        {
            return (q & 0xC0) == 0xC0;
        }


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest niepewna
        /// </summary>
        public bool IsQualityUncertain()
        {
            return (q & 0xC0) == 0x40;
        }


        /// <summary>
        /// Funkcja pozwala sprawdzić czy jakość zmiennej jest zła
        /// </summary>
        public bool IsQualityBad()
        {
            return (q & 0xC0) == 0;
        }
    };


    /// <summary>
    /// Typ wyniku zwracanego przez funkcję ReadVariableAggregateArchive, reprezentuje fragment archiwum wartości agregowanych jednej zmiennej
    /// </summary>
    public class VariableAggregateArchive
    {
        /// <summary>
        /// Nazwa zmiennej i nazwa agregatu oddzielone ukośnikiem
        /// </summary>
        public string id; 

        /// <summary>
        /// Wynik odczytu wartości historycznych zmiennej
        /// </summary>
        public bool readSucceeded; 

        /// <summary>
        /// Tekstowy opis błędu odczytu
        /// </summary>
        public string readStatusString; 

        /// <summary>
        /// Początek okresu, dla którego pobrano dane
        /// </summary>
        public DateTime periodStartTime; 

        /// <summary>
        /// Koniec okresu, dla którego pobrano dane
        /// </summary>
        public DateTime periodEndTime; 

        /// <summary>
        /// Tablica wartości archiwalnych
        /// </summary>
        public VariableAggregateSample[] samples; 
    };



    /// <summary>
    /// Typ wyniku funkcji ReadAlarmState. Reprezentuje stan jednego alarmu
    /// </summary>
    public class AlarmState
    {
        /// <summary>
        /// Identyfikator alarmu(domena i nazwa alarmu oddzielone ukośnikiem)
        /// </summary>
        public string id;

        /// <summary>
        /// Wynik odczytu stany alarmu 
        /// </summary>
        public bool readSucceeded;

        /// <summary>
        /// Tekstowy opis błędu odczytu
        /// </summary>
        public string readStatusString;  

        /// <summary>
        /// Flaga określający czy alarm jest aktualnie aktywny
        /// </summary>
        public bool active;   

        /// <summary>
        /// Opis stanu alarmu
        /// </summary>
        public string stateDescription;    

        /// <summary>
        /// Flaga określający czy dostępna jest informacja o potwierdzeniu alarmu
        /// </summary>
        public bool ackAvailabe;   

        /// <summary>
        /// Flaga określający czy alarm jest potwierdzony
        /// </summary>
        public bool ack;           
    };


    /// <summary>
    /// Klasa reprezentuje stana jednego zapisu w archiwum alarmów
    /// </summary>
    public class HistAlarmState
    {
        /// <summary>
        /// Nazwa alarmu
        /// </summary>
        public string name;

        /// <summary>
        /// Opis alarmu
        /// </summary>
        public string description; 

        /// <summary>
        /// Priorytet alarmu
        /// </summary>
        public string priority; 

        /// <summary>
        /// Wartość true, jeśli alarm ma tylko początek
        /// </summary>
        public bool startOnly; 

        /// <summary>
        /// Data początku alarmu
        /// </summary>
        public DateTime startTime; 

        /// <summary>
        /// Data detekcji początku alarmu
        /// </summary>
        public DateTime startDetectTime; 

        /// <summary>
        /// Data końca alarmu; null jeśli alarm się nie zakończył
        /// </summary>
        public DateTime? endTime; 

        /// <summary>
        /// Data detekcji końca alarmu; null jeśli alarm się nie zakończył
        /// </summary>
        public DateTime? endDetectTime; 

        /// <summary>
        /// Flaga określająca czy alarm jest potwierdzony
        /// </summary>
        public bool ack; 

        /// <summary>
        /// Nazwa użytkownika, który potwierdził alarm
        /// </summary>
        public string ackUser; 

        /// <summary>
        /// Nazwa komputera, na którym potwierdzono alarm
        /// </summary>
        public string ackStation;

        /// <summary>
        /// Data potwierdzenia alarmu
        /// </summary>
        public DateTime? ackTime; 

        /// <summary>
        /// Notatka potwierdzenia alarmu
        /// </summary>
        public string ackNote; 
    }


    /// <summary>
    /// Typ wyniku funkcji ReadHistAlarmArchive. Reprezentuje stan zbioru alarmów pobranego z archiwum alarmów
    /// </summary>
    public class HistAlarmArchive
    {
        /// <summary>
        /// Wynik odczytu z archiwum alarmów
        /// </summary>
        public bool readSucceeded;

        /// <summary>
        /// Tekstowy opis błędu odczytu
        /// </summary>
        public string readStatusString; 

        /// <summary>
        /// Flaga określający, czy w podanym okresie znajduje się więcej alarmów niż podano w parametrze limit
        /// </summary>
        public bool moreDataAvailable; 

        /// <summary>
        /// Tablica obiektów wartości archiwalnych alarmów
        /// </summary>
        public HistAlarmState[] alarms; 
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

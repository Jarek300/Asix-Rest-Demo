# Opis
Repozytorium **Asix-REST-Demo** zawiera przykłady pobierania danych procesowych z serwera REST aplikacji Asix.Evo i prezentowania ich na stronach Web. 

[Asix.Evo](https://www.asix.com.pl) firmy [Askom](https://www.askom.pl) jest pakietem programowym klasy HMI/SCADA/MES umożliwiającym realizację komputerowych systemów wizualizacji, nadzoru i sterowania procesów przemysłowych. Asix.Evo ma możliwość udostępniania danych procesowych aplikacji m.in. za pośrednictwem serwera REST. Opis API serwera REST jest dostępny [tutaj](https://www.askom.pl/WebHelp/Asix_Evo_9/AsixConnect_HTML5/index.htm#t=Serwer_REST_pakietu_Asix_Evo%2Fwstep.htm).

Projektanci często stają przed zadaniem stworzenia dla użytkownika programu, który czyta dane z wielu źródeł danych i wyświetla w syntetyczny sposób. Może to być program dla systemu Windows, appka na urządzenia mobilne lub portal internetowy. Przykładem może być strona WWW - portal biurowca - prezentujący pracownikom informacje o działaniu infrastruktury budynku. Jeśli jednym ze źródeł danych jest aplikacja Asix.Evo to zalecanym sposobem pobierania z niej danych jest użycie serwera REST. Serwer REST jest wbudowany w Asix.Evo. Opis konfiguracji serwera REST jest dostępny [tutaj](https://www.askom.pl/WebHelp/Asix_Evo_9/AsixConnect_HTML5/index.htm#t=Serwer_REST_pakietu_Asix_Evo%2FUruchomienie_i_konfiguracja.htm).

Działanie programu - klienta REST - sprowadza się do wywołania odpowiedniej usługi serwera REST, odebrania odpowiedzi i wyświetlenia  danych zawartych w odpowiedzi. Obecnie praktycznie wszystkie języki programowania i wszystkie platformy tworzenia aplikacji mają wbudowane mechanizmy wywoływania usług serwerów REST. 


# Aplikacja WebApplication

Aplikacja **WebApplication** jest napisana w języku C# i działa na platformie .NET 6. Do tworzenia stron WWW użyta została biblioteka [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/getting-started/?view=aspnetcore-6.0). Strony WWW działają zgodnie z modelem [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-6.0). Do formatowanie elementów stron używana jest bilioteka [Bootstrap](https://getbootstrap.com/).

 * [Attribute/Demo1](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Attribute) - odczyt wybranych atrybutów zmiennej
 * [Attribute/Demo2](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Attribute) - odczyt wszystkich atrybutów  zmiennej
 * [Variable/Demo1](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Variable) - odczyt wartości bieżącej jednej zmiennej
 * [Variable/Demo2](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Variable) - odczyt wartości bieżącej wielu zmiennych
 * [Variable/Demo3](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Variable) - odczyt wartości bieżącej wielu zmiennych, sygnalizacja przekroczeń limitów, sygnalizacja trendu zmian wartości
 * [Variable/Demo4](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Variable) - zapis wartości bieżącej zmiennej
 * [VariableDynamic/Demo1](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/VariableDynamic) - odświeżanie wartości bieżącej wielu zmiennych bez przeładowywania całej strony
 * [VariableDynamic/Demo2](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/VariableDynamic) - odświeżanie wartości bieżącej z użyciem ajax/json/jQuery
 * [Archiwum/Demo1](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Archive) - odczyt wartości historycznych agregowanych dwu zmiennych i wyświetlenie w postaci tabeli
 * [Archiwum/Demo2](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Archive) - odczyt wartości historycznych surowych jednej zmiennych i wyświetlenie w postaci wykresu
 * [Alarm/Demo1](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Alarm) - odczyt wartości bieżącej wielu alarmów
 * [Alarm/Demo2](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/WebApplication/Pages/Alarm) - odczyt wartości historycznych z archiwum alarmów
 * [AsixRestClient.cs](https://github.com/Jarek300/Asix-Rest-Demo/blob/master/WebApplication/AsixRestClient.cs) - Klasa pomocnicza realizująca odczyt danych z serwera REST Asix.Evo.

Działająca aplikacja jest dostępna pod adresem [https://asixrestdemo.azurewebsites.net](https://asixrestdemo.azurewebsites.net)


# Aplikacja BlazorWebAssemblyApp

Aplikacja **BlazorWebAssemblyApp** jest napisana w języku C# i działa na platformie .NET 6. Do tworzenia stron WWW użyta została biblioteka [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/getting-started/?view=aspnetcore-6.0). Strony WWW działają zgodnie z modelem [Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0).

 * [VariableDemo](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/BlazorWebAssemblyApp/Pages/VariableDemo.razor) - odczyt i odświeżanie wartości bieżących zmiennych
 * [AlarmDemo](https://github.com/Jarek300/Asix-Rest-Demo/tree/master/BlazorWebAssemblyApp/Pages/AlarmDemo.razor) - odczyt i odświeżanie wartości bieżącej  alarmów



# Wymagania
Aby móc skompilować projekt należy zainstalować [Visual Studio 2022](https://visualstudio.microsoft.com/pl/downloads) oraz [.NET 6 SDK](https://dotnet.microsoft.com/download).

# Description
The Asix-Rest-Demo programm contains examples of reading process data from the REST server of Asix.Evo application and presenting them on Web pages.

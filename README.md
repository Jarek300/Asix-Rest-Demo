# Opis
Aplikacja **Asix-AspNetMvcCore-Demo** zawiera przykłady pobierania danych procesowych z serwera REST aplikacji Asix.Evo i umieszczania ich na stronach Web. 

[Asix.Evo](https://www.asix.com.pl) firmy [Askom](https://www.askom.pl) jest pakietem programowym klasy HMI/SCADA/MES umożliwiającym realizację komputerowych systemów wizualizacji, nadzoru i sterowania procesów przemysłowych. Asix.Evo ma możliwość udostępniania danych procesowych aplikacji m.in. za pośrednictwem serwera REST. Opis serwera REST jest dostepny [tutaj](https://www.askom.pl/WebHelp/Asix_Evo_9/AsixConnect_HTML5/index.htm#t=Serwer_REST_pakietu_Asix_Evo%2FHistoria_alarmow.htm).

Projektanci często stają przed zadaniem stworzenia dla użytkownika aplikacji, która łączy dane z wielu źródeł danych i wyświetla je dla użytkownika w syntetyczny sposób. Mogą to być aplikacje dla systemu Windows, aplikacje internetowe lub aplikacje na urządzenia mobilne. Przykładem jest strona WWW - portal biurowca - prezentujący pracownikom informacje o działaniu infrastruktury budynku.

Aplikacja **Asix-AspNetMvcCore-Demo** ma pomóc użytkownikom tworzącym własne aplikacje, które to aplikacje muszą wyświetlić dane pochodzące z różnych źródeł w tym z aplikacji Asix.Evo. w dowolnej technologii

Aplikacja **Asix-AspNetMvcCore-Demo** jest stworzona przy pomocy technologii Asp.Net MVC Core 2.2 oraz biblioteki Bootstrap. Aby móc skompilować projekt należy zainstalować [Visual Studio 2017](https://visualstudio.microsoft.com/pl/downloads) oraz [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download).


Zawartość projektu:
 * Attribute/Demo1 - Odczyt wybranych atrybutów zmiennej
 * Attribute/Demo2 - Odczyt wszystkich atrybutów  zmiennej
 * Variable/Demo1 - Odczyt wartości bieżącej jednej zmiennej
 * Variable/Demo2 - Odczyt wartości bieżącej wielu zmiennych
 * Variable/Demo3 - Odczyt wartości bieżącej wielu zmiennych, sygnalizacja przekroczeń limitów, sygnalizacja trendu zmian wartości
 * VariableDynamic/Demo1 - Odświeżanie wartości bieżącej wielu zmiennych bez przeładowywania całej strony
 * VariableDynamic/Demo2 - Odświeżanie wartości bieżącej z użyciem ajax/json/jQuery
 * Archiwum/Demo1 - Odczyt wartości historycznych agregowanych dwu zmiennych i wyświetlenie w postaci tabeli
 * Archiwum/Demo2 - Odczyt wartości historycznych surowych jednej zmiennych i wyświetlenie w postaci wykresu
 * Alarm/Demo1 - Odczyt wartości bieżącej wielu alarmów
 * Alarm/Demo2 - Odczyt wartości historycznych z archiwum alarmów
 * AsixRestClient.cs - Klasa pomocnicza realizująca odczyt danych z serwera REST.

Działająca aplikacja jest dostępna pod adresem [https://asixrestdemo.azurewebsites.net](https://asixrestdemo.azurewebsites.net)

# Description
Examples of displaying process data of the Asix.Evo application in a Web application.
The Web application is created using the Asp.Net MVC Core 2.2 technology.

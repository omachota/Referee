# Technická dokumentace

Zde se nachází technická dokumentace a také zde najdete různé technické poznámky k aplikaci.

### O aplikaci

Jedná se o WPF aplikaci běžící zatím na `net6.0-windows`. Všechny zdrojové soubory lze nalézt v adresáři `/Referee/`.

Celá aplikace je postavená nad knihovnou `MaterialDesignInXaml`. Výplatní listina je vygenerované PDF pomocí knihovny `itext7`.

Aplikace hojně využívá návrhového stylu MVVM:

- Model s `INotifyPropertyChanged`: `/Referee/Models/`, Třídy s daty 
- ViewModel: `/Referee/ViewModels/`, podklad pro GUI
- View: `/Referee/Views`, samotné GUI s bindingem

Main Entry: `App.xaml.cs`: `OnStartup`

### Model

Adresář: `/Referee/Models/`

- Hlavním prvkem je interface `IPerson`, který implementuje třída `Person`, ze které dědí třídy `Cetar` a `Rozhodci`.
- Poslední dvě zmíněné třídy obsahují statickou metodu `CreateEmpty`, která slouží pro pohodlnější vytváření nových instancí.
- Je nutné, aby třída `Person` dědila `AbstractNotifyPropertyChanged`, jinak se nebudou data automaticky propisovat do GUI. 

### ViewModel

Adresář: `/Referee/ViewModels/`

- Základem je `BaseViewModel`, který skrze `FilterCollection` poskytuje podporu pro vyhledávání jmen. K `FilterCollection` je totiž možné přistupovat z vyšších vrstev aplikace.
- ViewModely `CetaViewModel` a `RozhodciViewModel`
  - Nelze sloučit, protože `App.xaml` nepodporuje generika - existuje [MarkupExtension](http://10rem.net/blog/2011/03/09/creating-a-custom-markup-extension-in-wpf-and-soon-silverlight)
  - Jejich částečná duplicita je přenesena do `PersonViewModel`, ze které oba zmíněné ViewModely dědí
  - Zpomalují zobrazení osob, kvůli problémům se záseky při načítání dat
- Tlačítka v GUI jsou propojená skrze Commandy (třída `/Referee/Infrastructure/Command.cs`)
- `DialogSwitchViewModel` obsluhuje editor, ve kterém se vytváří/edituje rozhodčí/četař.
- `RozhodciViewModel`, `CetarViewModel`, `SettingsViewModel` podporují vrácení všech změn do původního stavu před editací

### Views

Adresář: `/Referee/Views/`

- Obsahuje všechna okna, která aplikace používá.

### Nastavení

- Adresář `/Referee/Infrastructure/SettingsFd`

Třída `SettingsHelper` slouží pro ukládání a čtení nastavení.

Třída `Settings` obsahuje všechna možná nastavení.

Nastavení se ukládá do souboru `%appdata%\OndrejMachota\Referee\Settings.json` při zavření aplikace.

### Databáze

Je možné použít MySQL, ke které se lze vzdáleně připojit, či lokální SQLite. Struktury tabulek jsou v souboru `/Referee/db_structure.md`.

Soubor SQLite databáze je distribuován v instalátoru. Při startu aplikace je zkontrolováno, zda se nachází v `%appdata%\OndrejMachota\Referee\`, případně je tam zkopírován. 

- Adresář `/Referee/Infrastructure/DataServices`

K přístupu do databáze je využíváno knihovny `Dapper`. Třída `DapperContext` umožňuje vytvářet nové připojení k databázi v závislosti na aktuálním nastavení.

### Vyhledávání osob

- při změně ve vyhledávacím políčku se vygeneruje regex
- změna se projevuje po 400 ms od posledního stisku, aby mohl uživatel psát a vyhledávání se pořád neobnovovalo
- Třída `Rozhodci` a `Cetar` obsahují `FullNameInverted`, aby bylo možné hledat ve stylu `Příjmení Jméno`.

### Instalátor

Aplikace používá `Inno Setup` instalátor. Ten je generován ze souboru `/Referee/setup.iss` .

1. Pro vytvoření instalátoru je potřeba aplikace nejdříve publikovat do složky `...\Referee\bin\Release\net6.0-windows\win-x64\publish\`. 

2. Zkontroluje, že se v adresáři `...\Referee\bin\Release\net6.0-windows\win-x64\publish\` nachází soubor `SQLite.Interop.dll`. Pokud ne, nachází se nejspíše hlouběji ve složce `.../publish/`. Následně je třeba jej zkopírovat přímo do složky `.../publish/`. Bez tohoto souboru nebude fungovat lokální SQLite databáze.
3. Poté lze spustit skript `/Referee/setup.iss`.

### Aktualizace

- Adresář `/Referee/Infrastructure/Update`

Statická třída `Updater` je zavolána při startu. Její účel je zkontrolovat, jaká verze byla vydána poslední, porovnat s aktuální instancí, případně stáhnout novou verzi a otevřít instalátor. 

Textová neshoda ve verzích vyústí ve stažení nejnovější vydané verze a zobrazení upozornění.

### Přepínání oken

- Adresář: `/Referee/Infrastructure/WindowNavigation`

Řešeno skrze třídu `WindowManager`, která zajištuje přepínaní oken pomocí `UpdateWindowCommand`. 

### Výplatní listiny

- Adresář: `/Referee/Infrastructure/`

Jedná se o vygenerováné PDF soubory pomocí knihovny `itext7` - třída `Priter`. PO uložení je soubor otevřen ve webovém prohlížeči. To zajišťuje třída `Browser`.

Dialog o tištění není možné automaticky otevírat: [zdroj](https://stackoverflow.com/questions/57895126/chrome-77-not-auto-printing-pdfs)

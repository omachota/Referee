# Technická dokumentace

Zde najdete různé technické poznámky k aplikaci.

### O aplikaci

Jedná se o WPF MVVM aplikaci běžící na `net6.0-windows`. Zdrojové soubory lze nalézt v adresáři `/Referee/`. 

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

Textová neshoda ve verzích vyústí ve stažení nejnovější vydané verze.

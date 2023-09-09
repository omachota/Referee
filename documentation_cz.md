# Dokumentace aplikace Referee

Uživatelská dokumentace aplikace Referee. Technickou dokumentaci najdete v souboru `technical_documentation_cz.md`.

### Navigační menu

Navigační menu umožňuje pohyb mezi jednotlivými okny a nastavením. 

- Okno Rozhodčí
- Okno Technická Četa
- Okno Nastavení

Navigační menu lze rozšířit kliknutím na tlačítko v levém horním rohu. Kliknutím na stejné tlačítko či pohybem myši mimo menu se menu zavře.

Při startu je automaticky otevřeno okno Nastavení. To lze zavřít skrze navigační menu či kliknutím mimo toto okno. 

### Rozhodčí

Hlavním prvkem v tomto okně je tabulka a menu vpravo od tabulky. 

##### Tabulka

Rozhodčí jsou v tabulce pro snazší hledání seřazeni podle příjmení. Pro hledání zle využít políčka `Hledat rozhodčího/četaře` - viz Vyhledávání osob.

Pomocí zaškrtávacího tlačítka se vybírají rozhodčí na výplatní listinu. 

Data v tabulce nelze editovat přímo. Tlačítkem `Edit` v příslušném řádku otevřeme editační okno, tlačítko `Koš` slouží pro smazání daného rozhodčího.

- Akci smazání rozhodčího je třeba potvrdit v zobrazeném okně.

  - `Smazat` - rozhodčí bude navždy smazán

  - `Zavřít` - okno se zavře a rozhodčí zůstane

- Akci editace je třeba potvrdit, jinak se data neuloží.

Jediný údaj, který je možné z tabulky editovat, je `Odměna`. Lze zadat libovolnou celočíselnou hodnotu.

##### Menu

- Tlačítko `Tiskárna` vytvoří a otevře soubor s výplatní listinou, která bude obsahovat vybrané rozhodčí.
  - Pod tlačítkem `Tiskárna` se zobrazuje počet vybraných rozhodčích.
  - Soubor se otevře automaticky ve webovém prohlížeči.
- Tlačítko `Čistý tisk` vytvoří a otevře soubor s prázdnou výplatní listinou. Výběrem vedle tlačítka volíme počet stránek.
- Tlačítko `Přidat` nastaví vybraným rozhodčím částku zvolenou v políčku vlevo.
- Tlačítko `Osoba` slouží k vytvoření nového rozhodčího.
  - Novému rozhodčímu stačí vyplnit pouze `Jméno`, `Příjmení` a `Datum narození`. Ostatní políčka mohou být prázdná.
    - Za `Datum narození` nelze zvolit aktuální datum. 

### Četa

Toto okno je vesměs podobné oknu s rozhodčími, pro četaře nelze vyplnit doplňující údaje jako telefon, číslo účtu, atd.

Výplatní listiny technické čety jsou unikátně označeny, aby se nepletly s výplatními listinami pro rozhodčí.

### Vyhledání osob

V pravém horním rohu je umístěno vyhledávací pole, přes které je možné vyhledávat osoby. Je třeba zadat alespoň 3 znaky, jinak se nic nestane. V případě nálezu se pod vyhledávacím polem zobrazí subtilní tabulka, ve které zle označit osobu.

Hledá se pouze mezi načtenými osobami v tabulce.

Hledání je poměrně chytré, stačí zadat např. iniciály doplněné o pár následujících písmen či přestat uprostřed jména a pokračovat příjmením či vice versa.

### Nastavení

V nastavení je možné upřesnit název oddílu a informace o závodu, ke kterému tisknete výplatní listinu. Dále se v nastavení vybírá a nastavuje přístup k databázi.

##### Automaticky vyplňované údaje

Zde nastavujeme údaje, které se propíší do výplatní listiny. Údaj se objeví na výplatní listině tehdy, když bude zaškrtnuté příslušné tlačítko (zobrazí se před ním fajfka a zmodrá).

Nezaškrtnuté údaje nelze editovat.

Je-li údaj `Datum začátku konaní soutěže` starší než aktuální datum, je při startu přepsán na aktuální datum.

##### Databáze

Viz sekce Databáze.

##### Ostatní

Tlačítko `zahodit změny` vrátí nastavení do původního stavu - tj. při otevření nastavení.

### Výplatní listina

Jedná se o pdf soubor s nadpisem, tabulkou a dodatečnými informacemi k soutěži.

##### Celkem vyplaceno

Hodnota `CELKEM VYPLACENO:` se počítá pro každou stranu samostatně. Chybí-li u někoho na straně údaj `Odměna`, celková hodnota zůstane prázdná.

### Databáze

Aplikace umí pracovat jak s MySQL databází, tak i s lokální SQLite databází. Typ databáze lze zvolit v nastavení. Pro použití lokální databáze stačí pouze odškrtnout zaškrtávací políčko `Použít externí databázi`. Jinak je třeba vyplnit přístupové údaje k databázi. 

Údaje jsou zatím ukládány nešifrované.  

### Aktualizace

Aplikace si po startu zkontroluje, zda není dostupná novější verze. V případě že ano, přijde oznámení o stahování nové verze a po stažení se automaticky otevře instalátor.

# Dokumentace aplikace Referee

### Navigační menu

Navigační menu umožňuje pohyb mezi jednotlivými okny a nastavením. Nastavení se při startu aplikace a není problém jej otevřít už s vybranými osobami. O výběr nepřijdete.

### Rozhodčí

Hlavním prvek v tomto okně je tabulka. Pomocí zaškrtávátka se vybírají rozhodčí na výplatní listinu, tlačítkem ``Edit`` v řádku editujeme rozhodčího, tlačítko s ikonou koše slouží pro smazání.

##### Pravé menu

- Tlačítko ``Tiskárna`` vytvoří a otevře soubor s výpatní listinou.
- Pod tlačítkem se zobrazuje počet vybraných rozhodčích
- Tlačítko ``Čistý tisk`` vytvoří a otevře soubor s prázdnou výplatní listinou. Výberem vedle tlačítka volíme počet stránek.
- Tlačítko ``Přidat`` nastaví vybraným rozhodčím částku zvolenou v políčku nalevo.
- Tlačítko s osobou slouží k vytvoření nového rozhodčího.
  - Novému rozhodčímu stačí vyplnit pouze ``jméno``, ``příjmení`` a ``datum narození``. Ostatní políčka mohou být prázdná.

### Četa

Toto okno je vesměs podobné oknu s rozhodčími, pro četaře nelze vyplnit doplňující údaje jako telefon, číslo účtu, atd.

Výplatní listiny technické čety jsou unikátně označeny, aby se nepletly s výplatními listinami pro rozhodčí.

### Nastavení

V nastavení je možné upřesnit název oddílu a informace o závodu, ke kterému tisknete výplatní listinu. Dále se v nastavení vybírá a nastavuje přístup k databázi.

Údaj se propíše do výplatní listiny pouze, když bude zaškrtnuté příslušné tlačítko (zobrazí se před ním fajfka a zmodrá). 

Tlačítko ``zahodit změny`` vrátí nastavení do původního stavu.

### Vyhledání osob

V pravém horním rohu je umístěno vyhledávací pole, přes které je možné vyhledávat osoby. Je třeba zadat alespoň 3 znaky, jinak se nic nestane. V přídadě shody se pod vyhledávacím polem zobrazí subtilní tabulka, ve které zle označit osobu za vybranou.

### Databáze

Aplikace umí pracovat jak s MySQL databází, tak i s lokální SQLite databází. Typ databáze lze zvolit v nastavení. Pro použití lokální databáze stačí pouze odškrtnout zaškrtávací políčko ``Použít externí databázi``. Jinak je třeba vyplnit přístupové údaje k databázi.

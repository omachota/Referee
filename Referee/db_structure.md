# Rozhodci

``` sqlite
CREATE TABLE "Rozhodci" (
	"Id"	INTEGER NOT NULL,
	"FirstName"	TEXT NOT NULL,
	"LastName"	TEXT NOT NULL,
	"BirthDate"	TEXT NOT NULL,
	"Address"	TEXT,
	"City"	TEXT,
	"Email"	TEXT,
	"Class"	TEXT,
	"TelephoneNumber"	TEXT,
	"RegistrationNumber"	TEXT,
	"BankAccountNumber"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
```

### Ceta

``` sqlite
CREATE TABLE "Ceta" (
	"Id"	INTEGER NOT NULL,
	"FirstName"	TEXT NOT NULL,
	"LastName"	TEXT NOT NULL,
	"BirthDate"	TEXT NOT NULL,
	"Address"	TEXT,
	"City"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
```

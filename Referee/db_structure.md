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

###### MySQL

```mysql
CREATE TABLE `referee`.`rozhodci` (
    `Id` INT NOT NULL AUTO_INCREMENT ,
    `FirstName` TEXT NOT NULL ,
    `LastName` TEXT NOT NULL , 
    `BirthDate` TEXT NOT NULL , 
    `Address` TEXT NULL , 
    `City` TEXT NULL ,
    `Email` TEXT NULL ,
    `Class` TEXT NULL ,
    `TelephoneNumber` TEXT NULL ,
    `RegistrationNumber` TEXT NULL ,
    `BankAccountNumber` TEXT NULL ,
    PRIMARY KEY (`Id`))
ENGINE = InnoDB;
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

###### MySQL

```mysql
CREATE TABLE `referee`.`ceta` (
    `Id` INT NOT NULL AUTO_INCREMENT ,
    `FirstName` TEXT NOT NULL ,
    `LastName` TEXT NOT NULL , 
    `BirthDate` TEXT NOT NULL , 
    `Address` TEXT NULL , 
    `City` TEXT NULL , 
    PRIMARY KEY (`Id`))
ENGINE = InnoDB;
```

### MySQL

Databázi je nutné nastavit tak, aby se k ní mohl připojit počítač, kde je nainstalovaná aplikace Referee. Tj. je nutné přidat uživatele s heslem a nastavit patřičná oprávnění.

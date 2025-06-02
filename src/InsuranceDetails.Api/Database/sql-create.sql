-- Create tables without constraints
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Citizen')
CREATE TABLE Citizen (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Bsn NVARCHAR(MAX) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HealthInsurer')
CREATE TABLE HealthInsurer (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    ApiKey NVARCHAR(MAX) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Log')
CREATE TABLE Log (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Action NVARCHAR(100) NOT NULL,
    Bsn NVARCHAR(10) NOT NULL,
    UserId INT NOT NULL,
    WhenDateTime DATETIME NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SupplementaryHealthInsurance')
CREATE TABLE SupplementaryHealthInsurance (
    Id INT PRIMARY KEY IDENTITY(1,1),
    HealthInsurerId INT NOT NULL,
    CitizenId INT NOT NULL,
    AsFromDate DATETIME NOT NULL,
    TillDate DATETIME NOT NULL,
    WhatIsCovered NVARCHAR(MAX) NOT NULL,
    PercentageCovered INT NOT NULL,
    MaxAmount INT NOT NULL,
    CONSTRAINT FK_SupplementaryHealthInsurance_HealthInsurer FOREIGN KEY (HealthInsurerId) REFERENCES HealthInsurer(Id),
    CONSTRAINT FK_SupplementaryHealthInsurance_Citizen FOREIGN KEY (CitizenId) REFERENCES Citizen(Id)

);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BasicHealthInsurance')
CREATE TABLE BasicHealthInsurance (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CitizenId INT NOT NULL,
    HealthInsurerId INT NOT NULL,
    AsFromDate DATETIME NOT NULL,
    TillDate DATETIME NOT NULL,
    CONSTRAINT FK_BasicHealthInsurance_Citizen FOREIGN KEY (CitizenId) REFERENCES Citizen(Id),
    CONSTRAINT FK_BasicHealthInsurance_HealthInsurer FOREIGN KEY (HealthInsurerId) REFERENCES HealthInsurer(Id)
);
﻿CREATE TABLE Contacts (
	Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Subject NVARCHAR(255),
    Body NVARCHAR(MAX)
);

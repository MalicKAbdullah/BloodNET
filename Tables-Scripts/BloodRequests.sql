﻿CREATE TABLE BloodRequests (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BloodGroup NVARCHAR(10) NOT NULL,
    DTime DATETIME NOT NULL,
    RecipientName NVARCHAR(100) NOT NULL,
    RecipientPhone NVARCHAR(20) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Status INT NOT NULL DEFAULT 1,
    userId NVARCHAR(50) NOT NULL

);

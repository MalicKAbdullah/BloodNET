CREATE TABLE [dbo].[Respondants]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [BloodDonors] NVARCHAR(50) NULL, 
    [BloodRequests] NVARCHAR(50) NULL, 
    [DateTimes] DATETIME NULL,

)

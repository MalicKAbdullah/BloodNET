CREATE TABLE Donation (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DonorId NVARCHAR(50) NOT NULL,
    RequestId INT NOT NULL,
    Created DATETIME NOT NULL,
    Status INT NOT NULL,
    FOREIGN KEY (DonorId) REFERENCES BloodDonors(DonorId),
    FOREIGN KEY (RequestId) REFERENCES BloodRequests(Id)
);

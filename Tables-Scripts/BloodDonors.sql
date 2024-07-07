CREATE TABLE BloodDonors (
    DonorId NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    DOB DATE NOT NULL,
    Gender NVARCHAR(10),
    Weight FLOAT,
    WeightUnit NVARCHAR(20),
    BloodGroup NVARCHAR(10),
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(255),
    City NVARCHAR(100),
    Country NVARCHAR(100),
    MedicalHistory NVARCHAR(MAX),
    DonorStatus INT,
    ImgUrl NVARCHAR(255)
);

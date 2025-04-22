CREATE DATABASE LOANMANAGEMENTSYSTEM;
USE LOANMANAGEMENTSYSTEM

CREATE TABLE Customer (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    EmailAddress NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(200),
    CreditScore INT
);


CREATE TABLE Loan (
    LoanId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT FOREIGN KEY REFERENCES Customer(CustomerId),
    PrincipalAmount DECIMAL(18,2),
    InterestRate FLOAT,
    LoanTerm INT,
    LoanType NVARCHAR(50),
    LoanStatus NVARCHAR(20)
);


CREATE TABLE HomeLoan (
    LoanId INT PRIMARY KEY, FOREIGN KEY (LoanId) REFERENCES Loan(LoanId),
    PropertyAddress NVARCHAR(200),
    PropertyValue INT
);


CREATE TABLE CarLoan (
    LoanId INT PRIMARY KEY, FOREIGN KEY (LoanId) REFERENCES Loan(LoanId),
    CarModel NVARCHAR(100),
    CarValue INT
);


INSERT INTO Customer (Name, EmailAddress, PhoneNumber, Address, CreditScore) VALUES
('Naveen Kumar', 'naveen.j@gmail.com', '9876543210', '45 Elm Street, chennai', 720),
('Sanjay kumar', 'sanjay.kumar@outlook.com', '9001122334', '23 King’s Road, pune', 680),
('Manoj dev', 'manoj.dev@hotmail.com', '7788990011', '10 Raja street, Kerala', 640),
('Ashwini g', 'ashwini.g@gmail.com', '8123456789', '87 High Street, Goa', 710),
('Geetha samynathan', 'geetha.samynathan@yahoo.com', '7008009001', '99 Gandhi Avenue,Chennai', 600);

INSERT INTO Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) VALUES
(1, 200000, 8.5, 24, 'HomeLoan', 'Pending'),
(2, 150000, 9.2, 36, 'CarLoan', 'Pending'),
(3, 100000, 10.0, 12, 'HomeLoan', 'Pending'),
(4, 250000, 7.5, 48, 'HomeLoan', 'Pending'),
(5, 120000, 11.0, 18, 'CarLoan', 'Pending');

INSERT INTO HomeLoan (LoanId, PropertyAddress, PropertyValue) VALUES
(1, 'Happy stays ECR, Chennai', 300000),
(3, 'Queensland stays OMR, Chennai', 150000),
(4, 'Gandhi resorts, Pune', 400000);

INSERT INTO CarLoan (LoanId, CarModel, CarValue) VALUES
(2, 'Toyota Innova 2022', 180000),
(5, 'Ford Mustang 2021', 190000);

select * from Customer
select * from Loan
select * from HomeLoan
select * from CarLoan
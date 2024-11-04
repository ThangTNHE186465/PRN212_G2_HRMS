CREATE DATABASE FUHRM;
GO

USE FUHRM;
GO

CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(100) NOT NULL
);

CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    RoleID INT NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL,
    CreateDate DATETIME DEFAULT GETDATE(),
);

CREATE TABLE Positions (
    PositionID INT PRIMARY KEY IDENTITY(1,1),
    PositionName NVARCHAR(100) NOT NULL
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(50) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    DepartmentID INT NOT NULL,
    PositionID INT NOT NULL,
    AccountID INT NOT NULL,
    Salary FLOAT NOT NULL,
    StartDate DATE NOT NULL,
    ProfilePicture NVARCHAR(255),
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
    FOREIGN KEY (PositionID) REFERENCES Positions(PositionID),
    FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);

CREATE TABLE Salaries (
    SalaryID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    BaseSalary FLOAT NOT NULL,
    Allowance FLOAT,
    Bonus FLOAT,
    Penalty FLOAT,
    TotalIncome AS (BaseSalary + Allowance + Bonus - Penalty) PERSISTED,
    PaymentDate DATE NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

CREATE TABLE Attendances (
    AttendanceID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    Date DATE NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    OvertimeHours INT,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

CREATE TABLE Notifications (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    DepartmentID INT NOT NULL,
    CreatedDate DATETIME NOT NULL,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

CREATE TABLE LeaveRequests (
    LeaveRequestID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    LeaveType NVARCHAR(100) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);


-- Chèn dữ liệu vào bảng Roles
INSERT INTO Roles (RoleName) VALUES ('Admin');
INSERT INTO Roles (RoleName) VALUES ('Employee');

-- Chèn dữ liệu vào bảng Accounts
INSERT INTO Accounts (Username, Password, RoleID) VALUES ('admin', 'admin123', 1);
INSERT INTO Accounts (Username, Password, RoleID) VALUES ('employee1', 'employee1', 2);
INSERT INTO Accounts (Username, Password, RoleID) VALUES ('employee2', 'employee2', 2);
INSERT INTO Accounts (Username, Password, RoleID) VALUES ('employee3', 'employee3', 2);

-- Chèn dữ liệu vào bảng Departments
INSERT INTO Departments (DepartmentName, CreateDate) VALUES ('Human Resources', GETDATE());
INSERT INTO Departments (DepartmentName, CreateDate) VALUES ('Finance', GETDATE());
INSERT INTO Departments (DepartmentName, CreateDate) VALUES ('IT', GETDATE());

-- Chèn dữ liệu vào bảng Positions
INSERT INTO Positions (PositionName) VALUES ('HR Manager');
INSERT INTO Positions (PositionName) VALUES ('Finance Manager');
INSERT INTO Positions (PositionName) VALUES ('IT Specialist');

-- Chèn dữ liệu vào bảng Employees
INSERT INTO Employees (FullName, DateOfBirth, Gender, Address, PhoneNumber, DepartmentID, PositionID, AccountID, Salary, StartDate, ProfilePicture) 
VALUES ('John Doe', '1985-05-15', 'Male', '123 Main St', '123-456-7890', 1, 1, 2, 50000, '2020-01-01', 'john_doe.jpg');

INSERT INTO Employees (FullName, DateOfBirth, Gender, Address, PhoneNumber, DepartmentID, PositionID, AccountID, Salary, StartDate, ProfilePicture) 
VALUES ('Jane Smith', '1990-07-20', 'Female', '456 Elm St', '987-654-3210', 2, 2, 3, 60000, '2021-02-01', 'jane_smith.jpg');

INSERT INTO Employees (FullName, DateOfBirth, Gender, Address, PhoneNumber, DepartmentID, PositionID, AccountID, Salary, StartDate, ProfilePicture) 
VALUES ('Alice Johnson', '1992-03-10', 'Female', '789 Oak St', '555-666-7777', 3, 3, 4, 55000, '2022-03-01', 'alice_johnson.jpg');

-- Chèn dữ liệu vào bảng Salaries
INSERT INTO Salaries (EmployeeID, BaseSalary, Allowance, Bonus, Penalty, PaymentDate) 
VALUES (1, 50000, 5000, 2000, 500, '2023-01-31');

INSERT INTO Salaries (EmployeeID, BaseSalary, Allowance, Bonus, Penalty, PaymentDate) 
VALUES (2, 60000, 6000, 2500, 600, '2023-01-31');

INSERT INTO Salaries (EmployeeID, BaseSalary, Allowance, Bonus, Penalty, PaymentDate) 
VALUES (3, 55000, 5500, 2200, 550, '2023-01-31');

-- Chèn dữ liệu vào bảng Attendances
INSERT INTO Attendances (EmployeeID, Date, Status, OvertimeHours) 
VALUES (1, '2023-01-01', 'Present', 2);

INSERT INTO Attendances (EmployeeID, Date, Status, OvertimeHours) 
VALUES (2, '2023-01-01', 'Present', 1);

INSERT INTO Attendances (EmployeeID, Date, Status, OvertimeHours) 
VALUES (3, '2023-01-01', 'Absent', 0);

-- Chèn dữ liệu vào bảng Notifications
INSERT INTO Notifications (Title, Content, DepartmentID, CreatedDate) 
VALUES ('Meeting Reminder', 'Don''t forget about the meeting tomorrow at 10 AM.', 1, '2023-01-01 09:00:00');

INSERT INTO Notifications (Title, Content, DepartmentID, CreatedDate) 
VALUES ('Policy Update', 'Please review the updated company policies.', 2, '2023-01-02 10:00:00');

INSERT INTO Notifications (Title, Content, DepartmentID, CreatedDate) 
VALUES ('System Maintenance', 'The IT system will be down for maintenance this weekend.', 3, '2023-01-03 11:00:00');


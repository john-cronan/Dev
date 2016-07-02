CREATE DATABASE {DB_NAME};
GO
USE {DB_NAME};
CREATE TABLE People
(
	FirstName VARCHAR(50) NULL,
	LastName VARCHAR(50) NULL
);
GO
CREATE PROCEDURE CreatePerson
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50)
AS
	INSERT INTO People (FirstName, LastName) VALUES (@FirstName, @LastName);
GO
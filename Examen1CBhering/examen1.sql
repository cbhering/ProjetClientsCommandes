CREATE DATABASE examen1;
GO

USE examen1;
GO

CREATE TABLE Client(
	ClientId INT PRIMARY KEY,
	Nom VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Commandes(
	ComID INT PRIMARY KEY,
	Description VARCHAR(100) NOT NULL,
	Prix DECIMAL(10,2) NOT NULL,
	ClientId INT FOREIGN KEY REFERENCES Client(ClientId)
);
GO

INSERT INTO Client(ClientId, Nom)
VALUES	(1, 'Pierre'),
		(2, 'Marie'),
		(3, 'Anne'),
		(4, 'Jacques');
GO

INSERT INTO Commandes(ComID, Description, Prix, ClientId)
VALUES	(1, 'Clavier', 30.00, 3),
		(2, 'Souris', 15.00, 3),
		(3, 'Imprimante', 250.00, 1);
GO
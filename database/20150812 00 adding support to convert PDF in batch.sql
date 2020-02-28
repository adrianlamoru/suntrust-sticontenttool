CREATE TABLE ProcessFileTask
(
	ID int IDENTITY PRIMARY KEY NOT NULL,
	PIDs nvarchar(max) NOT NULL,
	CIDs nvarchar(max) NULL,
	[Status] int NOT NULL,
	EmailTo nvarchar(250) NOT NULL,
	ConvertedFilePath nvarchar(250) NULL,
	[Description] nvarchar(250) NULL,
	RequestedDate datetime NOT NULL,
	ConvertedDate datetime NULL,
	SentDate datetime NULL
)
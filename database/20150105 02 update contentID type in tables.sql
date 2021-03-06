use st1001
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Component
	DROP CONSTRAINT FK_Component_Project
GO
ALTER TABLE dbo.Project SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Project', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Project', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Project', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Component
	(
	ID int NOT NULL IDENTITY (1, 1),
	ProjectID int NOT NULL,
	ContentID nvarchar(14) NOT NULL,
	SectionID int NOT NULL,
	ComponentID int NOT NULL,
	Data ntext NOT NULL,
	Inactive bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Component SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Component ON
GO
IF EXISTS(SELECT * FROM dbo.Component)
	 EXEC('INSERT INTO dbo.Tmp_Component (ID, ProjectID, ContentID, SectionID, ComponentID, Data, Inactive)
		SELECT ID, ProjectID, CONVERT(nvarchar(14), ContentID), SectionID, ComponentID, Data, Inactive FROM dbo.Component WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Component OFF
GO
DROP TABLE dbo.Component
GO
EXECUTE sp_rename N'dbo.Tmp_Component', N'Component', 'OBJECT' 
GO
ALTER TABLE dbo.Component ADD CONSTRAINT
	PK_Component PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Component ADD CONSTRAINT
	FK_Component_Project FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Project
	(
	ID
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Component', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Component', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Component', 'Object', 'CONTROL') as Contr_Per 
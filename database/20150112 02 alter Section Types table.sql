use [st1001]
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
CREATE TABLE dbo.Tmp_SectionType
	(
	ID int NOT NULL,
	Name nvarchar(255) NOT NULL,
	Code nvarchar(50) NOT NULL,
	Type nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_SectionType SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.SectionType)
	 EXEC('INSERT INTO dbo.Tmp_SectionType (ID, Name, Code, Type)
		SELECT ID, Name, Code, Type FROM dbo.SectionType WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.SectionType
GO
EXECUTE sp_rename N'dbo.Tmp_SectionType', N'SectionType', 'OBJECT' 
GO
ALTER TABLE dbo.SectionType ADD CONSTRAINT
	PK_SectionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.SectionType', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.SectionType', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.SectionType', 'Object', 'CONTROL') as Contr_Per 
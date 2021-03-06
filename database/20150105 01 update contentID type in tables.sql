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
CREATE TABLE dbo.Tmp_LayoutType
	(
	ID nvarchar(14) NOT NULL,
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_LayoutType SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.LayoutType)
	 EXEC('INSERT INTO dbo.Tmp_LayoutType (ID, Name)
		SELECT CONVERT(nvarchar(14), ID), Name FROM dbo.LayoutType WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.LayoutType
GO
EXECUTE sp_rename N'dbo.Tmp_LayoutType', N'LayoutType', 'OBJECT' 
GO
ALTER TABLE dbo.LayoutType ADD CONSTRAINT
	PK_LayoutType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.LayoutType', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.LayoutType', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.LayoutType', 'Object', 'CONTROL') as Contr_Per 
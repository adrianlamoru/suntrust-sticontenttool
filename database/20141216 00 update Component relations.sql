use st1001;
GO


/*
   Tuesday, December 16, 20142:39:28 PM
   User: 
   Server: .\sqlexpress
   Database: st1001
   Application: 
*/

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
DROP TABLE dbo.MediaAsset
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Project SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Project', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Project', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Project', 'Object', 'CONTROL') as Contr_Per 

GO

DELETE FROM dbo.Component

GO


/*
   Thursday, March 17, 20165:43:40 PM
   User: 
   Server: .\SQLEXPRESS2014
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
ALTER TABLE dbo.Offer ADD
	Deleted bit NOT NULL CONSTRAINT DF_Offer_Deleted DEFAULT 0
GO
ALTER TABLE dbo.Offer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

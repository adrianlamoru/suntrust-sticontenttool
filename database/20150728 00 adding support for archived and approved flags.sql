USE [st1001]
GO

ALTER TABLE Project ADD ApprovedDate datetime null
GO

UPDATE Project SET Archived = Approved, ApprovedDate = ArchivedDate
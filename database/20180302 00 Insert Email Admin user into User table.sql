USE [suntrust-email-proofing]
GO

INSERT INTO [dbo].[User]
           ([Email]
           ,[FirstName]
           ,[LastName]
           ,[Password]
           ,[Role]
           ,[Deleted]
           ,[Created]
           ,[UserType])
     VALUES
           ('admin.email@suntrust.com','Suntrust','EmailAdmin','vDDjfzjs4g1P+MxrnCTQoebuMp0=', 0, 0 ,GETDATE() ,1)
GO



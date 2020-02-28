USE [st1002-qa]
GO

INSERT INTO [dbo].[SectionType]
           ([ID]
           ,[Name]
           ,[Code]
           ,[Type])
     VALUES
           (22,'Primary Banner','MOB_PRIMARY_ZONE','RTO'),
		   (23,'Offer Details','MOB_OFFR_DETAIL_PAGE','RTO')
GO
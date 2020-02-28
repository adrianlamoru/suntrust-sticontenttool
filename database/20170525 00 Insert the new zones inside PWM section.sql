USE [st1002-qa]
GO

INSERT INTO [dbo].[SectionType]
           ([ID]
           ,[Name]
           ,[Code]
           ,[Type])
     VALUES
           (15,'Bulletin','PWM_BULLETIN','PWM'),
		   (16,'PWM Details Page','PMW_LEARN_MORE','PWM'),
		   (17,'Primary Banner','PRIMARY_OFFR','PWM'), 
		   (18,'All Offers','VIEW_ALL','PWM'), 
		   (19,'Bulletin Zone','BULLETIN_ZONE','PWM'),
		   (20,'Recommended Accounts','GHOST_OFFR','PWM'),
		   (21,'Details Page','LEARN_MORE','PWM')
GO



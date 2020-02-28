USE st1001
GO


DELETE FROM dbo.Project;
DELETE FROM dbo.Offer;
DELETE FROM dbo.[User];

GO

SET IDENTITY_INSERT dbo.[User] ON;

INSERT INTO [st1001].[dbo].[User]
	([ID], [Email],[FirstName],[LastName],[Password],[Role])
VALUES  (1, 'a@a.com', 'Test1', 'LastName', 'CgS5cbA9pgfObEVRhAN7Zgyon3g=', 0)
           
INSERT INTO [st1001].[dbo].[User]
	([ID], [Email],[FirstName],[LastName],[Password],[Role])
VALUES  (2, 'deisbel@gmail.com', 'Deisbel', 'Diaz', 'CgS5cbA9pgfObEVRhAN7Zgyon3g=', 0)
  
SET IDENTITY_INSERT dbo.[User] OFF;  
         
GO

DELETE FROM dbo.LayoutType;

INSERT INTO dbo.LayoutType (ID, Name)
VALUES (1, 'Layout1')

INSERT INTO dbo.LayoutType (ID, Name)
VALUES (2, 'Layout2')

INSERT INTO dbo.LayoutType (ID, Name)
VALUES (3, 'Layout3')

GO

DELETE FROM dbo.SectionType;

INSERT INTO dbo.SectionType (Id, Name)
VALUES (1, 'Primary Banner')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (2, 'Details Page')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (3, 'Ghost Accounts')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (4, 'All Offers')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (5, 'Splash Page')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (6, 'Sign-off Page')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (7, 'Credit Cards')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (8, 'Deposits')

INSERT INTO dbo.SectionType (Id, Name)
VALUES (9, 'Equity')

GO 

DELETE FROM dbo.ComponentType;

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (1, 'Main Image', 'Main Image Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (2, 'Call to Action', 'Call to Action Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (3, 'Details', 'Details Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (4, 'Disclaimer', 'Disclaimer Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (5, 'Reminder', 'Reminder Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (6, 'Offer Rejection', 'Offer Rejection Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (7, 'Headline', 'Headline Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

INSERT INTO dbo.ComponentType (ID, Name, Headline, Description)
VALUES (8, 'Terms & Conditions', 'Terms & Conditions Headline', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard dummy text ever since the.')

GO

INSERT INTO dbo.Offer (ID, Name, [Description], 
		CreatedBy, UpdatedDate, ContentIDs, HasProject, CreateByExternal)
VALUES (1, 'Offer1', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard', 
		1, getdate(), '1, 2', 0, NULL);
	
INSERT INTO dbo.Offer (ID, Name, [Description], CreatedBy, UpdatedDate, ContentIDs, HasProject, CreateByExternal)
VALUES (2, 'Offer2', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard', 
		1, getdate(), '1, 3', 0, NULL);
		
INSERT INTO dbo.Offer (ID, Name, [Description], CreatedBy, UpdatedDate, ContentIDs, HasProject, CreateByExternal)
VALUES (3, 'Offer3', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard', 
		NULL, getdate(), '2, 3', 0, 'APrimo John');
		
INSERT INTO dbo.Offer (ID, Name, [Description], CreatedBy, UpdatedDate, ContentIDs, HasProject, CreateByExternal)
VALUES (4, 'Offer4', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard', 
		2, getdate(), '3, 1', 0, NULL)
	
INSERT INTO dbo.Offer (ID, Name, [Description], CreatedBy, UpdatedDate, ContentIDs, HasProject, CreateByExternal)
VALUES (5, 'Offer5', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard', 
		1, getdate(), '1, 2', 0, NULL)
		
INSERT INTO dbo.Offer (ID, Name, [Description], CreatedBy, UpdatedDate, ContentIDs, HasProject, CreateByExternal)
VALUES (6, 'Offer6', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry standard', 
		NULL, getdate(), '1, 3', 0, 'Delbert Diaz')	
		
GO

INSERT INTO dbo.Project (ID, CreatedBy, Archived, UpdatedDate, Locked, Approved, HasPrivateMediaAssets)
VALUES (1, 1, 0, getdate(), 0, 0, 0);

INSERT INTO dbo.Project (ID, CreatedBy, Archived, UpdatedDate, Locked, Approved, HasPrivateMediaAssets)
VALUES (2, 2, 1, dateadd(d, -10, getdate()), 0, 0, 1);

INSERT INTO dbo.Project (ID, CreatedBy, Archived, UpdatedDate, Locked, Approved, HasPrivateMediaAssets)
VALUES (3, 1, 0, dateadd(d, -8, getdate()), 0, 0, 0);

INSERT INTO dbo.Project (ID, CreatedBy, Archived, UpdatedDate, Locked, Approved, HasPrivateMediaAssets)
VALUES (4, 1, 0, dateadd(d, -20, getdate()), 0, 0, 0);

INSERT INTO dbo.Project (ID, CreatedBy, Archived, UpdatedDate, Locked, Approved, HasPrivateMediaAssets)
VALUES (5, 2, 0, dateadd(d, -1, getdate()), 0, 0, 1);

INSERT INTO dbo.Project (ID, CreatedBy, Archived, UpdatedDate, Locked, Approved, HasPrivateMediaAssets)
VALUES (6, 1, 0, dateadd(d, -2, getdate()), 1, 0, 0);

GO

		
			
		


USE [suntrust-email-proofing]
GO

UPDATE [dbo].[Component]
   SET [Data] = REPLACE(
    SUBSTRING([Data], 1, DATALENGTH([Data])),
    'https://onlinebanking.suntrust.com/UI/accounts?dl=balancetransfer', '')
GO
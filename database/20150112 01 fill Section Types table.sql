
use [st1001]
GO

delete from dbo.SectionType

insert into dbo.SectionType(ID, Name, Code, Type)
values (1, 'Primary Banner', 'PRIMARY_BANNER', 'VO')

insert into dbo.SectionType(ID, Name, Code, Type)
values (2, 'Details Page', 'DETAIL_PAGE', 'VO')

insert into dbo.SectionType(ID, Name, Code, Type)
values (3, 'Recommended Accounts', 'RECOMM_ACC', 'VO')

insert into dbo.SectionType(ID, Name, Code, Type)
values (4, 'All Offers', 'ALL_OFFERS', 'VO')

insert into dbo.SectionType(ID, Name, Code, Type)
values (5, 'Splash Page', 'SPLASH_PAGE', 'VO')

insert into dbo.SectionType(ID, Name, Code, Type)
values (6, 'Sign-Off Page', 'LOGOFF_PAGE', 'VO')

insert into dbo.SectionType(ID, Name, Code, Type)
values (7, 'Credit Cards', 'CC_PROD_PAGE', 'NEW')

insert into dbo.SectionType(ID, Name, Code, Type)
values (8, 'Deposits', 'DEPOSIT', 'NEW')

insert into dbo.SectionType(ID, Name, Code, Type)
values (9, 'Equity', 'EQUITY', 'NEW')


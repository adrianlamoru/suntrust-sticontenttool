
use [st1001]
GO 

update dbo.SectionType 
set Code = 'NAC_DEFAULT'
where ID in (7,8,9)


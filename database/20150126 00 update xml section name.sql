use st1001
go


update dbo.SectionType
set Code = 'PRIMARY_OFFR'
where ID = 1

update dbo.SectionType
set Code = 'GHOST_OFFR'
where ID = 3

update dbo.SectionType
set Code = 'VIEW_ALL'
where ID = 4

update dbo.SectionType
set Code = 'LEARN_MORE'
where ID = 2

update dbo.SectionType
set Code = 'EQ_PROD_PAGE'
where ID = 9

update dbo.SectionType
set Code = 'DEPOSITS_PAGE'
where ID = 8








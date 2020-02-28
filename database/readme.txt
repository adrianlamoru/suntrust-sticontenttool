Every change should be reflected in scripts
--------------------------------------------

The name of the script must follow this format: 

"yyyymmdd nn optional-description.sql"

yyyy - year
mm - two digit for month
dd - two digit for day
nn - consecutive in the same day
optional-description - some comments to recognize better the file

Example: 

"20141117 00 create table user.sql"

Every team member should verify this folder in every update to look for new updates.

Last runned script in Somnio servers:
-----------------------------------------

DEV: 20180531 00 Update Component goToOLB-BalanceTransfer data url to empty.sql
QA: 20180531 00 Update Component goToOLB-BalanceTransfer data url to empty.sql
UAT: 20160811 00 Update BulletinZone in SectionType
BETA: 20160503 01 Update GUID Field in Project.sql
TRAINING: 20160503 01 Update GUID Field in Project.sql
LIVE: 20161017 00 Add new Call to Action.sql


#Installation steps.

##SQL SERVER

1- Create an empty DB (any name, "st-db" for example). 
2- Run a script to create tables and fill out the data (It can be gotten from an existing bd).
3- Create an sql-server user and give it "owner" permit over the BD. 

##IIS


1- Create a new WebSite or Virtual App/Directory using its domain name header and local folder path.
2- Update the ConnectionString property inside the web.config file. 
3- Update the property RelativeAppPath inside web.config. If you have created the App as a WebSite then this value must be empty.
4- Give Write permit (Modify) to the MediaLibrary folder for the IIS_IUSRS user. 
5- Put in TRUE the property "Enable 32-bits Applications" for the Application Pool used by our App. (You can find this one inside the Advance Settings tab)
6- Download and install http://downloads.sourceforge.net/project/wkhtmltopdf/0.12.2.1/wkhtmltox-0.12.2.1_msvc2013-win32.exe on the server.
7- Enable the possibility of Write to the Windows Event Log through this our App: 

To allow that you need to do some set up in the system registry. 

- Locate HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog in regedit
- Add permissions Read and Full Controls for "NETWORK SERVICE" user
- Locate HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application
- Add a folderKey with name st1001 
 You can read the steps here: 

http://www.christiano.ch/wordpress/2009/12/02/iis7-web-application-writing-to-event-log-generates-security-exception/




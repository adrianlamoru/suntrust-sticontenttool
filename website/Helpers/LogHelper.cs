using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Configuration;

namespace st1001.website.Helpers
{
    public class LogHelper
    {
        static EventLog appLog;
        static string environment = ConfigurationManager.AppSettings["RelativeAppPath"].Replace("/somnio/somnio1001/", string.Empty);

        public static void Log(string msg){
            if (appLog == null){
                appLog = new EventLog();
                appLog.Source = "st1001";
            }

            try
            {
                appLog.WriteEntry(environment + " - " + msg, EventLogEntryType.Error);
            }
            catch (Exception ex) {
                //http://www.christiano.ch/wordpress/2009/12/02/iis7-web-application-writing-to-event-log-generates-security-exception/

                //Locate HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog in regedit
                //Add permisions Read and Full Controls for "NETWORK SERVICE" user
                //Locate HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application
                //Add a folderKey with name st1001

                throw new Exception("Solution: "  + 
                    @" - Locate HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog in regedit" +
                    @" - Add permisions Read and Full Controls for [NETWORK SERVICE] user" + 
                    @" - Locate HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application" + 
                    @" - Add a folderKey with name st1001" + 
                    "Error detail: " + ex.Message);
            }
            
        }
    }
}
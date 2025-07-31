using BpDmTerminal.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BpDmTerminal.Models
{
    public class LogHelper
    {
        public static void AddSearchRequestInfo(TerminalEntities db, string searchValue, string ip)
        {
            db.SearchRequestInfo.Add(new SearchRequestInfo
            {
                CreatedDateTime = DateTime.Now,
                IpAddress = ip,
                SearchValue = searchValue
            });

            db.SaveChanges();
        }

        public static void AddPassCardVisitorInfoLog(TerminalEntities db, string passCardVisitorId, string rfidNumber, string notice)
        {
            db.PassCardVisitorInfoLog.Add(new PassCardVisitorInfoLog
            {
                CreatedDateTime = DateTime.Now,
                Notice = notice,
                PassCardVisitorId = passCardVisitorId,
                RfidNumber = rfidNumber
            });

            db.SaveChanges();
        }

        public static void AddError(string description, string idAddress, string notice = null)
        {
            using (var db = new TerminalEntities())
            {
                try
                {
                    db.TerminalError.Add(new TerminalError
                    {
                        CreatedDate = DateTime.Now,
                        Description = description,
                        IdAddress = idAddress,
                        Notice = notice
                    });

                    db.SaveChanges();
                }
                catch { }
            }
        }
    }
}
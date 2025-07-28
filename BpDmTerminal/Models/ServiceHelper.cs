using BpDmTerminal.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BpDmTerminal.Models
{
    public class ServiceHelper
    {
        public static ResponseCard GetVisitor(string searchValue, string terminalName)
        {
            RequestCard requestCard = new RequestCard
            {
                RequestInfo = new RequestInfo
                {
                    MessageID = Guid.NewGuid().ToString(),
                    Sender = new Sender
                    {
                        User = terminalName,
                        Password = "Ter2minaL",
                    },
                },
                RequestData = new RequestCardData
                {
                    DocumentNumber = searchValue
                },
            };

            ServiceReference1.TerminalService1SoapClient client = new ServiceReference1.TerminalService1SoapClient();

            return client.GetVisitor(requestCard);
        }

        public static Response SetVisitorsRFID(string passCardVisitorId, string rfidNumber, string terminalName)
        {
            RequestRFID requestRFID = new RequestRFID
            {
                RequestInfo = new RequestInfo
                {
                    MessageID = Guid.NewGuid().ToString(),
                    Sender = new Sender
                    {
                        User = terminalName,
                        Password = "Ter2minaL",
                    },
                },
                RequestData = new RequestRFIDData
                {
                    CardID = passCardVisitorId,
                    RFID_Number = rfidNumber
                },
            };

            ServiceReference1.TerminalService1SoapClient client = new ServiceReference1.TerminalService1SoapClient();
            return client.SetVisitorsRFID(requestRFID);
        }

        public static Response CardsEnded(string terminalName)
        {
            Request request = new Request
            {
                RequestInfo = new RequestInfo
                {
                    MessageID = Guid.NewGuid().ToString(),
                    Sender = new Sender
                    {
                        User = terminalName, 
                        Password = "Ter2minaL"
                    },
                }
            };

            ServiceReference1.TerminalService1SoapClient client = new ServiceReference1.TerminalService1SoapClient();
            var sdfsdsdf = client.CardsEnded(request);
            return sdfsdsdf;
        }
    }
}
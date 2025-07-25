using BpDmTerminal.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BpDmTerminal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult SetLanguage(string lang)
        {
            HttpCookie langCookie = new HttpCookie("lang", lang);
            langCookie.Expires = DateTime.Now.AddYears(2);
            Response.Cookies.Add(langCookie);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult SearchPassCardPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchValue)
        {
            try
            {
                RequestCard requestCard = new RequestCard
                {
                    RequestInfo = new RequestInfo
                    {
                        MessageID = Guid.NewGuid().ToString(),
                        Sender = new Sender
                        {
                            User = "Terminal4k1t",
                            Password = "Ter2minaL",
                        },
                    },
                    RequestData = new RequestCardData
                    {
                        DocumentNumber = searchValue
                    },
                };

                ServiceReference1.TerminalService1SoapClient client = new ServiceReference1.TerminalService1SoapClient();

                //var resp = client.GetVisitor(requestCard);

                //if (resp.Status == false)
                //    return RedirectToAction("PassCardNotFoundPage");

                var resp = new ResponseCard();
                resp.CabinetFloor = "1";
                resp.InvitersFullname = "Ержан Нурсултан";
                resp.InvitersPhoneNumber = "74-56-98";
                resp.CabinetNumber = "103";
                resp.VisitorFullname = "Ләйлім";

                return View(resp);

            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage");
            }

        }

        public ActionResult PassCardNotFoundPage()
        {
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}
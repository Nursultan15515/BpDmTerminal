using BpDmTerminal.Models;
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
                var resp = ServiceHelper.GetVisitor(searchValue, "Terminal4k1t");

                if (resp.Status == false)
                    return RedirectToAction("PassCardNotFoundPage");

                //var resp = new ResponseCard();
                //resp.CabinetFloor = "1";
                //resp.InvitersFullname = "Ержан Нурсултан";
                //resp.InvitersPhoneNumber = "74-56-98";
                //resp.CabinetNumber = "103";
                //resp.VisitorFullname = "Ләйлім";
                //resp.NeedPhoto = true;
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

        [HttpPost]
        public ActionResult ConfirmYes(ResponseCard model)
        {
            if (model.NeedPhoto)
                return RedirectToAction("TakePhoto", model);
            else
                return RedirectToAction("GiveCard", model);
        }

        public ActionResult TakePhoto(ResponseCard model)
        {
            return View(model);
        }

        public ActionResult GiveCard(ResponseCard model)
        {
            try
            {
                var cardResponse = CardRequester.IssueCard();

                if (!cardResponse.success)
                    return RedirectToAction("ErrorPage");

                if (string.IsNullOrEmpty(cardResponse.uid))
                    return RedirectToAction("ErrorPage");

                //if (cardResponse.isCardEnd)
                //    ServiceHelper.CardsEnded("Terminal4k1t");

                //ServiceHelper.SetVisitorsRFID(model.CardID, cardResponse.uid, "Terminal4k1t");

                return RedirectToAction("OfferTakeСard");
            }
            catch (Exception ex) 
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult OfferTakeСard()
        {
            return View();
        }

    }
}
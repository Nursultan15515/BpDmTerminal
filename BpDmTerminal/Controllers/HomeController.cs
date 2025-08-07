using BpDmTerminal.Database;
using BpDmTerminal.Models;
using BpDmTerminal.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading;
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

        public ActionResult test()
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
                using (var db = new TerminalEntities())
                {
                    LogHelper.AddSearchRequestInfo(db, searchValue, Request.UserHostAddress);

                    var terminalName = db.TerminalInfo.Where(r => r.IpAddress == Request.UserHostAddress).Select(r => r.TerminalName).FirstOrDefault();

                    if (string.IsNullOrEmpty(terminalName))
                    {
                        LogHelper.AddError("terminalName is null or empty", Request.UserHostAddress, $"Search; searchValue=${searchValue}");
                        return RedirectToAction("ErrorPage");
                    }

                    var response = ServiceHelper.GetVisitor(searchValue, terminalName); //"Terminal4k1t"

                    if (response == null)
                    {
                        LogHelper.AddError("response is null", Request.UserHostAddress, $"Search; searchValue=${searchValue}");
                        return RedirectToAction("ErrorPage");
                    }

                    if (response.Status == false)
                        return RedirectToAction("PassCardNotFoundPage");


                    //var response = new ResponseCard();
                    //response.CabinetFloor = "1";
                    //response.InvitersFullname = "Ержан Нурсултан";
                    //response.InvitersPhoneNumber = "74-56-98";
                    //response.CabinetNumber = "103";
                    //response.VisitorFullname = "Ләйлім";
                    //response.NeedPhoto = true;
                    //response.CardID = "01010109";
                    return View(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.AddError(ex.ToString(), Request.UserHostAddress, $"searchValue = {searchValue}");
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult PassCardNotFoundPage()
        {
            ViewBag.CurrentLang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return View();
        }

        public ActionResult ErrorPage(string errorMessage = null)
        {
            if (!string.IsNullOrEmpty(errorMessage))
                LogHelper.AddError(errorMessage, Request.UserHostAddress, "");

            ViewBag.CurrentLang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmYes(ResponseCard model)
        {
            if (model.NeedPhoto)
                return RedirectToAction("TakePhoto", "Home", new { passCardVisitorId = model.CardID });
            else
                return RedirectToAction("GiveCard", "Home", new { passCardVisitorId = model.CardID });
        }

        public ActionResult TakePhoto(string passCardVisitorId)
        {
            ViewBag.PassCardVisitorId = passCardVisitorId;
            return View();
        }


        public ActionResult SavePhoto(string passCardVisitorId, string base64photo)
        {
            try
            {
                using (var db = new TerminalEntities())
                {
                    LogHelper.AddPassCardVisitorInfoLog(db, passCardVisitorId, "", "savePhoto");

                    var terminalName = db.TerminalInfo.Where(r => r.IpAddress == Request.UserHostAddress).Select(r => r.TerminalName).FirstOrDefault();

                    if (string.IsNullOrEmpty(terminalName))
                    {
                        LogHelper.AddError("terminalName is null or empty", Request.UserHostAddress, $"SavePhoto; passCardVisitorId=${passCardVisitorId}");
                        return RedirectToAction("ErrorPage");
                    }

                    var response = ServiceHelper.SetVisitorsPhoto(passCardVisitorId, base64photo, terminalName);

                    if (response == null)
                    {
                        LogHelper.AddError("response is null", Request.UserHostAddress, $"SavePhoto; passCardVisitorId={passCardVisitorId}; base64photoLength={base64photo.Length}");
                        return RedirectToAction("ErrorPage");
                    }

                    if (!response.Status)
                    {
                        LogHelper.AddError("response status false", Request.UserHostAddress, $"SavePhoto; passCardVisitorId={passCardVisitorId}; base64photoLength={base64photo.Length}");
                        return RedirectToAction("ErrorPage");
                    }

                    return RedirectToAction("GiveCard", "Home", new { passCardVisitorId });
                }
            }
            catch (Exception ex)
            {
                LogHelper.AddError(ex.ToString(), Request.UserHostAddress, $"SavePhoto; passCardVisitorId={passCardVisitorId}; base64photoLength={base64photo.Length}");
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult GiveCard(string passCardVisitorId)
        {
            ViewBag.passCardVisitorId = passCardVisitorId;
            return View();
        }

        public ActionResult SetVisitorsRFID(string passCardVisitorId, string rfidNumber)
        {
            try
            {
                using (var db = new TerminalEntities())
                {
                    LogHelper.AddPassCardVisitorInfoLog(db, passCardVisitorId, rfidNumber, "SetVisitorsRFID");

                    var terminalName = db.TerminalInfo.Where(r => r.IpAddress == Request.UserHostAddress).Select(r => r.TerminalName).FirstOrDefault();

                    if (string.IsNullOrEmpty(terminalName))
                    {
                        LogHelper.AddError("terminalName is null or empty", Request.UserHostAddress, $"SetVisitorsRFID; passCardVisitorId=${passCardVisitorId}");
                        return RedirectToAction("ErrorPage");
                    }

                    var response = ServiceHelper.SetVisitorsRFID(passCardVisitorId, rfidNumber, terminalName);

                    if (response == null)
                    {
                        LogHelper.AddError("response is null", Request.UserHostAddress, $"SetVisitorsRFID; passCardVisitorId={passCardVisitorId}; rfidNumber={rfidNumber}");
                        return RedirectToAction("ErrorPage");
                    }

                    if (!response.Status)
                    {
                        LogHelper.AddError("response status false", Request.UserHostAddress, $"SetVisitorsRFID; passCardVisitorId={passCardVisitorId}; rfidNumber={rfidNumber}");
                        return RedirectToAction("ErrorPage");
                    }

                    return RedirectToAction("OfferTakeСard");
                }
            }
            catch (Exception ex)
            {
                LogHelper.AddError(ex.ToString(), Request.UserHostAddress, $"SetVisitorsRFID; passCardVisitorId={passCardVisitorId}; rfidNumber={rfidNumber}");
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult OfferTakeСard()
        {
            var currentLanguage = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            ViewBag.CurrentLang = currentLanguage;

            ViewBag.TakeCardInfo = "";
            switch (currentLanguage)
            {
                case "kk":
                    ViewBag.TakeCardInfo = "15 секунд ішінде картаны алуыңызды өтінеміз";
                    break;
                case "ru":
                    ViewBag.TakeCardInfo = "Пожалуйста, заберите карту в течение 15 секунд";
                    break;
                default:
                    ViewBag.TakeCardInfo = "Please take your card within 15 seconds";
                    break;
            }

            return View();
        }

        public ActionResult SendCardEndInformation()
        {
            using (var db = new TerminalEntities())
            {
                try
                {
                    var terminalName = db.TerminalInfo.Where(r => r.IpAddress == Request.UserHostAddress).Select(r => r.TerminalName).FirstOrDefault();

                    if (string.IsNullOrEmpty(terminalName))
                    {
                        LogHelper.AddError("terminalName is null or empty", Request.UserHostAddress, $"SendCardEndInformation");
                        return RedirectToAction("ErrorPage");
                    }

                    ServiceHelper.CardsEnded(terminalName);

                    return Json("Ok", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    LogHelper.AddError(ex.ToString(), Request.UserHostAddress, "SendCardEndInformation");
                    return RedirectToAction("ErrorPage");
                }
            }
        }
    }
}
using BpDmTerminal.Database;
using BpDmTerminal.Models;
using BpDmTerminal.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                using (var db = new TerminalEntities())
                {
                    LogHelper.AddSearchRequestInfo(db, searchValue, Request.UserHostAddress);

                    var terminalName = db.TerminalInfo.Where(r => r.IpAddress == Request.UserHostAddress).Select(r => r.TerminalName).FirstOrDefault();

                    if (string.IsNullOrEmpty(terminalName))
                        terminalName = "Terminal4k1t";

                    var response = ServiceHelper.GetVisitor(searchValue, "Terminal4k1t");

                    if (response == null)
                    {
                        LogHelper.AddError("response is null", Request.UserHostAddress, $"Search; searchValue=${searchValue}");
                        return RedirectToAction("ErrorPage");
                    }

                    if (response.Status == false)
                        return RedirectToAction("PassCardNotFoundPage");


                    //var resp = new ResponseCard();
                    //resp.CabinetFloor = "1";
                    //resp.InvitersFullname = "Ержан Нурсултан";
                    //resp.InvitersPhoneNumber = "74-56-98";
                    //resp.CabinetNumber = "103";
                    //resp.VisitorFullname = "Ләйлім";
                    //resp.NeedPhoto = false;
                    //resp.CardID = "01010109";
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
                        terminalName = "Terminal4k1t";

                    var response = ServiceHelper.SetVisitorsPhoto(passCardVisitorId, base64photo, "Terminal4k1t");

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

                    return RedirectToAction("GiveCard", passCardVisitorId);
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
                    var response = ServiceHelper.SetVisitorsRFID(passCardVisitorId, rfidNumber, "Terminal4k1t");

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
            return View();
        }

    }
}
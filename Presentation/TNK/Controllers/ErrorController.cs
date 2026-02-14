using System.Collections.Generic;
using System.Web.Mvc;
using TNK.Core.Domain;
using TNK.Model;
using TNK.Services.Catalog;
using TNK.Services.SoThu;
using TNK.Services.Users;
using System.Linq;
using System;
using TNK.Services.SoChi;
using TNK.Services.Log;

namespace TNK.Controllers
{
    public class ErrorController : Controller
    {
        IUserervice _userService;
        ILogger _logger;
        
        public ErrorController(IUserervice _userService,
            ILogger _logger
            ) : base()
        {
            this._userService = _userService;
            this._logger = _logger;
        }       
        

        #region 
        public ActionResult Index()
        {            
            if (Session["Error"] != null && Session["Error"].ToString() != "")
            {
                ViewBag.ErrorMessage = Session["Error"].ToString();
                Session["Error"] = "";
                _logger.WriteLog("Error:" + ViewBag.ErrorMessage);
            }
            else
                ViewBag.ErrorMessage = "";
            if (Session["LastException"] != null)
            {
                _logger.WriteLog("Error:" , Session["LastException"]);
                Session["LastException"] = null;
            }
            return View("Index");
        }       
        #endregion

    }
}
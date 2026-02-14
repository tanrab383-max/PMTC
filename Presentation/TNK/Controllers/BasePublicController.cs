using TNK.Core;
using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Web;
using System.Web.Security;
using TNK.Services.Users;
using TNK.Services.Log;
using TNK.Core.Domain;
using System.Collections.Generic;
using System.Data;
using TNK.Data;
using TNK.Model.Authentication;
using System.Configuration;
using System.Collections;

namespace TNK.Controllers
{
    [Authorize]
    public abstract partial class BasePublicController : BaseController
    {
        protected string DateFormat = "dd/MM/yyyy";
        IUserervice _userService;
        IUserRegistrationService _userRegistrationService;
        public ILogger _logger;
        IDbContext _dbContext;
        protected AF.Library.Logger log = new AF.Library.Logger("BasePublicController");
        protected AuthenticationModel _auModel = new AuthenticationModel();

        protected List<string> lstReadOnlyUser = new List<string>() { "dung.bui", "phuc.ngo"};

        protected string ActionKey = "";

        public string ShowRoom = "";

        private static readonly Dictionary<string, string> ShowRoomNames = new Dictionary<string, string>
        {
            { "HLAN", "HONDA LONG AN" },
            { "HPTN", "HONDA QUẬN 2" },
            { "HTNH", "HONDA TÂY NINH" },
            { "HBHA", "HONDA BIÊN HÒA" },

            { "TNK", "TOYOTA NINH KIỀU" },
            { "TTG", "TOYOTA TIỀN GIANG" },
            { "TBTR", "TOYOTA BẾN TRE" },
            { "MVL", "MITSUBISHI VĨNH LONG" },
        };



        protected DateTime startTime = DateTime.Now;
        protected DateTime endTime = DateTime.Now;

        protected Hashtable hshStartTime = new Hashtable();
        protected Hashtable hshEndTime = new Hashtable();

        protected void SetStartTime(string action)
        {
            startTime = DateTime.Now;
            hshStartTime[action] = startTime;
        }
        protected void SetEndTime(string action)
        {
            endTime = DateTime.Now;
            hshEndTime[action] = endTime;
        }

        protected void WriteLog(string controller, string action, string error, string url, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (_logger != null )
                {
                    _logger.WriteLogAccessTime(controller, action, startDate, endDate, error, url);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void WriteLog(string controller, string action, string error,string url)
        {
            try
            {
                if (_logger != null && hshEndTime[action] != null && hshStartTime[action] != null)
                {
                    _logger.WriteLogAccessTime(controller, action, (DateTime)hshStartTime[action], (DateTime)hshEndTime[action], error,url);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public BasePublicController(
          ILogger _logger
         ) : this()
        {
            log.Start("BasePublicController");
            this._logger = _logger;
            log.End("BasePublicController");

        }

        public BasePublicController(IUserervice _userService,
            IUserRegistrationService _userRegistrationService,
            IDbContext _dbContext,
            ILogger _logger
           ) : base()
        {
            log.Start("BasePublicController");
            this._userService = _userService;
            this._userRegistrationService = _userRegistrationService;
            this._logger = _logger;
            this._dbContext = _dbContext;
            log.End("BasePublicController");

        }

        protected override RedirectToRouteResult RedirectToActionPermanent(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return base.RedirectToActionPermanent(actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Them action log
        /// </summary>
        /// <param name="log"></param>
        protected void AddActionLog(string user, string controller, string action, DateTime start, DateTime end, object error)
        {
            string url = Request.RawUrl;
            DataTable tbl = new DataTable("ActionLog");
            if (Session["ActionLog"] == null)
            {
                tbl.Columns.Add(new DataColumn("user", typeof(string)));
                tbl.Columns.Add(new DataColumn("url", typeof(string)));
                tbl.Columns.Add(new DataColumn("controller", typeof(string)));
                tbl.Columns.Add(new DataColumn("action", typeof(string)));
                tbl.Columns.Add(new DataColumn("start", typeof(DateTime)));
                tbl.Columns.Add(new DataColumn("end", typeof(DateTime)));
                tbl.Columns.Add(new DataColumn("error", typeof(string)));
                Session["ActionLog"] = tbl;

            }
            tbl = (DataTable)(Session["ActionLog"]);
            tbl.Rows.Add(new object[7] { user, url,controller, action, start, end, error.ToString() });
        }

        protected void SummitActionLogToDatabase()
        {
            try
            {
                if (Session["LastSummitLog"] == null)
                {
                    Session["LastSummitLog"] = DateTime.Now;
                }
                DateTime lastSummitLog = (DateTime)Session["LastSummitLog"];
                if (DateTime.Now.Subtract(lastSummitLog).Minutes > 1)
                {
                    String sql = "";
                    DataTable tbl = new DataTable("ActionLog");
                    tbl = (DataTable)(Session["ActionLog"]);
                    if (tbl != null)
                    {
                        foreach (DataRow r in tbl.Rows)
                        {
                            string user = r["user"].ToString();
                            string url = r["url"].ToString();
                            string controller = r["controller"].ToString();
                            url = url.Replace("'", "");
                            url = url.Replace("-", "");
                            url = url.Replace("/*", "");
                            url = url.Replace("*/", "");
                            string action = r["action"].ToString();
                            DateTime start = (DateTime)r["start"];
                            DateTime end = (DateTime)r["end"];
                            string error = r["error"].ToString();
                            error = error.Replace("'", "");
                            error = error.Replace("-", "");
                            error = error.Replace("/*", "");
                            error = error.Replace("*/", "");
                            double duration = end.Subtract(start).Seconds;
                           
                            sql = sql + @" insert into ActionLog(Id,UserName,Url,Controller,Action,StartTime,EndTime,Duration,Error,ActionTime) values(newid(),'" + user + @"',N'" + url + @"',N'" + controller + @"','" + action + @"','" + start.ToString("yyyy-MM-dd HH:mm:ss") + "','" + end.ToString("yyyy-MM-dd HH:mm:ss") + "'," + duration + @",N'" + error + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                        }
                        //goi insert xuong database
                        //_pts.insert(sql);
                      //  if (!string.IsNullOrEmpty(sql))
                        //    _dbContext.ExecuteSqlCommand(sql);
                        //insertDB(sql);
                        Session["LastSummitLog"] = DateTime.Now;
                        ((DataTable)(Session["ActionLog"])).Rows.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                AF.Library.Logger.CreateInstant("SummitActionLogToDatabase").Error(ex);
                _logger.WriteLog(this.Request.RawUrl, ex);
            }
        }


        protected string GetIPClient()
        {
            if (Request != null)
                return Request.UserHostAddress;
            return "";
        }

        protected bool CheckRole(string actionKey)
        {
            
            // lay action tu url ra
            //string actionKey = base.ControllerContext.RouteData.Values["action"].ToString();
            //kiem tra trong database co action nay duoc cap quyen khong
            //neu co thi di tiep

            //neu khong thi cho ra trang bao loi

            return false;
        }

        protected string GetHostNameClient()
        {
            //if (Request == null)
            //    return "";
            //string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
            //if(computer_name.Length>0)
            //    return computer_name[0];           
            return "";
        }
        protected virtual void InvokeAction()
        {

            log.Start("InvokeAction");

            log.Debug(this.ControllerContext.RouteData.Values["action"].ToString());
            SetStartTime(this.ControllerContext.RouteData.Values["action"].ToString());

            try
            {                
                //_auModel.Action = this.ControllerContext.RouteData.Values["action"].ToString();
                //_auModel.Controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                //log.Param("Action", _auModel.Action);
                //log.Param("Controller", _auModel.Controller);
                //.Area = this.ControllerContext.RouteData.Values["area"].ToString();
                //if (_userRegistrationService != null)
                //{
                //    var user = _userRegistrationService.getCurrentUser();

                //    // if (Request.HttpMethod == "POST" && !_userRegistrationService.CheckRole(_auModel))
                //    if (Request.HttpMethod == "POST" && lstReadOnlyUser.Contains(user.UserName))
                //    {
                //        Session["Error"] = "Bạn không có quyền truy cập vào chức năng này.";
                //        _logger.WriteLog(this.Request.RawUrl, "User " + user + " không có quyền truy cập");
                //        RedirectToActionPermanent("Index", "Error");
                //    }
                //}
                ////return Redirect("~/Home/NoAccess");
                
                //if (Session["Error"] != null && Session["Error"].ToString() != "")
                //{
                //    ViewBag.ErrorMessage = Session["Error"].ToString();
                //    Session["Error"] = "";
                //    _logger.WriteLog("Error:" , ViewBag.ErrorMessage);
                //}
            }
            catch (Exception ex)
            {
                AF.Library.Logger.CreateInstant("BasePublicController").Error(ex);
                _logger.WriteLog(this.Request.RawUrl , ex);
            }            
            log.End("InvokeAction");
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            log.Start("EndExecute");
            try
            {               
                InvokeAction();                

                base.EndExecute(asyncResult);
                this.SummitActionLogToDatabase();
                _auModel.Action = this.ControllerContext.RouteData.Values["action"].ToString();
                _auModel.Controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                log.Param("Action", _auModel.Action);
                log.Param("Controller", _auModel.Controller);
               
                SetEndTime(_auModel.Action);
                string url = HttpUtility.UrlDecode( this.ControllerContext.HttpContext.Request.Url.ToString());
                //if (this.ControllerContext.HttpContext.Request["from"] != null)
                //    url += "@from=" + this.ControllerContext.HttpContext.Request["from"];
                //if (this.ControllerContext.HttpContext.Request["to"] != null)
                //    url += "@to=" + this.ControllerContext.HttpContext.Request["to"];
                WriteLog(_auModel.Controller, _auModel.Action,"", url);
                SummitActionLogToDatabase();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (ex.StackTrace != null)
                    log.Error(ex.StackTrace);
                if(ex.InnerException!= null)
                    log.Error(ex.InnerException);
                if(_logger != null)
                    _logger.WriteLog(this.Request.RawUrl + ":" , ex);
                WriteLog(_auModel.Controller, _auModel.Action, ex.ToString(),"");
                throw (ex);
            }
            log.End("EndExecute");
        }

        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            return new EmptyResult();
        }
        public BasePublicController()
        {
            ViewBag.DateFormat = DateFormat;
            ViewBag.ShowRoom = ConfigurationSettings.AppSettings["ShowRoom"];
            ShowRoom = ConfigurationSettings.AppSettings["ShowRoom"];
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Lấy giá trị của cookie trong phương thức này
            var storeIdCookie = filterContext.HttpContext.Request.Cookies["StoreId"]?.Value;
            if (!string.IsNullOrEmpty(storeIdCookie))
            {
                ShowRoomNames.TryGetValue(storeIdCookie, out ShowRoom);
                ViewBag.ShowRoom = ShowRoom;
            }
        }
        public int total = 0;
        public int pageSize = 30;
        public HttpContextBase _httpContext;
        public void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string type,int pageSize=30)
        {

            var cookie = Request.Cookies.Get(type);
            try
            {
                if (from == "" && to == "" && query == "")
                {
                    if (cookie == null)
                    {
                        from = DateTime.Now.ToString(DateFormat);
                        to = DateTime.Now.ToString(DateFormat);
                        cookie = new HttpCookie(type);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                        cookie.Values["from"] = from;
                        cookie.Values["to"] = to;
                        cookie.Values["query"] = query;
                        _httpContext.Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        from = cookie.Values["from"];
                        to = cookie.Values["to"];
                        query = cookie.Values["query"];
                    }
                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                }
                else if (from != "" && to != "")
                {
                    if (cookie == null)
                    {
                        f = Convert.ToDateTime(from + " 00:00:00");
                        t = Convert.ToDateTime(to + " 23:59:59");
                        cookie = new HttpCookie(type);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                    }
                    cookie.Values["from"] = from;
                    cookie.Values["to"] = to;
                    cookie.Values["query"] = query;

                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    _httpContext.Response.Cookies.Set(cookie);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SetSearch", ex);
            }
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Warning = TempData["Warning"];
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Info = TempData["Info"];
            ViewBag.Error = TempData["Error"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&p=";
        }

        public void SetSearch(ref string from, ref string to, ref DateTime f, ref DateTime t,int p, ref string query, string type,string tinhtrang,int pageSize=30)
        {

            var cookie = Request.Cookies.Get(type);
            try
            {
                if (from == "" && to == "" && query == "" && tinhtrang == "")
                {
                    if (cookie == null)
                    {
                        from = DateTime.Now.ToString(DateFormat);
                        to = DateTime.Now.ToString(DateFormat);
                        cookie = new HttpCookie(type);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                        cookie.Values["from"] = from;
                        cookie.Values["to"] = to;
                        cookie.Values["query"] = query;
                        cookie.Values["tinhtrang"] = tinhtrang;
                        _httpContext.Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        from = cookie.Values["from"];
                        to = cookie.Values["to"];
                        query = cookie.Values["query"];
                        tinhtrang = cookie.Values["tinhtrang"];
                    }
                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                }
                else if (from != "" && to != "")
                {
                    if (cookie == null)
                    {
                        f = Convert.ToDateTime(from + " 00:00:00");
                        t = Convert.ToDateTime(to + " 23:59:59");
                        cookie = new HttpCookie(type);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                    }
                    cookie.Values["from"] = from;
                    cookie.Values["to"] = to;
                    cookie.Values["query"] = query;
                    cookie.Values["tinhtrang"] = tinhtrang;

                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    _httpContext.Response.Cookies.Set(cookie);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SetSearch",ex);
            }
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Warning = TempData["Warning"];
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Info = TempData["Info"];
            ViewBag.Error = TempData["Error"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&tinhtrang=" + tinhtrang + "&p=";
        }

        //phan cho index phiếu thu va phieu chi
        public void SetSearchPhieuThu_Chi(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string maLoaiPhieu,int pageSize=30)
        {

            var cookie = Request.Cookies.Get(maLoaiPhieu);
            try
            {
                if (from == "" && to == "" && query == "")
                {
                    if (cookie == null)
                    {
                        from = DateTime.Now.ToString(DateFormat);
                        to = DateTime.Now.ToString(DateFormat);
                        cookie = new HttpCookie(maLoaiPhieu);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                        cookie.Values["from"] = from;
                        cookie.Values["to"] = to;
                        cookie.Values["query"] = query;
                        _httpContext.Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        from = cookie.Values["from"];
                        to = cookie.Values["to"];
                        query = cookie.Values["query"];
                    }
                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                }
                else if (from != "" && to != "")
                {
                    if (cookie == null)
                    {
                        f = Convert.ToDateTime(from + " 00:00:00");
                        t = Convert.ToDateTime(to + " 23:59:59");
                        cookie = new HttpCookie(maLoaiPhieu);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                    }
                    cookie.Values["from"] = from;
                    cookie.Values["to"] = to;
                    cookie.Values["query"] = query;

                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    _httpContext.Response.Cookies.Set(cookie);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SetSearchPhieuThu_Chi", ex);
            }
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Warning = TempData["Warning"];
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Info = TempData["Info"];
            ViewBag.Error = TempData["Error"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&maLoaiPhieu=" + maLoaiPhieu + "&p=";
        }

        public void SetSearchPhieuThu_Chi(ref string from, ref string to, ref DateTime f, ref DateTime t, int p, ref string query, string maLoaiPhieu,string chonngay,int pageSize=30)
        {

            var cookie = Request.Cookies.Get(maLoaiPhieu);
            try
            {
                if (from == "" && to == "" && query == "")
                {
                    if (cookie == null)
                    {
                        from = DateTime.Now.ToString(DateFormat);
                        to = DateTime.Now.ToString(DateFormat);
                        cookie = new HttpCookie(maLoaiPhieu);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                        cookie.Values["from"] = from;
                        cookie.Values["to"] = to;
                        cookie.Values["query"] = query;
                        cookie.Values["chonngay"] = chonngay;
                        _httpContext.Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        from = cookie.Values["from"];
                        to = cookie.Values["to"];
                        query = cookie.Values["query"];
                        chonngay = cookie.Values["chonngay"];
                    }
                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                }
                else if (from != "" && to != "")
                {
                    if (cookie == null)
                    {
                        f = Convert.ToDateTime(from + " 00:00:00");
                        t = Convert.ToDateTime(to + " 23:59:59");
                        cookie = new HttpCookie(maLoaiPhieu);
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                    }
                    cookie.Values["from"] = from;
                    cookie.Values["to"] = to;
                    cookie.Values["query"] = query;
                    cookie.Values["chonngay"] = chonngay;
                    f = Convert.ToDateTime(from + " 00:00:00");
                    t = Convert.ToDateTime(to + " 23:59:59");
                    _httpContext.Response.Cookies.Set(cookie);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("SetSearchPhieuThu_Chi", ex);
            }
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Warning = TempData["Warning"];
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Info = TempData["Info"];
            ViewBag.Error = TempData["Error"];
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.Query = query;
            ViewBag.Url = Request.Url.AbsolutePath + "?query=" + query + "&from=" + from + "&to=" + to + "&maLoaiPhieu=" + maLoaiPhieu + "&chonngay" + chonngay + "&p=" ;
        }
        public void RemoveScheduleCache(string controller, string action)
        {
            string path = Url.Action(controller, action);
            Response.RemoveOutputCacheItem(path);
        }
        /// <summary>
        /// Kiem tra truoc khi luu phieu
        /// </summary>
        /// <param name="phieuThu"></param>
        /// <param name="lstChiTietPhieuThu"></param>
        /// <returns></returns>
        public string CheckPhieuThu(PhieuThu phieuThu, List<ChiTietPhieuThu> lstChiTietPhieuThu)
        {
            foreach (ChiTietPhieuThu o in lstChiTietPhieuThu)
            {
                if (!string.IsNullOrEmpty(o.MaNganHang) && o.SoTienThanhToan > 0)
                    return "Vui lòng nhập túi tiền thanh toán";
                if (!string.IsNullOrEmpty(o.MaNganHang) && o.SoTienThanhToan == 0)
                    return "Vui lòng nhập số tiền thanh toán";
            }
            return "";
        }

        /// <summary>
        /// Kiem tra truoc khi luu phieu
        /// </summary>
        /// <param name="phieuThu"></param>
        /// <param name="lstChiTietPhieuThu"></param>
        /// <returns></returns>
        public string CheckPhieuChi(PhieuChi phieuChi, List<ChiTietPhieuChi> lstChiTietPhieuChi)
        {
            foreach (ChiTietPhieuChi o in lstChiTietPhieuChi)
            {
                if (!string.IsNullOrEmpty(o.MaNganHang) && o.SoTienThanhToan > 0)
                    return "Vui lòng nhập túi tiền thanh toán";
                if (!string.IsNullOrEmpty(o.MaNganHang) && o.SoTienThanhToan == 0)
                    return "Vui lòng nhập số tiền thanh toán";
            }
            return "";
        }
    }
}
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Model.Authentication;
using TNK.Services.Authentication;
using TNK.Services.Users;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Data;
using Newtonsoft.Json;
using TNK.Services.Users;
using System.Globalization;
using System.Configuration;

namespace TNK.Controllers
{
    [Authorize]
    public class HomeController : BasePublicController
    {
        private IUserRegistrationService _userRegistrationService;
        private IUserervice _userService;
        private IAuthenticationService _authenticationService;
        private AuthenticationModel _auModel= new AuthenticationModel();
        User CurrentUser;
        private readonly IRepository<User> _userRepository;

        public HomeController(IUserRegistrationService _userRegistrationService
            , IUserervice _userService
            , IAuthenticationService _authenticationService
            , IRepository<User> _userRepository) : base()
        {
            this._userRegistrationService = _userRegistrationService;
            this._authenticationService = _authenticationService;
            this._userService = _userService;
            _auModel.Area = "Web";
            _auModel.Controller = "Home";
            this._userRepository = _userRepository;
            CurrentUser = _authenticationService.GetAuthenticatedUser();

        }
        string format = "#,##0";
        public ActionResult Index()
       {
            if(CurrentUser.UserName == "banhang")
            {
                return Redirect("~/SoThu/TCOBXIndex");
            }
            else
            {
                DataTable dt_new = _userService.getData(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));
                if (dt_new.Rows.Count == 1)
                {
                    DateTime ngay = Convert.ToDateTime(dt_new.Rows[0]["Ngay"] + "/" + DateTime.Now.Year);
                    if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                    {

                        ngay = ngay.AddDays(-1);
                    }
                    else
                    {
                        ngay = ngay.AddDays(1);
                    }
                    DataRow dr = dt_new.NewRow();
                    string ngay_new = ngay.ToString().Substring(0, 5);
                    dr["Ngay"] = ngay_new;
                    dr["LuotBAXE"] = 0;
                    dr["LuotBaoHiem"] = 0;
                    dr["LuotDichVu"] = 0;
                    dr["LuotPhuKien"] = 0;

                    dr["DoanhThuBAXE"] = 0;
                    dr["DoanhThuCocBX"] = 0;
                    dr["DoanhThuNNHBX"] = 0;
                    dr["DoanhThuDichVu"] = 0;
                    dr["DoanhThuCocDV"] = 0;

                    dr["DoanhThuNBHDV"] = 0;
                    dr["DoanhThuBaoHiem"] = 0;
                    dr["DoanhThuPhuKien"] = 0;
                    dr["DoanhThuCacLoaiConLai"] = 0;
                    dr["TongThu"] = 0;

                    dr["ChiMuaXe"] = 0;
                    dr["ChiMuaPhuTung"] = 0;
                    dr["ChiTamUng"] = 0;
                    dr["ChiDatCoc"] = 0;
                    dr["ChiPhi"] = 0;

                    dr["ChiTraNo"] = 0;
                    dr["ChiCacLoaiConLai"] = 0;
                    dr["TongChi"] = 0;
                    if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                    {

                        dt_new.Rows.InsertAt(dr, 0);
                    }
                    else
                    {
                        dt_new.Rows.Add(dr);
                    }
                }
                if (dt_new.Rows.Count == 0)
                {
                    DateTime ngay1 = DateTime.Now.AddDays(-2);
                    if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                    {

                        ngay1 = ngay1.AddDays(-1);
                    }
                    //else
                    //{
                    //    ngay1 = ngay1.AddDays(1);
                    //}
                    DataRow dr = dt_new.NewRow();
                    string ngay_new = ngay1.ToString().Substring(0, 5);
                    dr["Ngay"] = ngay_new;
                    dr["LuotBAXE"] = 0;
                    dr["LuotBaoHiem"] = 0;
                    dr["LuotDichVu"] = 0;
                    dr["LuotPhuKien"] = 0;

                    dr["DoanhThuBAXE"] = 0;
                    dr["DoanhThuCocBX"] = 0;
                    dr["DoanhThuNNHBX"] = 0;
                    dr["DoanhThuDichVu"] = 0;
                    dr["DoanhThuCocDV"] = 0;

                    dr["DoanhThuNBHDV"] = 0;
                    dr["DoanhThuBaoHiem"] = 0;
                    dr["DoanhThuPhuKien"] = 0;
                    dr["DoanhThuCacLoaiConLai"] = 0;
                    dr["TongThu"] = 0;

                    dr["ChiMuaXe"] = 0;
                    dr["ChiMuaPhuTung"] = 0;
                    dr["ChiTamUng"] = 0;
                    dr["ChiDatCoc"] = 0;
                    dr["ChiPhi"] = 0;

                    dr["ChiTraNo"] = 0;
                    dr["ChiCacLoaiConLai"] = 0;
                    dr["TongChi"] = 0;
                    if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                    {

                        dt_new.Rows.InsertAt(dr, 0);
                    }
                    else
                    {
                        dt_new.Rows.Add(dr);
                    }
                    DateTime ngay2 = DateTime.Now.AddDays(-1);
                    if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                    {

                        ngay2 = ngay2.AddDays(-1);
                    }
                    //else
                    //{
                    //    ngay2 = ngay2.AddDays(1);
                    //}
                    DataRow dr2 = dt_new.NewRow();
                    string ngay_new2 = ngay2.ToString().Substring(0, 5);
                    dr2["Ngay"] = ngay_new2;
                    dr2["LuotBAXE"] = 0;
                    dr2["LuotBaoHiem"] = 0;
                    dr2["LuotDichVu"] = 0;
                    dr2["LuotPhuKien"] = 0;

                    dr2["DoanhThuBAXE"] = 0;
                    dr2["DoanhThuCocBX"] = 0;
                    dr2["DoanhThuNNHBX"] = 0;
                    dr2["DoanhThuDichVu"] = 0;
                    dr2["DoanhThuCocDV"] = 0;

                    dr2["DoanhThuNBHDV"] = 0;
                    dr2["DoanhThuBaoHiem"] = 0;
                    dr2["DoanhThuPhuKien"] = 0;
                    dr2["DoanhThuCacLoaiConLai"] = 0;
                    dr2["TongThu"] = 0;

                    dr2["ChiMuaXe"] = 0;
                    dr2["ChiMuaPhuTung"] = 0;
                    dr2["ChiTamUng"] = 0;
                    dr2["ChiDatCoc"] = 0;
                    dr2["ChiPhi"] = 0;

                    dr2["ChiTraNo"] = 0;
                    dr2["ChiCacLoaiConLai"] = 0;
                    dr2["TongChi"] = 0;
                    if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                    {

                        dt_new.Rows.InsertAt(dr2, 0);
                    }
                    else
                    {
                        dt_new.Rows.Add(dr2);
                    }

                }
                for (int i = 0; i < dt_new.Rows.Count; i++)
                {
                    if (dt_new.Rows[i]["TongThu"] != null && dt_new.Rows[i]["TongThu"].ToString() != "")
                    {
                        dt_new.Rows[i]["TongThu"] = string.Format("{0:#.000}", Convert.ToDecimal(dt_new.Rows[i]["TongThu"]) / 1000);
                    }
                    else
                        dt_new.Rows[i]["TongThu"] = 0;
                    if (dt_new.Rows[i]["TongChi"] != null && dt_new.Rows[i]["TongChi"].ToString() != "")
                    {
                        dt_new.Rows[i]["TongChi"] = string.Format("{0:#.000}", Convert.ToDecimal(dt_new.Rows[i]["TongChi"]) / 1000);
                    }
                    else
                        dt_new.Rows[i]["TongChi"] = 0;
                }
                ViewBag.Data_new = dt_new;
                DataTable dt = _userService.getData();
                int bien = 7 - dt.Rows.Count;
                if (bien > 1 && dt.Rows.Count>0)
                {
                    for (int i = 0; i < bien; i++)
                    {
                        DateTime ngay = Convert.ToDateTime(dt?.Rows[dt.Rows.Count - 1]["Ngay"] + "/" + DateTime.Now.Year);
                        ngay = ngay.AddDays(1);
                        DataRow dr = dt.NewRow();
                        string ngay_new = ngay.ToString().Substring(0, 5);
                        dr["Ngay"] = ngay_new;
                        dr["LuotBAXE"] = 0;
                        dr["LuotBaoHiem"] = 0;
                        dr["LuotDichVu"] = 0;
                        dr["LuotPhuKien"] = 0;

                        dr["DoanhThuBAXE"] = 0;
                        dr["DoanhThuCocBX"] = 0;
                        dr["DoanhThuNNHBX"] = 0;
                        dr["DoanhThuDichVu"] = 0;
                        dr["DoanhThuCocDV"] = 0;

                        dr["DoanhThuNBHDV"] = 0;
                        dr["DoanhThuBaoHiem"] = 0;
                        dr["DoanhThuPhuKien"] = 0;
                        dr["DoanhThuCacLoaiConLai"] = 0;
                        dr["TongThu"] = 0;

                        dr["ChiMuaXe"] = 0;
                        dr["ChiMuaPhuTung"] = 0;
                        dr["ChiTamUng"] = 0;
                        dr["ChiDatCoc"] = 0;
                        dr["ChiPhi"] = 0;

                        dr["ChiTraNo"] = 0;
                        dr["ChiCacLoaiConLai"] = 0;
                        dr["TongChi"] = 0;

                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    if (dt.Rows.Count == 6)
                    {
                        DateTime ngay = Convert.ToDateTime(dt.Rows[5]["Ngay"] + "/" + DateTime.Now.Year);
                        ngay = ngay.AddDays(1);
                        DataRow dr = dt.NewRow();
                        string ngay_new = ngay.ToString().Substring(0, 5);
                        dr["Ngay"] = ngay_new;
                        dr["LuotBAXE"] = 0;
                        dr["LuotBaoHiem"] = 0;
                        dr["LuotDichVu"] = 0;
                        dr["LuotPhuKien"] = 0;

                        dr["DoanhThuBAXE"] = 0;
                        dr["DoanhThuCocBX"] = 0;
                        dr["DoanhThuNNHBX"] = 0;
                        dr["DoanhThuDichVu"] = 0;
                        dr["DoanhThuCocDV"] = 0;

                        dr["DoanhThuNBHDV"] = 0;
                        dr["DoanhThuBaoHiem"] = 0;
                        dr["DoanhThuPhuKien"] = 0;
                        dr["DoanhThuCacLoaiConLai"] = 0;
                        dr["TongThu"] = 0;

                        dr["ChiMuaXe"] = 0;
                        dr["ChiMuaPhuTung"] = 0;
                        dr["ChiTamUng"] = 0;
                        dr["ChiDatCoc"] = 0;
                        dr["ChiPhi"] = 0;

                        dr["ChiTraNo"] = 0;
                        dr["ChiCacLoaiConLai"] = 0;
                        dr["TongChi"] = 0;

                        dt.Rows.Add(dr);
                    }
                }
                ViewBag.Data = dt;
                ViewBag.ShowRoom = ConfigurationManager.AppSettings["ShowRoom"];
                return View();
            }
        }
        public string Index_New(DateTime FromDate)
        {
            try
            {
                //DateTime to1 = new DateTime(FromDate.Year, FromDate.Day, FromDate.Month);
                DataTable dt_new = _userService.getData(FromDate.AddDays(-1), FromDate);

                if(dt_new.Rows.Count ==0)
                {
                    DataRow dr = dt_new.NewRow();
                    string ngay_new = FromDate.ToString().Substring(0, 5);
                    dr["Ngay"] = ngay_new;
                    dr["LuotBAXE"] = 0;
                    dr["LuotBaoHiem"] = 0;
                    dr["LuotDichVu"] = 0;
                    dr["LuotPhuKien"] = 0;

                    dr["DoanhThuBAXE"] = 0;
                    dr["DoanhThuCocBX"] = 0;
                    dr["DoanhThuNNHBX"] = 0;
                    dr["DoanhThuDichVu"] = 0;
                    dr["DoanhThuCocDV"] = 0;

                    dr["DoanhThuNBHDV"] = 0;
                    dr["DoanhThuBaoHiem"] = 0;
                    dr["DoanhThuPhuKien"] = 0;
                    dr["DoanhThuCacLoaiConLai"] = 0;
                    dr["TongThu"] = 0;

                    dr["ChiMuaXe"] = 0;
                    dr["ChiMuaPhuTung"] = 0;
                    dr["ChiTamUng"] = 0;
                    dr["ChiDatCoc"] = 0;
                    dr["ChiPhi"] = 0;

                    dr["ChiTraNo"] = 0;
                    dr["ChiCacLoaiConLai"] = 0;
                    dr["TongChi"] = 0;

                    dt_new.Rows.Add(dr);
                }
                if(dt_new.Rows.Count == 1)
                {
                    DataRow dr = dt_new.NewRow();
                    string ngay_new = "";
                    if (dt_new.Rows[0]["Ngay"].ToString() == FromDate.AddDays(-1).ToString("dd/MM"))
                    {
                         ngay_new = FromDate.ToString().Substring(0, 5);
                    }
                    else
                    {
                         ngay_new = FromDate.AddDays(-1).ToString().Substring(0, 5);
                    }
                    dr["Ngay"] = ngay_new;
                    dr["LuotBAXE"] = 0;
                    dr["LuotBaoHiem"] = 0;
                    dr["LuotDichVu"] = 0;
                    dr["LuotPhuKien"] = 0;

                    dr["DoanhThuBAXE"] = 0;
                    dr["DoanhThuCocBX"] = 0;
                    dr["DoanhThuNNHBX"] = 0;
                    dr["DoanhThuDichVu"] = 0;
                    dr["DoanhThuCocDV"] = 0;

                    dr["DoanhThuNBHDV"] = 0;
                    dr["DoanhThuBaoHiem"] = 0;
                    dr["DoanhThuPhuKien"] = 0;
                    dr["DoanhThuCacLoaiConLai"] = 0;
                    dr["TongThu"] = 0;

                    dr["ChiMuaXe"] = 0;
                    dr["ChiMuaPhuTung"] = 0;
                    dr["ChiTamUng"] = 0;
                    dr["ChiDatCoc"] = 0;
                    dr["ChiPhi"] = 0;

                    dr["ChiTraNo"] = 0;
                    dr["ChiCacLoaiConLai"] = 0;
                    dr["TongChi"] = 0;
                    if(dt_new.Rows[0]["Ngay"].ToString() == FromDate.AddDays(-1).ToString("dd/MM"))
                    {
                        dt_new.Rows.Add(dr);
                    }
                    else
                    {
                        dt_new.Rows.InsertAt(dr, 0);
                    }
                }
                //for (int i = 0; i < dt_new.Rows.Count; i++)
                //{
                //    dt_new.Rows[i]["TongThu"] = string.Format("{0:#.000}", Convert.ToDecimal(dt_new.Rows[i]["TongThu"]) / 1000);
                //    dt_new.Rows[i]["TongChi"] = string.Format("{0:#.000}", Convert.ToDecimal(dt_new.Rows[i]["TongChi"]) / 1000);
                //}
                ViewBag.Data_new = dt_new;
                ViewBag.ShowRoom = ConfigurationManager.AppSettings["ShowRoom"];
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt_new, Formatting.Indented);
                return JSONString;
            }
            catch (Exception)
            {

                throw;
            }
            return "";
        }

        public string Index_NewOfWeek(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                DataTable dt = _userService.getData(FromDate, ToDate);
                ViewBag.Data_new = dt;
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt, Formatting.Indented);
                return JSONString;
            }
            catch (Exception)
            {

                throw;
            }
            return "";
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NoAccess()
        {
            Session.Remove("Menu");
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNK.Core.Domain;
using TNK.Services.Authentication;
using TNK.Services.SoChi;
using TNK.Services.SoQuyetToan;
using TNK.Services.SoThu;

namespace TNK.Controllers
{
    public class HuaTangController : BasePublicController
    {
        // GET: HuaTang
        ISoChiService _soChiService;
        ISoQuyetToanService _soQuyetToanService;
        IAuthenticationService _authenticationService;
        User CurrentUser;
        IPhieuThuService _phieuThuService;
        public HuaTangController(HttpContextBase _httpContext, ISoChiService _soChiService, ISoQuyetToanService _soQuyetToanService, IAuthenticationService _authenticationService, IPhieuThuService _phieuThuService)
        {
            this._httpContext = _httpContext;
            this._soQuyetToanService = _soQuyetToanService;
            this._soChiService = _soChiService;
            this._authenticationService = _authenticationService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
            this._phieuThuService = _phieuThuService;
        }
        public ActionResult Index(string from = "", string to = "", string query = "", int p = 1, string status = "", string tab = "", int pageSize = 30)
        {
            DateTime start = DateTime.Now;
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "HuaTang", pageSize);
                tab = tab == "" ? "TBAHI" : tab;
                var model = _soQuyetToanService.GetDanhSachNKMPK("NKMPK", f, t, query, p, ref total, pageSize, status);
                ViewBag.Total = total;
                //ViewBag.PageSize = pageSize;
                ViewBag.TypePaging = "Index";
                ViewBag.TbodyId = "tbodyKM";
                ViewBag.Status = status;
                //ViewBag.PageIndex = p;
                ViewBag.Url = Request.Url.AbsolutePath + "?from=" + from + "&to=" + to + "&query=" + query + "&status=" + status + "&p=";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("HuaTangController:tbodyKMTab" + ex.ToString());
                TempData["Error"] = ex;
                DateTime end = new DateTime();
                end = DateTime.Now;
            }
            return View();
        }

        public ActionResult PhieuQTIndex(string from = "", string to = "", string query = "", int p = 1, string status = "", string tab = "", int pageSize = 30, string loai="")
        {
            DateTime start = DateTime.Now;
            try
            {
                DateTime f = new DateTime(), t = new DateTime();
                SetSearch(ref from, ref to, ref f, ref t, p, ref query, "PhieuThuQTIndex", pageSize);
                tab = tab == "" ? "PhieuThuQTIndex" : tab;
                var model = _soQuyetToanService.GetDanhSachPhieuQT("NKMPK", f, t, query, p, ref total, pageSize, status, loai);
                ViewBag.Total = total;
                //ViewBag.PageSize = pageSize;
                ViewBag.TypePaging = "Index";
                ViewBag.TbodyId = "tbodyKM";
                //ViewBag.PageIndex = p;
                DateTime end = new DateTime();
                end = DateTime.Now;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("HuaTangController:tbodyKMTab" + ex.ToString());
                TempData["Error"] = ex;
                DateTime end = new DateTime();
                end = DateTime.Now;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreatePhieuQuyetToan(List<string> maPhieuNos)
        {
            if (maPhieuNos == null || maPhieuNos.Count == 0)
            {
                return Json(new { Msg = "Không nhận được dữ liệu" });
            }
            string maPhieu = string.Join(",", maPhieuNos);
            var user = _authenticationService.GetAuthenticatedUser().UserId;
            _soQuyetToanService.Create(maPhieu, user);
            return Json(new { });
        }
        public ActionResult Delete(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Content("Bạn không có quyền xóa");
            }
            string message = _soQuyetToanService.Delete(id, CurrentUser.UserId);
            if (string.IsNullOrEmpty(message))
                return Content("success");
            else
                return Content(message);

        }
        public ActionResult NoPKHTIndex(string from = "", string to = "", string query = "", int p = 1, string export = "")
        {
            ViewBag.Alert = TempData["Alert"];
            ViewBag.Title = "Thu phụ kiện 2";
            DateTime f = new DateTime(), t = new DateTime();
            SetSearch(ref from, ref to, ref f, ref t, p, ref query, "TPHKI");
            if (export == "Export")
            {
                //ExportExcel("TPHKI", f, t, query);
            }
            var model = _phieuThuService.GetTPHKI(f, t, query, p, ref total, pageSize);
            ViewBag.Total = total;
            return View("~/Views/SOThu/TPHKIIndex.cshtml", model);
        }

    }
}
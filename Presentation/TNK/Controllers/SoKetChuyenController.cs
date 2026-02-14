using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNK.Core.Domain;
using TNK.Services.Authentication;
using TNK.Services.Manager;

namespace TNK.Controllers
{
    public class SoKetChuyenController : BasePublicController
    {
        // GET: SoKetChuyen
        ISoKetChuyenService _soKetChuyenService;
        IAuthenticationService _authenticationService;
        User CurrentUser;
        public SoKetChuyenController(
            ISoKetChuyenService _soKetChuyenService,
            IAuthenticationService _authenticationService
            ) : base()
        {
            this._soKetChuyenService = _soKetChuyenService;
            this._authenticationService = _authenticationService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }

        public ActionResult Index()
        {
            //string strDateKC = _soKetChuyenService.LayNgayKetChuyenKeTiep();
            string strDateKC = "";
            DataTable dtbCashCosts = _soKetChuyenService.LayDanhSachSoKetChuyen("KC");
            if (dtbCashCosts.Rows.Count > 0)
            {
                strDateKC = dtbCashCosts.Rows[0]["NgayKetChuyen"].ToString().Substring(0,7);
            }
            if (!string.IsNullOrEmpty(strDateKC)) {
                strDateKC = strDateKC.Substring(0, 4) + "/" + strDateKC.Substring(4, 2);// + "/" + strDateKC.Substring(6, 2);
            }
            ViewBag.DateKC = strDateKC;
            return View();
        }

        public ActionResult SoKetChuyen()
        {
            DataTable dtbCashCosts = _soKetChuyenService.LayDanhSachSoKetChuyen("KC");
            return PartialView(dtbCashCosts);
        }

        public ActionResult DanhSachHuy()
        {
            DataTable dtbCashCosts = _soKetChuyenService.LayDanhSachSoKetChuyen("HU");
            return PartialView(dtbCashCosts);
        }

        public ActionResult TaoKetChuyen(string strNgayKetChuyen)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string result = _soKetChuyenService.TaoKetChuyen("", CurrentUser.UserName);
            if (!string.IsNullOrEmpty(result))
            {
                return Json(new { IsError = true, Message = result });
            }
            return Json(new { IsError = false });
        }

        public ActionResult HuyKetChuyen(Guid gdId, string strGhiChu)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string result = _soKetChuyenService.HuyKetChuyen(gdId, CurrentUser.UserName, strGhiChu);
            if (!string.IsNullOrEmpty(result))
            {
                return Json(new { IsError = true, Message = result });
            }
            return Json(new { IsError = false });
        }

        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}
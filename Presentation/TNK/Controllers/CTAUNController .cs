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
using TNK.Services.Authentication;

namespace TNK.Controllers
{
    public class CTAUNController : Controller
    {
        public string LoaiPhieu = "CTAUN";
        public string ViewIndexName { get { return "../SoChi/" + LoaiPhieu + "Index"; } }
        public string ViewCreateName { get { return "../SoChi/" + LoaiPhieu + "Create"; } }
        public string ViewEditName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string ViewDeleteName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string UrlIndex { get { return "/" + LoaiPhieu + "/Index"; } }

        TNK.Services.Users.IUserervice _userService;
        ICategoryService _categoryService;
        ISoChiService _soChiService;
        IAuthenticationService _authenticationService;
        User CurrentUser;

        public CTAUNController(IUserervice _userService
            , ICategoryService _categoryService
            , ISoChiService _soChiService
            , IAuthenticationService _authenticationService
            ) : base()
        {
            this._userService = _userService;
            this._categoryService = _categoryService;
            this._soChiService = _soChiService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }
       
        public SoChiModel GetModel(string code)
        {
            
            SoChiModel model = new SoChiModel();
            model.LoaiPhieu = LoaiPhieu;
            switch (code)
            {
                case "CMHTD":
                    model.DoiTac = _soChiService.GetDoiTac("NCC");
                    break;
            }
            model.Users = _userService.get();
            model.HTTT = _soChiService.GetHTTT();

            return model;
        }

        #region Chi tạm ứng
        public ActionResult Index()
        {
            
            ViewBag.Alert = TempData["Alert"];
            ViewBag.LoaiPhieu = LoaiPhieu;
            var model = _soChiService.GetPhieuChiList(LoaiPhieu, DateTime.Now, DateTime.Now);
            return View(ViewIndexName, model);
        }
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo phiếu";
            SoChiModel model = GetModel(LoaiPhieu);
            model.CTPT = new List<ChiTietPhieuChi>();
            model.CTPT.Add(new ChiTietPhieuChi());
            return View(ViewCreateName, model);
        }
        [HttpPost]
        public ActionResult Create(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Title = "Tạo phiếu ";
            item.MaLoaiPhieu = LoaiPhieu;
            string message = _soChiService.CreatePhieuChi(item, httt);
            if (message =="")
            {
                TempData["Info"] = "Tạo mới thành công";
                return Redirect(UrlIndex);
            }
            else
            {
                TempData["Info"] = "Tạo mới thất bại" + message;
                ViewBag.Error = "Tạo mới thất bại" + message;
            }
                
            SoChiModel model = GetModel(LoaiPhieu);
            model.Item = item;
            model.CTPT = httt;
            return View(ViewCreateName, model);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật phiếu mua ";
            SoChiModel model = GetModel(LoaiPhieu);
            model.Item = _soChiService.GetPhieuChi(id);
            model.LoaiPhieu = LoaiPhieu;
            if (model.Item == null || model.Item.MaLoaiPhieu != LoaiPhieu)
            {
                return Redirect(UrlIndex);
            }
            model.CTPT = _soChiService.GetCTPC(id);
            if (model.CTPT.Count == 0)
            {
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
            }
            return View(ViewEditName, model);
        }
        [HttpPost]
        public ActionResult Edit(PhieuChi item, List<ChiTietPhieuChi> httt)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Title = "Cập nhật phiếu";
            string sessionId = "";
            string message = _soChiService.UpdatePhieuChi(item, httt,ref sessionId);
            if (string.IsNullOrEmpty(message))
            {
                ViewBag.Info = "Cập nhật thành công";
               // return Redirect(UrlIndex);
            }
            else
                ViewBag.Error = "Cập nhật thất bại";
            SoChiModel model = GetModel(LoaiPhieu);
            model.LoaiPhieu = LoaiPhieu;
            model.Item = item;
            model.CTPT = _soChiService.GetCTPC(item.MaPhieuChi);
            if (model.CTPT.Count == 0)
            {
                model.CTPT = new List<ChiTietPhieuChi>();
                model.CTPT.Add(new ChiTietPhieuChi());
            }
            return View(model);
        }
        public ActionResult Delete(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            string message = _soChiService.DeletePhieuChi(id);
            if (string.IsNullOrEmpty(message))
            {
                TempData["Info"] = "Xóa thành công";
            }
            else
                TempData["Error"] = "Xóa thất bại:" + message;
            return Redirect(UrlIndex);
        }
       
        #endregion

    }
}
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
    public class CGCNGController : Controller
    {
        public string LoaiPhieu = "CGCNG";
        public string ViewIndexName { get { return "../SoChi/" + LoaiPhieu + "Index"; } }
        public string ViewCreateName { get { return "../SoChi/" + LoaiPhieu + "Create"; } }
        public string ViewEditName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string ViewDeleteName { get { return "../SoChi/" + LoaiPhieu + "Edit"; } }
        public string UrlIndex { get { return "/" + LoaiPhieu + "/Index"; } }

        IUserervice _userService;
        ICategoryService _categoryService;
        ISoChiService _soChiService;
        ISoThuService _soThuService;
        IAuthenticationService _authenticationService;
        User CurrentUser;

        public CGCNGController(IUserervice _userService
            , ICategoryService _categoryService
            , ISoChiService _soChiService
            , ISoThuService _soThuService
            , IAuthenticationService _authenticationService
            ) : base()
        {
            this._userService = _userService;
            this._categoryService = _categoryService;
            this._soChiService = _soChiService;
            this._soThuService = _soThuService;
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
                case "CMXTT":
                    model.DoiTac = _soChiService.GetDoiTac("NCC");
                   // model.MaXe =_loa .GetCategoryItem("MAX");
                    break;
                case "CGCNG":
                    model.DoiTac = _soChiService.GetDoiTac("NCC");
                    model.ListNoPhaiTraTuPhieuThu = _soChiService.GetNoPhaiTraTuPhieuThuList("NGCNG", DateTime.Now, DateTime.Now);
                    break;
                   // model.ItemNoPhaiTraTuPhieuThu = _soChiService.GetNoPhaiTraTuPhieuThu("NGCNG",DateTime.Now,DateTime.Now);
            }
            model.Users = _userService.get();
            model.HTTT = _soChiService.GetHTTT();

            return model;
        }

        #region 
        public ActionResult Index()
        {
            ViewBag.Alert = TempData["Alert"];
            var model = GetModel(LoaiPhieu); // _soChiService.GetPhieuChiList(LoaiPhieu, DateTime.Now, DateTime.Now);
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
                TempData["Alert"] = "Tạo mới thành công";
                return Redirect(UrlIndex);
            }
            else
            {
                TempData["Alert"] = "Tạo mới thất bại :" + message;
                ViewBag.Alert = "Tạo mới thất bại:" + message;
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
            //if (_soChiService.UpdatePhieuChi(item, httt))
            //{
            //    ViewBag.Alert = "Cập nhật thành công";
            //    return Redirect(UrlIndex);
            //}
            //else
            //    ViewBag.Alert = "Cập nhật thất bại";
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
            //if (_soChiService.DeletePhieuChi(id))
            //    TempData["Alert"] = "Xóa thành công";
            //else
            //    TempData["Alert"] = "Xóa thất bại";
            return Redirect(UrlIndex);
        }
       
        #endregion

    }
}
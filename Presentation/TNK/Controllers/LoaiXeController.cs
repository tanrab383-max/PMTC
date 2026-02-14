using TNK.Core.Domain;
using TNK.Services.Authentication;
using TNK.Services.Catalog;
using System.Web.Mvc;
using System.Linq;
using TNK.Model;
using System.Collections.Generic;
using System;
using TNK.Core.Domain.View;

namespace TNK.Controllers
{
    public class LoaiXeController : BasePublicController
    {
        private ILoaiXeService _loaiXeService;
        private IAuthenticationService _authenticationService;
        private IDoiTacService _doiTacService;
        User CurrentUser;
        public LoaiXeController(ILoaiXeService _loaiXeService
            , IAuthenticationService _authenticationService
            , IDoiTacService _doiTacService
            ) : base()
        {
            this._loaiXeService = _loaiXeService;
            this._doiTacService = _doiTacService;
            this._authenticationService = _authenticationService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }
        // GET: Category
        public ActionResult Index()
        {
            ViewBag.Alert = TempData["Alert"];
            var model = _loaiXeService.GetViewLoaiXeList();
            return View(model);
            //return View("~/View/Category/LoaiXeIndex.cshtml", model);
            //return View("~/Views/Category/LoaiXeIndex.cshtml");
        }

        public ActionResult GiaNiemYet(LoaiXe item, int p = 1, int pageSize = 30)
        {
            p = 1;
            int total = 0;
            LoaiXeModel model = new LoaiXeModel();
            model.ListModelXe = _loaiXeService.GetCategoryItemByParent("MODEL");
            model.ListMauXe = _loaiXeService.GetCategoryItemByParent("COLOR");
            model.ListDoiXe = _loaiXeService.GetCategoryItemByParent("YEAR");
            model.ListGrad = _loaiXeService.GetCategoryItemByParent("GRAD");
            model.ListHang = _doiTacService.GetNCC();
            model.Item = item;
            List<ViewLoaiXe> lst = _loaiXeService.SearchViewLoaiXeList(item, ref total, p, pageSize);
            ViewBag.ListLoaiXe = lst;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.Url = "/LoaiXe/SearchGiaNiemYet" + "?MaLoaiXe=" + item.MaLoaiXe + "&MaModel=" + item.MaModel + "&Grad=" + item.Grad + "&DoiXe=" + item.DoiXe + "&MaMau = " + item.MaMau + "&XuatXu=" + item.XuatXu + "&p=";
            return View("~/Views/Category/GiaNiemYet.cshtml", model);
        }
        public ActionResult SearchGiaNiemYet(LoaiXe item, int p = 1, int pageSize = 30)
        {
            int total = 0;
            string XuatXu = Request.QueryString["XuatXu"];
            LoaiXeModel model = new LoaiXeModel();
            model.ListModelXe = _loaiXeService.GetCategoryItemByParent("MODEL");
            model.ListMauXe = _loaiXeService.GetCategoryItemByParent("COLOR");
            model.ListDoiXe = _loaiXeService.GetCategoryItemByParent("YEAR");
            model.ListGrad = _loaiXeService.GetCategoryItemByParent("GRAD");
            model.ListHang = _doiTacService.GetNCC();
            model.Item = item;
            List<ViewLoaiXe> lst = _loaiXeService.SearchViewLoaiXeList(item, ref total, p, pageSize);
            ViewBag.ListLoaiXe = lst;
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.Url = Request.Url.AbsolutePath + "?MaLoaiXe=" + item.MaLoaiXe + "&MaModel=" + item.MaModel + "&Grad=" + item.Grad + "&DoiXe=" + item.DoiXe + "&MaMau = " + item.MaMau + "&XuatXu=" + item.XuatXu + "&p=";
            return View("~/Views/Category/GiaNiemYet.cshtml", model);
        }
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo phiếu";
            LoaiXeModel model = new LoaiXeModel();
            model.ListModelXe = _loaiXeService.GetCategoryItemByParent("MODEL");
            model.ListMauXe = _loaiXeService.GetCategoryItemByParent("COLOR");
            model.ListDoiXe = _loaiXeService.GetCategoryItemByParent("YEAR");
            model.ListGrad = _loaiXeService.GetCategoryItemByParent("GRAD");
            model.ListHang = _doiTacService.GetNCC();
            return View("~/Views/Category/LoaiXeCreate.cshtml", model);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật ";
            LoaiXeModel model = new LoaiXeModel();
            model.ListModelXe = _loaiXeService.GetCategoryItemByParent("MODEL");
            model.ListMauXe = _loaiXeService.GetCategoryItemByParent("COLOR");
            model.ListDoiXe = _loaiXeService.GetCategoryItemByParent("YEAR");
            model.ListGrad = _loaiXeService.GetCategoryItemByParent("GRAD");
            model.ListHang = _doiTacService.GetNCC();
            model.Item = _loaiXeService.GetLoaiXe(id);

            return View("~/Views/Category/LoaiXeEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(LoaiXe item)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Title = "Cập nhật ";
            if (_loaiXeService.Update(item))
            {
                TempData["Alert"] = "Tạo mới thành công";
                return Redirect("~/Category/Index");
            }
            else
                TempData["Alert"] = "Cập nhật thất bại";
            return Redirect("~/Category/Index");

        }

        public ActionResult EditLoaiXe(int Id, string strMaLoaiXe, double dblGiaNiemYet, string XuatXu, string DoiXe)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (string.IsNullOrEmpty(strMaLoaiXe))
                return Json(new { IsError = true, Message = "Lỗi: Không có thông tin cập nhật. Vui lòng kiểm tra lại." });

            var obj = _loaiXeService.GetLoaiXe(strMaLoaiXe);
            double GNYOLD = obj.GiaNiemYet.Value;
            if (obj == null)
                return Json(new { IsError = true, Message = "Thông tin không tồn tại. Vui lòng kiểm tra lại." });
            string namDoiXe = strMaLoaiXe.Substring(strMaLoaiXe.Length - 4, 4);
            CategoryItem itemNamDoiXe = _loaiXeService.NamDoiXe(namDoiXe);
            CategoryItem itemNamDoiXe_Edit = _loaiXeService.NamDoiXe_Edit(DoiXe);
            string strMaLoaiXeNew = "";
            if (itemNamDoiXe != null)
            {
                if (strMaLoaiXe.Contains("CKD") == true)
                {
                    strMaLoaiXeNew = strMaLoaiXe.Replace("CKD", XuatXu);
                }
                else if (strMaLoaiXe.Contains("CBU") == true)
                {
                    strMaLoaiXeNew = strMaLoaiXe.Replace("CBU", XuatXu);
                }
                if (namDoiXe != "")
                {
                    if (strMaLoaiXeNew.Contains(namDoiXe) == true)
                    {
                        strMaLoaiXeNew = strMaLoaiXeNew.Replace(namDoiXe, itemNamDoiXe_Edit.Code);
                    }
                }
            }
            else
            {
                strMaLoaiXeNew = strMaLoaiXe.Substring(0, (strMaLoaiXe.Length - 3));
                strMaLoaiXeNew = strMaLoaiXeNew + XuatXu + itemNamDoiXe_Edit.Code;
            }
            var obj_new = _loaiXeService.GetLoaiXe(strMaLoaiXeNew);
            if (obj_new != null && obj_new.GiaNiemYet == dblGiaNiemYet)
                return Json(new { IsError = true, Message = "Đã tồn tại . Vui lòng kiểm tra lại." });
            obj.GiaNiemYet = dblGiaNiemYet;
            obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
            obj.UpdatedDate = DateTime.Now;
            obj.XuatXu = XuatXu;
            obj.DoiXe = Convert.ToInt32(DoiXe);
            //obj.MaLoaiXe = strMaLoaiXeNew;
            obj.Id = Id;
            if (_loaiXeService.Update_New(obj, strMaLoaiXeNew, GNYOLD))
                return Json(new { IsError = false, Message = "Cập nhật thành công." });
            else
                return Json(new { IsError = true, Message = "Cập nhật thất bại." });
        }

        [HttpPost]
        public ActionResult Create(LoaiXe item)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            //ViewBag.Title = "Tạo phiếu ";
            ViewBag.Alert = TempData["Alert"];
            string message = _loaiXeService.Insert(item);
            if (message == "")
            {
                TempData["Alert"] = "Tạo mới thành công";
                return Redirect("~/LoaiXe/GiaNiemYet");
            }
            else
            {
                TempData["Alert"] = "Tạo mới thất bại: " + message;
                return Redirect("~/LoaiXe/Create");
                //ViewBag.Alert = "Tạo mới thất bại:" +message ;
            }

        }

        public ActionResult Delete(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            var result = _loaiXeService.Delete(id);
            string message = string.Empty;
            if (result == "S")
            {
                message = "";
            }
            else
            {
                switch (result)
                {
                    case "N":
                        message = "Xóa thất bại. Mã loại xe không tồn tại";
                        break;
                    case "F":
                        message = "Xóa thất bại. Có lỗi xảy ra";
                        break;
                    case "I":
                        message = "Xóa thất bại. Mã loại xe đã được sử dụng, không được xóa";
                        break;
                    default:
                        break;
                }
            }

            return Json(new { isError = message });
        }
        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}
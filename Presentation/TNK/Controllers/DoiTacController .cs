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
    public class DoiTacController : BasePublicController
    {
        private IDoiTacService _doiTacService;
        private ICategoryService _categoryService;
        private IAuthenticationService _authenticationService;
        private ITuiDinhKhoanService tuiDinhKhoanService;
        User CurrentUser;
        public DoiTacController(IDoiTacService _doiTacService
            , IAuthenticationService _authenticationService
            , ICategoryService _categoryService
            , ITuiDinhKhoanService tuiDinhKhoanService) : base()
        {
            this._doiTacService = _doiTacService;
            this._authenticationService = _authenticationService;
            this._categoryService = _categoryService;
            this.tuiDinhKhoanService = tuiDinhKhoanService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }
        // GET: Category
        static List<ViewDoiTac> list = new List<ViewDoiTac>();
        public ActionResult Index()
        {
            ViewBag.Alert = TempData["Alert"];
            var model = _doiTacService.GetDoiTacList();
            return View(model);
        }

        public ActionResult ListEdit(DoiTac model, int p = 1, int pageSize = 30)
        {
            p = 1;
            int total = 0;
            ViewBag.DoiTac = _categoryService.GetCategoryItem("NDT");
            ViewBag.ListDoiTac = _doiTacService.SearchDoiTacList(model, ref total, p, pageSize);
            list = ViewBag.ListDoiTac;
            ViewBag.LinkCreate = "Create";
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.Url = "/DoiTac/Search" + "?MaDoiTac=" + model.MaDoiTac + "&MaVietTat=" + model.MaVietTat + "&TenDoiTac=" + model.TenDoiTac + "&LoaiDoiTac=" + model.LoaiDoiTac + "&p=";
            return View(model);
        }
        public ActionResult Search(DoiTac model, int p = 1, int pageSize = 30)
        {
            ViewBag.DoiTac = _categoryService.GetCategoryItem("NDT");
            int total = 0;
            ViewBag.ListDoiTac = _doiTacService.SearchDoiTacList(model, ref total, p, pageSize);
            list = ViewBag.ListDoiTac;
            ViewBag.LinkCreate = "Create";
            ViewBag.PageIndex = p;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.Url = Request.Url.AbsolutePath + "?MaDoiTac=" + model.MaDoiTac + "&MaVietTat=" + model.MaVietTat + "&TenDoiTac=" + model.TenDoiTac + "&LoaiDoiTac=" + model.LoaiDoiTac + "&p=";
            return View("ListEdit", model);
        }
        public ActionResult Create()
        {
            ViewBag.Edit = false;
            ViewBag.Title = "Thêm mới đối tác";
            ViewBag.DoiTac = _categoryService.GetCategoryItem("NDT");
            return View("Form");
        }
        [HttpPost]
        public ActionResult Create(DoiTac model, bool? TaoTuiDKHoan)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Edit = false;
            ViewBag.Title = "Thêm mới đối tác";
            ViewBag.Alert = "";
            model.MaDoiTac = model.MaDoiTac.Trim();
            var item = _doiTacService.GetDoiTac(model.MaDoiTac);
            model.CreatedBy = _authenticationService.GetAuthenticatedUser().UserId;
            model.CreatedDate = DateTime.Now;
            model.UpdatedBy = model.CreatedBy;
            model.UpdatedDate = model.CreatedDate;
            model.IsDeleted = false;
            model.IsActive = true;
            //string alert = "";
            if (item != null)
            {
                TempData["Info"] = "Mã đã tồn tại";
                ViewBag.Alert = TempData["Info"];
                return Redirect("~/DoiTac/ListEdit");
                //ViewBag.Alert = TempData["Alert"];
                //return Redirect("~/DoiTac/Create");
            }
            else
            {
                var item_doitac = _doiTacService.CheckDoiTac(model.MaDoiTac);
                if(item_doitac != null)
                {
                    _doiTacService.Delete(item_doitac);
                }
                if (_doiTacService.Create(model, _authenticationService.GetAuthenticatedUser().UserId))
                {
                    ViewBag.Info = "Tạo đối tác thành công";
                    if(TaoTuiDKHoan ?? false)
                    {
                        TuiDinhKhoan dinhKhoan = tuiDinhKhoanService.GetTuiDinhKhoan(model.MaDoiTac);
                        if(dinhKhoan!=null)
                        {
                            ViewBag.Alert = "Đã tồn tại túi định khoản";
                        }
                        else
                        {
                            TuiDinhKhoan tuiDinhKhoan = new TuiDinhKhoan
                            {
                                MaTui = model.MaDoiTac,
                                TenTui = model.MaDoiTac,
                                LoaiTui = "NH",
                                MaTuiCha = "NH",
                                SoTienDangCo = 0,
                                SoTienKhoiTao = 0,
                                SoTienDangCoLanTruoc = 0,
                                SoTienThayDoi = 0,
                                CreatedBy = model.CreatedBy,
                                CreatedDate = model.CreatedDate,
                                IsDeleted = false,
                                IsActive = true
                            };
                            tuiDinhKhoanService.Create(tuiDinhKhoan);
                        }
                    }
                    return Redirect("~/DoiTac/ListEdit");
                }
                else
                {
                    TempData["Error"] = "Có lỗi xảy ra";
                }
            }
            //ViewBag.Alert = TempData["Alert"];
            return View();
            //return null;
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_doiTacService.Delete(id,_authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Alert"] = "Xóa thành công";
                return Content("true");
            }
            else
            {
                TempData["Alert"] = "Xóa thất bại";
                return Content("false");
            }
            //return Redirect("~/DoiTac/ListEdit");
        }
        public JsonResult LayMaDoiTac(string MaDoiTac)
        {
            bool result = true;
            MaDoiTac = MaDoiTac.Trim();
            if (MaDoiTac != "")
            {
                if(list == null)
                    _doiTacService.GetDoiTacList();
                foreach (var temp in list)
                {
                    if (temp.madoitac == MaDoiTac)
                    {
                        result = false;
                        break;
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.Edit = true;
            ViewBag.Title = "Cập nhật đối tác";
            ViewBag.DoiTac = _categoryService.GetCategoryItem("NDT");
            var model = _doiTacService.GetDoiTac(id);
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Edit(DoiTac model)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            ViewBag.Edit = true;
            ViewBag.Title = "Cập nhật đối tác";
            var obj = _doiTacService.GetDoiTac(model.MaDoiTac);
            if (model == null)
            {
                ViewBag.Alert = "Not Found Item";
                return View("Form", model);
            }
            obj.MaDoiTac = model.MaDoiTac;
            obj.MaVietTat = model.MaVietTat;
            obj.TenDoiTac = model.TenDoiTac;
            obj.GhiChu = model.GhiChu;
            obj.TyLeHoaHong = model.TyLeHoaHong;
            obj.LoaiDoiTac = model.LoaiDoiTac;

            if (_doiTacService.Update(model))
            {
                TempData["Alert"] = "Update Success";
                return Redirect("~/Category/Index");
            }
            else
            {
                ViewBag.Alert = "Error";
                return View("Form", model);
            }
        }
        [HttpPost]
        public ActionResult EditDoiTac(string strMaDoiTac,string loaiDoiTac, double dblTyLeHoaHong,double HanMucVay, string DoiTacNoiBo)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (string.IsNullOrEmpty(strMaDoiTac))
                return Json(new { IsError = true, Message = "Lỗi: Không có thông tin cập nhật. Vui lòng kiểm tra lại." });

            var obj = _doiTacService.GetDoiTac(strMaDoiTac);
            if (obj == null)
                return Json(new { IsError = true, Message = "Thông tin không tồn tại. Vui lòng kiểm tra lại." });
            obj.MaDoiTac = obj.MaDoiTac.Trim();
            obj.TyLeHoaHong = dblTyLeHoaHong;
            obj.LoaiDoiTac = loaiDoiTac;
            obj.HanMucVay = HanMucVay;
            obj.DoiTacNoiBo = DoiTacNoiBo;
            obj.UpdatedBy = _authenticationService.GetAuthenticatedUser().UserId;
            obj.UpdatedDate = DateTime.Now;
            if (_doiTacService.Update(obj))
                return Json(new { IsError = false, Message = "Cập nhật thành công." });
            else
                return Json(new { IsError = true, Message = "Cập nhật thất bại." });
        }

        public ActionResult UpdateCategory(DoiTac edit)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_doiTacService.Update(edit, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Alert"] = "Cập nhật thành công";
            }
            else
            {
                TempData["Alert"] = "Cập nhật thất bại. Code đã tồn tại";
            }
            return Redirect("Index");
        }

        public ActionResult AddCategory(DoiTac item)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_doiTacService.Create(item, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Alert"] = "Thêm mới thành công";
            }
            else
            {
                TempData["Alert"] = "Thêm mới thất bại. Code đã tồn tại";
            }
            return Redirect("Index");
        }

        public ActionResult Get(string maDoiTac)
        {
            var model = _doiTacService.GetDoiTac(maDoiTac);
            return Json(
                new
                {
                    id = model.Id,
                    name = model.TenDoiTac,
                    code = model.MaDoiTac,
                    note = model.GhiChu
                }, JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult GetCategory(string parent = "")
        {
            List<TreeItem> model = new List<TreeItem>();
            if (parent == "#")
            {
                TreeItem item = new TreeItem();
                item.id = "node_0";
                item.text = "Root";
                item.children = true;
                item.type = "root";
                item.children = true;
                item.icon = "fa fa-folder";
                model.Add(item);
            }
            else
            {
                parent = parent.Replace("node_", "");
                /*
                model = _doiTacService.Get(int.Parse(parent)).Select(x => new TreeItem
                {
                    id = "node_" + x.Id,
                    text = x.Name,
                    children = _categoryService.getByParent(x.Id).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
                */
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /*
        public ActionResult GetCategoryItem(int id)
        {
            var model = _categoryService.GetCategoryItem(id);
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_categoryService.Delete(id, _authenticationService.GetAuthenticatedUser().UserId))
            {
                return Content("success");
            }
            else
                return Content("error");
        }

        [HttpPost]
        public ActionResult DeleteCategoryItem(int id)
        {
            if(_categoryService.DeleteCategoryItem(id, _authenticationService.GetAuthenticatedUser().UserId))
            {
                return Content("success");
            }
            else
                return Content("error");
        }

        [HttpPost]
        public ActionResult AddCategoryItem(CategoryItem categoryitem)
        {
            if (_categoryService.CreateCategoryItem(categoryitem, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Alert"] = "Thêm mới thành công";
            }
            else
            {
                TempData["Alert"] = "Thêm mới thất bại. Code đã tồn tại";
            }
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult UpdateCategoryItem(CategoryItem categoryitemedit)
        {
            if (_categoryService.UpdateCategoryItem(categoryitemedit, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Alert"] = "Cập nhật thành công";
            }
            else
            {
                TempData["Alert"] = "Cập nhật thất bại. Code đã tồn tại";
            }
            return Redirect("Index");
        }

        public ActionResult GetInfoCategoryItem(int id)
        {
            CategoryItem model  = _categoryService.Get(id);
            return Json(
               new
               {
                   id = model.Id,
                   name = model.Name,
                   code = model.Code,
                   note = model.Note
               }, JsonRequestBehavior.AllowGet
               );
        }*/

        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}
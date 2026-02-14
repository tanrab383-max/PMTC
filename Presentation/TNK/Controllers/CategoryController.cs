using TNK.Core.Domain;
using TNK.Services.Authentication;
using TNK.Services.Catalog;
using System.Web.Mvc;
using System.Linq;
using TNK.Model;
using System.Collections.Generic;

namespace TNK.Controllers
{
    public class CategoryController : BasePublicController
    {
        private ICategoryService _categoryService;
        private IAuthenticationService _authenticationService;
        User CurrentUser;
        public CategoryController(ICategoryService _categoryService
            , IAuthenticationService _authenticationService):base()
        {
            this._categoryService = _categoryService;
            this._authenticationService = _authenticationService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }
        // GET: Category
        public ActionResult Index()
        {
            ViewBag.Alert = TempData["Alert"];
            return View();
        }
        
        public ActionResult UpdateCategory(Category edit)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_categoryService.Update(edit, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Info"] = "Cập nhật thành công";
            }
            else
            {
                TempData["Info"] = "Cập nhật thất bại. Code đã tồn tại";
            }
            return Redirect("Index");
        }

        public ActionResult AddCategory(Category item)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_categoryService.Create(item, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Info"] = "Thêm mới thành công";
            }
            else
            {
                TempData["Info"] = "Thêm mới thất bại. Code đã tồn tại";
            }
            return Redirect("Index");
        }

        public ActionResult Get(int id)
        {
            var model = _categoryService.get(id);
            return Json(
                new
                {
                    id = model.Id,
                    name = model.Name,
                    code = model.Code,
                    note = model.Note
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
                item.text = "Danh mục chung";
                item.children = true;
                item.type = "root";
                item.children = true;
                item.icon = "fa fa-folder";
                model.Add(item);
                //TreeItem item2 = new TreeItem();
                //item2.id = "node_00";
                //item2.text = "Loại xe";
                //item2.children = false;
                //item2.type = "root";
                //item2.children = false;
                //item2.icon = "fa fa-folder";
                //model.Add(item2);
                //TreeItem item3 = new TreeItem();
                //item3.id = "node_000";
                //item3.text = "Đối tác";
                //item3.children = false;
                //item3.type = "root";
                //item3.children = false;
                //item3.icon = "fa fa-folder";
                //model.Add(item3);
            }
            else
            {
                parent = parent.Replace("node_", "");
                model = _categoryService.getByParent(int.Parse(parent)).Select(x => new TreeItem
                {
                    id = "node_" + x.Id,
                    text = x.Name,
                    children = _categoryService.getByParent(x.Id).Count > 0,
                    icon = "fa fa-folder",
                    type = "root"
                }).ToList();
                string a = "";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategoryItem(int id)
        {
            var model = _categoryService.GetCategoryItem(id);
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
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
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_categoryService.DeleteCategoryItem(id, _authenticationService.GetAuthenticatedUser().UserId))
            {
                return Content("success");
            }
            else
                return Content("error");
        }

        [HttpPost]
        public ActionResult AddCategoryItem(CategoryItem categoryitem)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_categoryService.CreateCategoryItem(categoryitem, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Info"] = "Thêm mới thành công";
            }
            else
            {
                TempData["Info"] = "Thêm mới thất bại. Code đã tồn tại";
            }
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult UpdateCategoryItem(CategoryItem categoryitemedit)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_categoryService.UpdateCategoryItem(categoryitemedit, _authenticationService.GetAuthenticatedUser().UserId))
            {
                TempData["Info"] = "Cập nhật thành công";
            }
            else
            {
                TempData["Info"] = "Cập nhật thất bại. Code đã tồn tại";
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
        }

        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
           // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}
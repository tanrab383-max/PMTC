using TNK.Core.Caching;
using TNK.Core.Domain;
using TNK.Services.Authentication;
using TNK.Services.Menus;
using TNK.Services.Roles;
using System.Linq;
using System.Web.Mvc;
using TNK.Models;

namespace TNK.Controllers
{
    public class MenuController : BasePublicController
    {
        IMenuService _menuService;
        IAuthenticationService _authenticationService;
        ICacheManager _cacheManager;
        IRoleService _roleService;
        User CurrentUser;
        public MenuController(
        IMenuService _menuService,
        IAuthenticationService _authenticationService
            , ICacheManager _cacheManager
            , IRoleService _roleService
            ) : base()
        {
            this._menuService = _menuService;
            this._authenticationService = _authenticationService;
            this._cacheManager = _cacheManager;
            this._roleService = _roleService;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
        }

        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexDetail(int id = 0, int tab = 0)
        {
            ViewBag.Tab = tab;
            string cacheKey = CacheKey.keyListMenu;
            var model = _cacheManager.Get(cacheKey, () => _menuService.get());

            if (model != null)
            {
                model = model.Where(x => x.Parent == id).ToList();
            }
            return View(model);
        }

        public ActionResult Create(int id = 0)
        {
            Menu model = new Menu();
            model.Parent = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu model)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            model.Area = "Web";
            if (_menuService.Create(model, _authenticationService.GetAuthenticatedUser().UserId))
            {
                _cacheManager.Remove(CacheKey.keyListMenu);
                return Redirect("~/Menu/Index");
            }
            else
            {
                ViewBag.Alert = "Co lỗi xảy ra";
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var model = _cacheManager.Get<MenuDetailModel>(CacheKey.keyMenuObject + id);
            ViewBag.Menus = _menuService.get();
            if (model == null)
            {
                model = new MenuDetailModel();
                model.Detail = _menuService.get(id);
                model.Types = _menuService.getMenuTypeFromMenuId(id);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Menu model)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_menuService.Update(model, _authenticationService.GetAuthenticatedUser().UserId))
            {
                ClearCache();
                return Redirect("~/Menu/Index");
            }
            else
            {
                ViewBag.Alert("Co lỗi xảy ra");
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_menuService.Delete(id, _authenticationService.GetAuthenticatedUser().UserId))
            {
                var data = _menuService.get(id);
                ClearCache();
                return Redirect("~/Menu/Index");
            }
            else
            {
                ViewBag.Alert("Có lỗi xảy ra");
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            var model = _cacheManager.Get<MenuDetailModel>(CacheKey.keyMenuObject + id);
            if (model == null)
            {
                model = new MenuDetailModel();
                model.Detail = _menuService.get(id);
                model.Types = _menuService.getMenuTypeFromMenuId(id);
            }
            return View(model);
        }

        public ActionResult UpdateActiveMenu(int id, bool isActive)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_menuService.UpdateActiveMenu(id, isActive, _authenticationService.GetAuthenticatedUser().UserId))
            {
                ClearCache();
                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "error" });
            }
        }

        private void ClearCache()
        {
            var obj = _cacheManager.Get(CacheKey.keyListRole, () => _roleService.get());
            foreach (var item in obj)
            {
                _cacheManager.Remove(CacheKey.keyMenuByRole + item.Id);
            }
            _cacheManager.Remove(CacheKey.keyListMenu);
        }

        public ActionResult UpdateActiveMenuType(int id, int typeid, bool isActive)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (_menuService.UpdateActiveMenuType(id, typeid, isActive, _authenticationService.GetAuthenticatedUser().UserId))
            {
                ClearCache();
                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "error" });

            }
        }

        public ActionResult AddAction(int id, string ActionName)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            MenuAction info = new MenuAction();
            info.MenuId = id;
            info.Name = ActionName;
            if (!_menuService.CreateAction(info, _authenticationService.GetAuthenticatedUser().UserId))
            {
                ViewBag.Alert("Có lỗi xảy ra");
            }
            else
            {
                ClearCache();
            }
            return Redirect("~/Menu/Edit/" + id);
        }

        public ActionResult DeleteAction(int mid, int id)
        {
            if (CurrentUser.ReadOnly == true)
            {
                return Redirect("/Home/NoAccess");
            }
            if (!_menuService.DeleteAction(mid, _authenticationService.GetAuthenticatedUser().UserId))
            {
                ViewBag.Alert("Có lỗi xảy ra");
            }
            else
            {
                ClearCache();
            }
            return Redirect("~/Menu/Edit/" + id);
        }
    }
}
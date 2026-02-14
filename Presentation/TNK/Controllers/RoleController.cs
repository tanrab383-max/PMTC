using TNK.Core.Caching;
using TNK.Core.Domain;
using TNK.Services.Authentication;
using TNK.Services.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using TNK.Services.SoChi;

namespace TNK.Controllers
{
    public class RoleController : BasePublicController
    {
        IRoleService _roleService;
        ISoChiService _soChiService;
        IAuthenticationService _authenticationService;
        ICacheManager _cacheManager;
        User CurrentUser;

        public RoleController(IRoleService _roleService
            , IAuthenticationService _authenticationService
            , ISoChiService _soChiService
            , ICacheManager _cacheManager) : base()
        {
            this._roleService = _roleService;
            this._authenticationService = _authenticationService;
            this._soChiService = _soChiService;
            this._cacheManager = _cacheManager;
            CurrentUser = _authenticationService.GetAuthenticatedUser();
            
        }
        // GET: Role
        public ActionResult Index()
        {
            var model = _roleService.get();
            
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Role model)
        {
            if (_roleService.Create(model, (Guid)CurrentUser.UserId))
            {

                return Redirect("Edit/" + model.Id);
            }
            else
            {
                ViewBag.Alert = "Thêm mới thất bại! Có lỗi xảy ra ";
                return View();
            }
        }
        public ActionResult Edit(Guid id)
        {
            _soChiService.CreateRoleInmenu(id);
            var model = _roleService.get(id);
            return View(model);
        }
        public ActionResult EditRoleReport(Guid id)
        {
            var reports = _roleService.ReportLists(id);
            var isCheck = reports.Where(s => s.IsActive == true).Count();
            ViewBag.IsCheckAll = isCheck == reports.Count() ? true : false;
            ViewBag.Reports = reports;
            _soChiService.CreateRoleInmenu(id);
            var model = _roleService.get(id);
            return View(model);
        }
        public ActionResult GetMenuChild(Guid id, int parent, int tab)
        {
            var model = _roleService.getRoleInMenu(id, parent);
            ViewBag.Id = id;
            ViewBag.Tab = tab;
            return View(model);
        }

        public ActionResult GetMenuType(Guid id, int mid = -1)
        {
            var model = _roleService.getRoleInMenuType(id, mid);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Role model)
        {
            if (_roleService.Update(model, (Guid)CurrentUser.UserId))
                return View(model);
            else
            {
                ViewBag.Alert = "Thêm mới thất bại! Có lỗi xảy ra ";
                return View();
            }
        }

        public ActionResult Details(Guid id)
        {
            var model = _roleService.get(id);
            return View(model);
        }
        public ActionResult Delete(Guid id)
        {
            _roleService.Delete(id, (Guid)CurrentUser.UserId);
            var model = _roleService.get();
            return Redirect("~/Role/Index");
        }

        [HttpPost]
        public ActionResult UpdateActiveRoleMenuType(Guid roleId, int typeId, bool isActive)
        {
            if (_roleService.UpdateActiveRoleMenuType(roleId, typeId, isActive, CurrentUser.UserId))
            {
                string cacheKey = CacheKey.keyMenuByRole + roleId;
                _cacheManager.Remove(cacheKey);
                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "error" });
            }
        }

        [HttpPost]
        public ActionResult UpdateActiveRoleMenu(Guid roleId, int menuId, bool isActive)
        {
            if (_roleService.UpdateActiveRoleMenu(roleId, menuId, isActive, CurrentUser.UserId))
            {
                string cacheKey = CacheKey.keyMenuByRole + roleId;
                _cacheManager.Remove(cacheKey);
                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "error" });
            }
        }


        public string GetMenu()
        {
            if (Session["Menu"] != null)
            {
                return Session["Menu"].ToString();
            }
            else
            {
                var html = CreateParentMenu();
                Session.Add("Menu", html);
                return html;
            }
        }

        public string CreateParentMenu()
        {
            var uid = _authenticationService.GetAuthenticatedUser().UserId;
            var roles = _cacheManager.Get(CacheKey.keyRoleByUser + uid, () => _roleService.getRoleFromUser(uid));    
            
            List<Menu> menus = new List<Menu>();
            foreach (var item in roles)
            {

                var obj = _roleService.MenuByRole(item.Id);
                menus.AddRange(obj);
            }
            menus = menus.Distinct().ToList();
            string html = "";
            var data = menus.Where(x => x.Parent == 0);
            foreach (var item in data)
            {
                var child = menus.Where(x => x.Parent == item.Id).ToList();
                var url = "";
                if (!string.IsNullOrEmpty(item.Controller))
                {
                    url = (item.Area == "Web" ? "" : item.Area) + "/" + item.Controller + "/" + item.ActionDefault;
                }
                else
                    url = "javascript:;";
                html += @"<li>";
                // html += "<a " + (url != "javascript:;" ? "target='_blank'" : "") + " href='" + url + "'><i class='fa " + item.Icon + "'></i>" + item.Name + (child.Count() > 0 ? "<span class='fa fa-chevron-down'></span>" : "") + " </a>";
                html += "<a " + (url != "javascript:;" ? "" : "") + " href='" + url + "'><i class='fa " + item.Icon + "'></i>" + item.Name + (child.Count() > 0 ? "<span class='fa fa-chevron-down'></span>" : "") + " </a>";
                if (child.Count() > 0)
                    html += CreateChildMenu(child, menus);
                html += "</li>";
            }
            return html;
        }

        public string CreateChildMenu(List<Menu> objs, List<Menu> menus)
        {
            string html = "";
            html += "<ul class='nav child_menu'>";
            foreach (var item in objs)
            {
                var child = menus.Where(x => x.Parent == item.Id).ToList();
                var url = "";
                if (!string.IsNullOrEmpty(item.Controller))
                {
                    url = (item.Area == "Web" ? "" : item.Area) + "/" + item.Controller + "/" + item.ActionDefault;
                }
                else
                    url = "javascript:;";
                //html += "<li><a  "+(url!= "javascript:;" ? "target='_blank'":"")+" href ='" + url + "'>"+ (string.IsNullOrEmpty( item.Icon)?"": ("<i class='fa " + item.Icon + "'></i>")) + item.Name + (child.Count() > 0 ? "<span class='fa fa-chevron-down'></span>" : "") + " </a>";
                html += "<li><a  " + (url != "javascript:;" ? "" : "") + " href ='" + url + "'>" + (string.IsNullOrEmpty(item.Icon) ? "" : ("<i class='fa " + item.Icon + "'></i>")) + item.Name + (child.Count() > 0 ? "<span class='fa fa-chevron-down'></span>" : "") + " </a>";
                if (child.Count() > 0)
                    html += CreateChildMenu(child, menus);
                html += "</li>";
            }
            html += "</ul>";
            return html;
        }

        public ActionResult MenuRole()
        {
            var uid = _authenticationService.GetAuthenticatedUser().UserId;
            var roles = _cacheManager.Get(CacheKey.keyRoleByUser + uid, () => _roleService.getRoleFromUser(uid));
            List<Menu> menus = new List<Menu>();
            foreach (var item in roles)
            {

                var obj = _roleService.MenuByRole(item.Id);
                menus.AddRange(obj);
            }
            menus = menus.Distinct().ToList();
            return View(menus);
        }

        public ActionResult MenuChildRole(List<Menu> menus, int id)
        {
            ViewBag.Id = id;
            return View(menus);
        }
        [HttpPost]
        public ActionResult UpdateActiveRoleReport(Guid roleId, Guid reportID, bool isActive)
        {
            if (_roleService.UpdateActiveReportRole(roleId, reportID, isActive))
            {
                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "error" });
            }
        }
        [HttpPost]
        public ActionResult UpdateActiveRoleReportLists(List<RoleInReports> reports)
        {
            try
            {
                foreach (var i in reports)
                {
                    _roleService.UpdateActiveReportRole(i.RoleId, i.ReportId, i.IsActive);
                }
                return Json(new { result = "success" });
            }
            catch (Exception)
            {
                return Json(new { result = "error" });
                throw;
            }
        }

    }
}
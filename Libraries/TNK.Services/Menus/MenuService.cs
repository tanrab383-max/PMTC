using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Menus
{
    public class MenuService : IMenuService
    {
        private IRepository<Menu> _menuRepository;
        private IRepository<MenuAction> _menuActionRepository;
        private IRepository<Role> _roleRepository;
        private IRepository<RoleInMenu> _roleInMenuRepository;

        public MenuService(IRepository<Menu> _menuRepository
            , IRepository<MenuAction> _menuActionRepository
            , IRepository<Role> _roleRepository
            , IRepository<RoleInMenu> _roleInMenuRepository)
        {
            this._menuRepository = _menuRepository;
            this._menuActionRepository = _menuActionRepository;
            this._roleRepository = _roleRepository;
            this._roleInMenuRepository = _roleInMenuRepository;
        }
        public List<MenuRole> get()
        {
            var query = from a in _menuRepository.Table
                        where a.IsDeleted == false
                        select new MenuRole
                        {
                            Area = a.Area
                            ,
                            Controller = a.Controller
                            ,
                            IsActive = a.IsActive
                            ,
                            IsDeleted = a.IsDeleted
                            ,
                            CreatedBy = a.CreatedBy
                            ,
                            CreatedDate = a.CreatedDate
                            ,
                            Icon = a.Icon
                            ,
                            Id = a.Id
                            ,
                            Name = a.Name
                            ,
                            ActionDefault = a.ActionDefault
                            ,
                            Parent = a.Parent
                            ,
                            UpdatedBy = a.UpdatedBy
                            ,
                            UpdatedDate = a.UpdatedDate
                            ,
                            OrderBy = a.OrderBy
                            ,
                            total = (from b in _menuRepository.Table where a.Id == b.Parent select b).Count()
                        }
                        ;
            return query.OrderBy(x => x.OrderBy).ToList();

        }

        public bool Create(Menu obj, Guid id)
        {
            try
            {
                DateTime d = DateTime.Now;
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.CreatedBy = id;
                obj.CreatedDate = d;
                obj.UpdatedBy = id;
                obj.UpdatedDate = d;
                _menuRepository.Insert(obj);
                var actions = _menuActionRepository.Table.Where(x => x.MenuId == 0).ToList().Select(x => new MenuAction
                {
                    UpdatedBy = id,
                    CreatedBy = id,
                    UpdatedDate = d,
                    CreatedDate = d,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    MenuId = obj.Id,
                    Name = x.Name
                }).ToList();
                _menuActionRepository.Insert(actions);

                var roleinmenu = _roleRepository.Table.Where(x => x.IsDeleted == false).ToList().Select(x => new RoleInMenu
                {
                    MenuId = obj.Id,
                    CreatedBy = id,
                    CreatedDate = d,
                    UpdatedBy = id,
                    UpdatedDate = d,
                    IsActive = false,
                    RoleId = x.Id
                });
                _roleInMenuRepository.Insert(roleinmenu);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int objId, Guid id)
        {
            try
            {
                var obj = _menuRepository.Table.FirstOrDefault(x => x.Id == objId);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.UpdatedBy = id;
                    obj.UpdatedDate = DateTime.Now;
                    _menuRepository.Update(obj);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool Update(Menu obj, Guid id)
        {
            try
            {
                var data = _menuRepository.Table.FirstOrDefault(x => x.Id == obj.Id);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    data.Name = obj.Name;
                    data.Area = obj.Area;
                    data.Icon = obj.Icon;
                    data.Parent = obj.Parent;
                    data.OrderBy = obj.OrderBy;
                    data.ActionDefault = obj.ActionDefault;
                    data.Controller = obj.Controller;
                    data.UpdatedBy = id;
                    data.UpdatedDate = DateTime.Now;
                    _menuRepository.Update(obj);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public Menu get(int id)
        {
            return _menuRepository.Table.FirstOrDefault(x => x.Id == id);

        }


        public List<MenuAction> getMenuTypeFromMenuId(int menuid)
        {
            return _menuActionRepository.Table.Where(x => x.MenuId == menuid && x.IsDeleted == false).ToList();
        }

        public bool UpdateActiveMenu(int id, bool isActive, Guid user)
        {
            try
            {
                var data = _menuRepository.Table.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    data.IsActive = isActive;
                    data.UpdatedBy = user;
                    data.UpdatedDate = DateTime.Now;
                    _menuRepository.Update(data);
                    return true;
                }
            }
            catch
            {

                return false;
            }
        }

        public bool UpdateActiveMenuType(int id, int typeid, bool isActive, Guid user)
        {
            try
            {
                var data = _menuActionRepository.Table.FirstOrDefault(x => x.MenuId == id && x.Id == typeid);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    data.IsActive = isActive;
                    data.UpdatedBy = user;
                    data.UpdatedDate = DateTime.Now;
                    _menuActionRepository.Update(data);
                    return true;
                }
            }
            catch
            {

                return false;
            }
        }

        public bool CreateAction(MenuAction data, Guid id)
        {
            try
            {
                DateTime d = DateTime.Now;
                data.IsDeleted = false;
                data.IsActive = true;
                data.CreatedBy = id;
                data.CreatedDate = d;
                data.UpdatedBy = id;
                data.UpdatedDate = d;
                _menuActionRepository.Insert(data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAction(int id, Guid user)
        {
            try
            {
                var data = _menuActionRepository.Table.First(x => x.Id == id);

                DateTime d = DateTime.Now;
                data.IsDeleted = true;
                data.UpdatedBy = user;
                data.UpdatedDate = d;
                _menuActionRepository.Update(data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

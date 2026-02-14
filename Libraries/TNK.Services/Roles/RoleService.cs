using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using TNK.Data;
using System.Data;
using TNK.Core.Caching;

namespace TNK.Services.Roles
{
    public class RoleService : IRoleService
    {
        private IRepository<Role> _roleRepository;
        private IRepository<UserInRole> _userInRoleRepository;
        private IRepository<RoleInMenu> _roleInMenuRepository;
        private IRepository<RoleInMenuAction> _roleInMenuActionRepository;
        private IRepository<ViewMenuAction> _ViewAllRoleInMenuActionRepository;
        private IRepository<Menu> _menuRepository;
        private IRepository<MenuAction> _menuActionRepository;
        private ICacheManager _cacheManager;
        IDbContext _dbContext;
        public RoleService(IRepository<Role> _roleRepository
            , IRepository<UserInRole> _userInRoleRepository
            , IRepository<RoleInMenu> _roleInMenuRepository
            , IRepository<RoleInMenuAction> _roleInMenuActionRepository
            , IRepository<Menu> _menuRepository
            , IRepository<MenuAction> _menuActionRepository
            , IRepository<ViewMenuAction> _ViewAllRoleInMenuActionRepository
            , IDbContext _dbContext
            , ICacheManager _cacheManager
            
            )//, ICacheManager _cacheManager)
        {
            this._roleRepository = _roleRepository;
            this._userInRoleRepository = _userInRoleRepository;
            this._roleInMenuRepository = _roleInMenuRepository;
            this._roleInMenuActionRepository = _roleInMenuActionRepository;
            this._menuRepository = _menuRepository;
            this._menuActionRepository = _menuActionRepository;
            this._ViewAllRoleInMenuActionRepository = _ViewAllRoleInMenuActionRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
        }

        public List<Role> get()
        {
            return _roleRepository.Table.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
        }

        public bool Create(Role role, Guid id)
        {
            try
            {
                role.Id = Guid.NewGuid();
                role.IsDeleted = false;
                role.IsActive = true;
                role.CreatedBy = id;
                role.CreatedDate = DateTime.Now;
                role.UpdatedBy = id;
                role.UpdatedDate = DateTime.Now;
                _roleRepository.Insert(role);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Delete(Guid roleid, Guid id)
        {
            try
            {
                var obj = _roleRepository.GetById(roleid);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.UpdatedBy = id;
                    obj.UpdatedDate = DateTime.Now;
                    _roleRepository.Update(obj);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(Role role, Guid id)
        {
            try
            {
                var obj = _roleRepository.GetById(role.Id);//id
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    obj.Name = role.Name;
                    obj.UpdatedBy = id;
                    obj.UpdatedDate = DateTime.Now;
                    _roleRepository.Update(obj);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Role get(Guid id)
        {
            return _roleRepository.GetById(id);
        }

        public List<RoleInMenus> getRoleInMenu(Guid id, int parent)
        {
            var query = from a in _menuRepository.Table
                        join b in _roleInMenuRepository.Table on a.Id equals b.MenuId into ps
                        from p in ps.DefaultIfEmpty()
                        where a.IsDeleted == false
                        && a.IsActive == true
                        && a.Parent == parent
                        && p.RoleId == id
                        select new RoleInMenus
                        {
                            id = a.Id
                            ,
                            Name = a.Name
                            ,
                            mid = a.Id
                            ,
                            Active = (from c in _roleInMenuRepository.Table
                                      where c.RoleId == id && a.Id == c.MenuId && c.IsActive == true
                                      select c).Count() >= 1 ? true : false
                        };
            return query.ToList();
        }
        public List<RoleInMenuType> getRoleInMenuType(Guid id, int mid)
        {
            var query = from a in _menuActionRepository.Table
                        join b in _menuRepository.Table on a.MenuId equals b.Id
                        where a.IsActive == true && a.IsDeleted == false && b.IsDeleted == false && b.IsActive == true
                        && a.MenuId == mid
                        select new RoleInMenuType
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Active = (from c in _roleInMenuActionRepository.Table
                                      where c.MenuActionId == a.Id && c.IsActive == true && c.RoleId == id
                                      select c).Count() >= 1 ? true : false
                        };
            return query.ToList();
        }

        
        public List<ViewMenuAction> getAllRoleMenuAction()
        {
            var query = from a in _ViewAllRoleInMenuActionRepository.Table
                        select a
                        ;                                               
            return query.ToList();
        }


        public List<RoleMenu> getRoleMenu(Guid id)
        {
            var query = from a in _roleRepository.Table
                        join b in _roleInMenuRepository.Table on a.Id equals b.RoleId
                        join c in _menuRepository.Table on b.MenuId equals c.Id
                        join d in _menuActionRepository.Table on c.Id equals d.MenuId
                        join e in _roleInMenuActionRepository.Table on a.Id equals e.RoleId
                        where e.MenuActionId == d.Id && a.Id == id
                        && a.IsActive == true && a.IsDeleted == false
                        && b.IsActive == true
                        && c.IsActive == true && c.IsDeleted == false
                        && d.IsActive == true && d.IsDeleted == false
                        && e.IsActive == true
                        select new RoleMenu
                        {
                            MenuName = c.Name,
                            Area = c.Area,
                            Controller = c.Controller,
                            Action = d.Name
                        };
            return query.ToList();
        }

        public List<RoleUser> getRoleUser(Guid user)
        {
            var query = from a in _roleRepository.Table
                        select new RoleUser
                        {
                            Id = a.Id
                            ,
                            Name = a.Name
                            ,
                            Active = (from b in _userInRoleRepository.Table
                                      where b.RoleId == a.Id && b.IsDeleted == false && b.IsActive == true && b.UserId == user
                                      select b).Count() >= 1 ? true : false
                        };
            return query.ToList();
        }

        public List<Role> getRoleFromUser(Guid id)
        {
            var list = from a in _userInRoleRepository.Table
                       join b in _roleRepository.Table on a.RoleId equals b.Id
                       where a.IsActive == true && a.IsDeleted == false && b.IsDeleted == false && b.IsActive == true && a.UserId == id
                       select b;
            return list.ToList();
        }
        public bool UpdateActiveRoleMenu(Guid roleId, int menuid, bool isActive, Guid user)
        {
            try
            {
                var data = _roleInMenuRepository.Table.FirstOrDefault(x => x.MenuId == menuid && x.RoleId == roleId);
                if (data == null)
                {
                    RoleInMenu obj = new RoleInMenu();
                    obj.RoleId = roleId;
                    obj.MenuId = menuid;
                    obj.IsActive = isActive;
                    obj.CreatedBy = user;
                    obj.CreatedDate = DateTime.Now;
                    obj.UpdatedBy = user;
                    obj.UpdatedDate = DateTime.Now;
                    _roleInMenuRepository.Insert(obj);
                }
                else
                {
                    data.IsActive = isActive;
                    data.UpdatedBy = user;
                    data.UpdatedDate = DateTime.Now;
                    _roleInMenuRepository.Update(data);
                }
                _cacheManager.Remove(CacheKey.keyMenuByRole);
                _cacheManager.Remove(CacheKey.keyRoleByUser);
                _cacheManager.Remove(CacheKey.keyRoleInMenuAction);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool UpdateActiveRoleMenuType(Guid roleId, int typeid, bool isActive, Guid user)
        {
            try
            {
                var data = _roleInMenuActionRepository.Table.FirstOrDefault(x => x.MenuActionId == typeid && x.RoleId == roleId);
                if (data == null)
                {
                    RoleInMenuAction obj = new RoleInMenuAction();
                    obj.RoleId = roleId;
                    obj.MenuActionId = typeid;
                    obj.IsActive = isActive;
                    obj.CreatedBy = user;
                    obj.CreatedDate = DateTime.Now;
                    obj.UpdatedBy = user;
                    obj.UpdatedDate = DateTime.Now;
                    _roleInMenuActionRepository.Insert(obj);
                }
                else
                {
                    data.IsActive = isActive;
                    data.UpdatedBy = user;
                    data.UpdatedDate = DateTime.Now;
                    _roleInMenuActionRepository.Update(data);
                }
                _cacheManager.Remove(CacheKey.keyMenuByRole);
                _cacheManager.Remove(CacheKey.keyRoleByUser);
                _cacheManager.Remove(CacheKey.keyRoleInMenuAction);
                return true;
            }
            catch
            {

                return false;
            }
        }

        public List<Menu> MenuByRole(Guid id)
        {
            var query = from m in _menuRepository.Table
                        join rm in _roleInMenuRepository.Table on m.Id equals rm.MenuId
                        where rm.RoleId == id
                        && rm.IsActive == true
                        && m.IsActive == true
                        && m.IsDeleted == false
                        select m;
            return query.OrderBy(x => x.OrderBy).ToList();
        }
        public bool UpdateRoleUser(bool check, Guid id, Guid roleid, Guid userid)
        {
            try
            {
                var obj = _userInRoleRepository.Table.FirstOrDefault(x => x.RoleId == roleid && x.UserId == id);
                if (obj == null)
                {
                    UserInRole data = new UserInRole();
                    data.IsActive = check;
                    data.RoleId = roleid;
                    data.UserId = id;
                    data.CreatedBy = userid;
                    data.CreatedDate = DateTime.Now;
                    data.UpdatedBy = userid;
                    data.UpdatedDate = DateTime.Now;
                    _userInRoleRepository.Insert(data);

                }
                else
                {
                    obj.IsActive = check;
                    obj.UpdatedBy = userid;
                    obj.UpdatedDate = DateTime.Now;
                    _userInRoleRepository.Update(obj);
                }
                _cacheManager.Remove(CacheKey.keyMenuByRole);
                _cacheManager.Remove(CacheKey.keyRoleByUser);
                _cacheManager.Remove(CacheKey.keyRoleInMenuAction);
                _cacheManager.Remove(CacheKey.keyListMenu);
                return true;
            }
            catch
            {

                return false;
            }

        }
        public bool CheckUserInRole (Guid UserId, string RoleName)
        {
            try
            {
                Role r = _roleRepository.Table.FirstOrDefault(x => x.Name == RoleName && x.IsDeleted == false);
                if (r == null)
                    return false;
                var usr = _userInRoleRepository.Table.SingleOrDefault(x => x.UserId == UserId && x.RoleId == r.Id && x.IsDeleted == false);
                if (usr == null)
                    return false;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public List<ReportList> ReportLists(Guid roleId)
        {
            try
            {
                SqlParameter roleID = new SqlParameter("roleID", roleId);
                return _dbContext.ExecuteStoredProcedureList<ReportList>("sp_GetEditReportsByRole", roleID).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdateActiveReportRole(Guid roleId, Guid reportId, bool isActive)
        {
            try
            {
                SqlParameter roleID = new SqlParameter("roleID", roleId);
                SqlParameter reportID = new SqlParameter("reportID", reportId);
                SqlParameter IsActive = new SqlParameter("isActive", isActive);
                _dbContext.ExecuteStoredProcedure("sp_UpdateRoleReport", roleID, reportID, IsActive);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

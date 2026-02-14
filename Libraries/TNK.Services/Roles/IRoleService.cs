using TNK.Core.Domain;
using TNK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TNK.Core.Domain.View;
namespace TNK.Services.Roles
{
    public interface IRoleService
    {
        List<Role> get();

        bool Create(Role role,Guid id);
        bool Update(Role role, Guid id);
        Role get(Guid id);
        bool Delete(Guid roleid, Guid id);
        List<RoleInMenus> getRoleInMenu(Guid id, int parent);
        List<RoleInMenuType> getRoleInMenuType(Guid id, int mid);
        List<RoleUser> getRoleUser(Guid user);
        List<Role> getRoleFromUser(Guid id);
        List<RoleMenu> getRoleMenu(Guid id);
        /// <summary>
        /// Lay danh sach toan bo role in menu action de kiem tra xem action nay co can phai check role khong. 
        /// Neu khong co dinh nghia trong RoleInMenuAction thi khong can xet phan quyen
        /// </summary>
        /// <returns></returns>
        List<ViewMenuAction> getAllRoleMenuAction();
        bool UpdateActiveRoleMenuType(Guid roleId, int typeid, bool isActive, Guid user);
        bool UpdateActiveRoleMenu(Guid roleId, int menuid, bool isActive, Guid user);
        List<Menu> MenuByRole(Guid id);
        bool UpdateRoleUser(bool check, Guid id, Guid roleid, Guid userid);
        //Sử dụng cho update nợ HHGP
        bool CheckUserInRole(Guid UserId, string RoleName);
        List<ReportList> ReportLists(Guid roleID);
        bool UpdateActiveReportRole(Guid roleID, Guid reportID, bool isActive);

    }
}

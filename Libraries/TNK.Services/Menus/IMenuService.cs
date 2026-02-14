using TNK.Core.Domain;
using TNK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Menus
{
    public interface IMenuService
    {
        //List<getListMenuFromParentId_Result> get();
        bool Create(Menu obj, Guid id);
        bool Delete(int objId, Guid id);
        bool Update(Menu obj, Guid id);
        Menu get(int id);
        List<MenuRole> get();
        //List<getListMenuFromParentId_Result> getMenuFromParentId(int id);
        List<MenuAction> getMenuTypeFromMenuId(int menuid);
        bool UpdateActiveMenu(int id, bool isActive, Guid user);
        bool UpdateActiveMenuType(int id, int typeid, bool isActive, Guid user);
        bool CreateAction(MenuAction data, Guid id);
        bool DeleteAction(int id, Guid user);
        //List<getListMenu_Result> getListMenu();

    }
}

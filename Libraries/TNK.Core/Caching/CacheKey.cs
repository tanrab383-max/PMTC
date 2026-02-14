using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Caching
{
    public static class CacheKey
    {
        public const int cacheTime = 86400000;
        public static class Category
        {
            public const string keyCategory = "keyCategory_";
            public const string keyCategoryParent = "keyCategoryParent_";

            public const string keyCategoryItem = "keyCategoryItem_";
            public const string keyCategoryItemParent = "keyCategoryItemParent_";

        }

        public static class TuiDinhKhoan
        {
            public const string keyTuiDinhKhoan_tien = "keyTuiDinhKhoan_";
            public const string keyTuiDinhKhoan_vay = "keyTuiDinhKhoan_";
            public const string keyTuiDinhKhoan_taisan = "keyTuiDinhKhoan_";
            public const string keyTuiDinhKhoan_nguonvon = "keyTuiDinhKhoan_";
        }


        public const string keyUserName = "keyUserName_";
        public const string keyUserId = "keyUserId_";
        public const string keyUser = "keyUser_";




        public const string keyListMenu = "keyListMenu_";
        public const string keyMenuByRole = "keyMenuByRole_";
        public const string keyMenuObject = "keyMenuObject_";
        public const string keyRoleUser = "keyRoleUser_";
        public const string keyRoleInMenuAction = "keyRoleInMenuAction_";



        public const string keyRole = "keyRole_";
        public const string keyListRole = "keyListRole_";
        public const string keyRoleByUser = "keyRoleByUser_";

        public const string keyPTDVT = "keyPTDVT_";

        public const string keyExcelTotal = "keyExcelTotal_";
    }
}

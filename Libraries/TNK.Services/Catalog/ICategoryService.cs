using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Catalog
{
    public interface ICategoryService
    {
        List<Category> get();
        bool Update(Category obj, Guid id);
        bool Create(Category obj, Guid id);
        bool Delete(int objId, Guid id);
        List<Category> getByParent(int id);
        Category get(int id);
        void UpdateCacheList();
        void UpdateCacheItem(int id);
        void UpdateCacheParent(int id);

        CategoryItem GetCategoryItemByCode(string code);
        List<CategoryItem> GetCategoryItem(int id);
        List<CategoryItem> GetCategoryItem(string code);
        bool DeleteCategoryItem(int catid, Guid id);
        bool UpdateCategoryItem(CategoryItem obj, Guid id);
        bool CreateCategoryItem(CategoryItem obj, Guid id);
        void ClearCacheCategoryList();
        void ClearCacheCategoryItem(CategoryItem id);
        CategoryItem Get(int id);
    }
}

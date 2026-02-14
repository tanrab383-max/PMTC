using TNK.Core;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using TNK.Services.LichSuThaoTac;
using TNK.Services.Authentication;

namespace TNK.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        IRepository<Category> _categoryRepository;
        IRepository<CategoryItem> _categoryItemRepository;
        ICacheManager _cacheManager;
        ILichSuThaoTacService _lichSuThaoTacService;
        IAuthenticationService _authenticationService;
        string _ipClient = "";
        string _hostNameClient = "";
        public CategoryService(IRepository<Category> _categoryRepository
            , ICacheManager _cacheManager
            , ILichSuThaoTacService _lichSuThaoTacService
            , IAuthenticationService _authenticationService
            , IRepository<CategoryItem> _categoryItemRepository)
        {
            this._categoryRepository = _categoryRepository;
            this._cacheManager = _cacheManager;
            this._lichSuThaoTacService = _lichSuThaoTacService;
            this._authenticationService = _authenticationService;
            this._categoryItemRepository = _categoryItemRepository;
        }
        public void SetIPClient(string ip)
        {
            this._ipClient = ip;
        }
        public void SetHostNameClient(string host)
        {
            this._hostNameClient = host;
        }
        public List<Category> get()
        {
            var list = _cacheManager.Get(CacheKey.Category.keyCategory, () => _categoryRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false).ToList());
            return list;
        }

        public Category get(int id)
        {
            var obj = _cacheManager.Get(CacheKey.Category.keyCategory + id, () => get().FirstOrDefault(x => x.Id == id));
            return obj;
        }


        public List<Category> getByParent(int id)
        {
            var list = _cacheManager.Get(CacheKey.Category.keyCategoryParent + id, () => get().Where(x => x.Parent == id).ToList());
            return list;
        }

        public void UpdateCacheList()
        {
            var list = _categoryRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            _cacheManager.Remove(CacheKey.Category.keyCategory);
            _cacheManager.Set(CacheKey.Category.keyCategory, list, CacheKey.cacheTime);
        }

        public void UpdateCacheItem(int id)
        {
            var obj = _categoryRepository.Table.FirstOrDefault(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
            if (obj != null)
            {
                _cacheManager.Remove(CacheKey.Category.keyCategory + id);
                _cacheManager.Set(CacheKey.Category.keyCategory + id, obj, CacheKey.cacheTime);
                UpdateCacheList();
            }
        }

        public void UpdateCacheParent(int id)
        {
            _cacheManager.Remove(CacheKey.Category.keyCategoryParent + id);
            getByParent(id);
        }

        public bool Create(Category obj, Guid id)
        {
            try
            {
                var data = get().FirstOrDefault(x => x.Code == obj.Code);
                if (data != null)
                    return false;
                DateTime d = DateTime.Now;
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.CreatedBy = id;
                obj.CreatedDate = d;
                obj.UpdatedBy = id;
                obj.UpdatedDate = d;
                _categoryRepository.Insert(obj);
                UpdateCacheItem(obj.Id);
                UpdateCacheParent(obj.Parent);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int objId, Guid id)
        {
            try
            {
                var obj = _categoryRepository.Table.FirstOrDefault(x => x.Id == objId);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.UpdatedBy = id;
                    obj.UpdatedDate = DateTime.Now;
                    _categoryRepository.Update(obj);
                    UpdateCacheList();
                    UpdateCacheParent(obj.Parent);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Category obj, Guid id)
        {
            try
            {
                var data = _categoryRepository.Table.FirstOrDefault(x => x.Id == obj.Id);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    var i = get().FirstOrDefault(x => x.Code == obj.Code);
                    if (i != null && i.Id != obj.Id)
                    {
                        return false;
                    }
                    data.Code = obj.Code;
                    data.Name = obj.Name;
                    data.Note = obj.Note;
                    data.Parent = obj.Parent;
                    data.UpdatedBy = id;
                    data.UpdatedDate = DateTime.Now;
                    _categoryRepository.Update(obj);
                    UpdateCacheItem(obj.Id);
                    UpdateCacheParent(obj.Parent);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public CategoryItem GetCategoryItemByCode(string code)
        {
            return GetCategoryItem().FirstOrDefault(x => x.Code == code);
        }

        public List<CategoryItem> GetCategoryItem(int id)
        {
            return _cacheManager.Get(CacheKey.Category.keyCategoryItemParent + id, () => _categoryItemRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false && x.Parent == id).ToList());
        }

        public List<CategoryItem> GetCategoryItem(string code)
        {
            var obj = _categoryRepository.Table.FirstOrDefault(x => x.Code == code && x.IsActive == true && x.IsDeleted == false);
            if (obj != null)
            {
                return _categoryItemRepository.Table.Where(x => x.Parent == obj.Id && x.IsDeleted == false && x.IsActive == true).ToList();
            }
            return new List<CategoryItem>();
        }

        public List<CategoryItem> GetCategoryItem()
        {
            return _cacheManager.Get(CacheKey.Category.keyCategoryItem, () => _categoryItemRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false).ToList());
        }


        public void ClearCacheCategoryItem(CategoryItem obj)
        {
            ClearCacheCategoryList();
            _cacheManager.Remove(CacheKey.Category.keyCategoryItem + obj.Id);
            Get(obj.Id);
            _cacheManager.Remove(CacheKey.Category.keyCategoryItemParent + obj.Parent);
            GetCategoryItem(obj.Parent);
        }

        public void ClearCacheCategoryList()
        {
            _cacheManager.Remove(CacheKey.Category.keyCategoryItem);
            GetCategoryItem();
        }

        public bool CreateCategoryItem(CategoryItem obj, Guid id)
        {
            try
            {

                //kiem tra neu trung ca Code va trung ca Category thì mới không cho thêm
                var data = GetCategoryItem().FirstOrDefault(x => x.Code == obj.Code && x.Parent == obj.Parent && x.IsDeleted == false);
                var cat = _categoryRepository.Table.FirstOrDefault(x => x.Id == obj.Parent && x.IsDeleted == false);
                if (data != null)
                    return false;
                cat.LastCode++;
                string code = "0000000000" + cat.LastCode.ToString();
                int catlen = cat.Code.Length;
                int len = 10 - catlen - 1;
                code = code.Substring(code.Length - len, len);
                code = cat.Code + "-" + code;
                _categoryRepository.Update(cat);
                DateTime d = DateTime.Now;
                if (obj.Code == "")
                    obj.Code = code;
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.CreatedBy = id;
                obj.CreatedDate = d;
                obj.UpdatedBy = id;
                obj.UpdatedDate = d;
                _categoryItemRepository.Insert(obj);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, obj.Code, obj.Name, "CREATE", "CreateCategoryItem", "Mã category :" + obj.Code + " tên  :" + obj.Name, _authenticationService.GetAuthenticatedUser().UserId);
                ClearCacheCategoryItem(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateCategoryItem(CategoryItem obj, Guid id)
        {
            try
            {
                var data = _categoryItemRepository.Table.FirstOrDefault(x => x.Id == obj.Id);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    var i = GetCategoryItem().FirstOrDefault(x => x.Code == obj.Code);
                    if (i != null && i.Id != obj.Id)
                    {
                        return false;
                    }
                    data.Code = obj.Code;
                    data.Name = obj.Name;
                    data.Note = obj.Note;
                    data.UpdatedBy = id;
                    data.UpdatedDate = DateTime.Now;
                    _categoryItemRepository.Update(obj);
                    _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, data.Code, data.Name, "UPDATE", "UpdateCategoryItem", "Mã category :" + data.Code + " tên  :" + data.Name, _authenticationService.GetAuthenticatedUser().UserId);
                    ClearCacheCategoryItem(data);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategoryItem(int catid, Guid id)
        {
            try
            {
                var data = _categoryItemRepository.Table.FirstOrDefault(x => x.Id == catid);
                data.IsDeleted = true;
                data.UpdatedBy = id;
                data.UpdatedDate = DateTime.Now;
                _categoryItemRepository.Update(data);
                _lichSuThaoTacService.WriteLichSuThaoTac(DateTime.Now.ToString("yyMMddhhmmss"), _ipClient, _hostNameClient, data.Code, data.Name, "DELETE", "DeleteCategoryItem", "Mã category :" + data.Code + " tên  :" + data.Name, _authenticationService.GetAuthenticatedUser().UserId);
                ClearCacheCategoryItem(data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CategoryItem Get(int id)
        {
            return _cacheManager.Get(CacheKey.Category.keyCategoryItem + id, () => GetCategoryItem().FirstOrDefault(x => x.Id == id));
        }
    }
}

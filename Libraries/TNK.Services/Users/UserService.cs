using TNK.Core.Data;
using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using TNK.Core.Caching;
using System.Data;
using TNK.Services.Log;
using System.Data.SqlClient;
using TNK.Data;
using TNK.Services.Users;

namespace TNK.Services.Users
{
    public class UserService : IUserervice
    {
        private IRepository<Core.Domain.User> _userRepository;
        private ICacheManager _cacheManager;
        //ILogger _log;
        IDbContext _dbContext;
        public UserService(IRepository<Core.Domain.User> _userRepository, ICacheManager _cacheManager,  IDbContext _dbContext)
        {
            this._userRepository = _userRepository;
            this._cacheManager = _cacheManager;
            this._dbContext = _dbContext;
        }

        public TNK.Core.Domain.User get(string username,string key)
        {
           // _cacheManager.Remove(CacheKey.keyUser + username);
            return _cacheManager.Get(CacheKey.keyUser + key, CacheKey.cacheTime, () => _userRepository.Table.First(x => x.UserName == username));
        }
        public TNK.Core.Domain.User getByUserName(string username)
        {
            return _userRepository.Table.Where(x=>x.UserName == username).FirstOrDefault();
        }
        public DataTable getData()
        {
            try
            {
                SqlParameter spFrom = new SqlParameter("fromDate", null);
                SqlParameter spTo = new SqlParameter("toDate", null);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_DashBoard_Summary", spFrom, spTo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable getData(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFrom = new SqlParameter("fromDate", FromDate);
                SqlParameter spTo = new SqlParameter("toDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_DashBoard_Summary", spFrom, spTo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TNK.Core.Domain.User getByID(int id)
        {
            return _cacheManager.Get(CacheKey.keyUserId + id, CacheKey.cacheTime, () => _userRepository.GetById(id));
        }
        /// <summary>
        /// Get all user 
        /// </summary>
        /// <returns></returns>
        public List<Core.Domain.User> get()
        {
            _cacheManager.Remove("keyUser_");
            return _cacheManager.Get(CacheKey.keyUser, CacheKey.cacheTime, () => _userRepository.Table.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
        }

        public bool Delete(int id, Guid userId)
        {
            try
            {
                var user = getByID(id);
                user.IsDeleted = true;
                user.UpdatedBy = userId;
                user.UpdatedDate = DateTime.Now;
                _userRepository.Update(user);
                _cacheManager.Remove(CacheKey.keyUser);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(Core.Domain.User user, Guid userId)
        {
            try
            {
                var obj = _userRepository.GetById(user.Id);
                if (obj != null)
                {
                    obj.UserName = user.UserName;
                    obj.UpdatedBy = userId;
                    obj.UpdatedDate = DateTime.Now;
                    _userRepository.Update(user);
                    _cacheManager.Remove(CacheKey.keyUser);
                    _cacheManager.Remove(CacheKey.keyUser + obj.UserName);
                    _cacheManager.Remove(CacheKey.keyUserId + obj.UserId);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public User getByUserId(Guid UserId)
        {
            return _userRepository.Table.Where(x => x.UserId == UserId && x.IsDeleted == false).FirstOrDefault();
        }
    }
}

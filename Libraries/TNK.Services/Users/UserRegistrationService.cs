using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Model.Authentication;
using TNK.Services.Authentication;
using TNK.Services.Roles;
using TNK.Services.Security;
using System;
using System.Linq;
using System.Net.Http;
using System.Data.SqlClient;
using TNK.Data;
using TNK.Core;

namespace TNK.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private IAuthenticationService _authenticationService;
        private IRepository<Core.Domain.User> _userRepository;
        private IEncryptionService _encryptService;
        private IRoleService _roleService;
        private ICacheManager _cacheManager;
        IDbContext _dbContext;
        string _ipClient = "";
        string _hostNameClient = "";

        //phan ghi log login
        public void SetIPClient(string ip)
        {
            this._ipClient = ip;
        }
        public void SetHostNameClient(string host)
        {
            this._hostNameClient = host;
        }
        public UserRegistrationService(IAuthenticationService _authenticationService, IRepository<Core.Domain.User> _userRepository
            , IEncryptionService _encryptService, IRoleService _iRoleService, ICacheManager _iCacheManager, IDbContext _dbContext
            
            )
        {
            this._authenticationService = _authenticationService;
            this._userRepository = _userRepository;
            this._encryptService = _encryptService;
            this._roleService = _iRoleService;
            this._cacheManager = _iCacheManager;
            this._dbContext = _dbContext;
        }

        public bool CreateUser(Core.Domain.User info )
        {
            bool flag = false;
            try
            {
                var userId = getCurrentUser().UserId;
                info.PasswordFormatId = 1;
                string saltKey = _encryptService.CreateSaltKey(5);
                info.PasswordSalt = saltKey;
                info.Password = _encryptService.CreatePasswordHash(info.Password, saltKey, "SHA1");
                info.IsActive = true;
                info.IsDeleted = false;
                info.CreatedBy = userId;
                info.CreatedDate = DateTime.Now;
                info.UpdatedBy = userId;
                info.UpdatedDate = DateTime.Now;
                info.UserId = Guid.NewGuid();
                _userRepository.Insert(info);
                _cacheManager.Remove(CacheKey.keyUser);
            }
            catch(Exception ex)
            {
                flag = true;
            }
            return flag;
        }

        public bool ValidateUser(string username,string password, string storeId)
        {
            bool flag = false;
            string pwd,status= "";
            var storeService = new StoreService();
            string connectionString = storeService.GetConnectionStringForStore(storeId);
            using (var context = new TNKObjectContext(connectionString))
            {
                IRepository<User> userRepository = new EfRepository<User>(context);
                var user = userRepository.Table.FirstOrDefault(x => x.UserName == username && x.IsDeleted == false);
                if (user != null)
                {
                    pwd = _encryptService.CreatePasswordHash(password, user.PasswordSalt, "SHA1");
                    if (user.Password == pwd)
                    {
                        flag = true;
                        _authenticationService.SignIn(user, true, storeId);
                    }
                }
                if (flag == true)
                {
                    status = "Thành công";
                }
                else
                {
                    status = "Thất bại";
                }
                SqlParameter IPClient = new SqlParameter("IPClient", _ipClient);
                SqlParameter HostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
                SqlParameter Status = new SqlParameter("Status", status);
                SqlParameter Active = new SqlParameter("Active", "Login");
                SqlParameter UserId = new SqlParameter("UserId", user.UserId);
                SqlParameter Note = new SqlParameter("Note", "");
                _dbContext.ExecuteStoredProcedure("sp_LichSuLogin_Insert", IPClient, HostNameClient, Status, Active, UserId, Note);
                return flag;
            }
        }

        public User GetUser(string username, string storeId)
        {          
            var storeService = new StoreService();
            string connectionString = storeService.GetConnectionStringForStore(storeId);
            using (var context = new TNKObjectContext(connectionString))
            {
                IRepository<User> userRepository = new EfRepository<User>(context);
                var user = userRepository.Table.FirstOrDefault(x => x.UserName == username && x.IsDeleted == false);               
                return user;
            }
        }
        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordnews"></param>
        /// <returns></returns>
        public string ResetPassWord(int userId)
        {
            try
            {
                string passwordnews = "Pmtc@123456#";
                string pwd;
                var user = _userRepository.Table.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    pwd = _encryptService.CreatePasswordHash(passwordnews, user.PasswordSalt, "SHA1");
                    user.Password = pwd;
                    _userRepository.Update(user);
                }
                return passwordnews;
            }
            catch (Exception ex)
            {
                return "Lỗi không thay đổi được password";
            }
        }


        public bool ChangePassWord(string username,string password,string passwordnews)
        {
            bool flag = false;
            string pwd;
            var user = _userRepository.Table.FirstOrDefault(x => x.UserName == username);
            if (user != null)
            {
                pwd = _encryptService.CreatePasswordHash(password, user.PasswordSalt, "SHA1");
                if (user.Password == pwd)
                {
                    flag = true;
                    pwd = _encryptService.CreatePasswordHash(passwordnews, user.PasswordSalt, "SHA1");
                    user.Password = pwd;
                    _userRepository.Update(user);
                }
            }
            return flag;
        }

        public TNK.Core.Domain.User getCurrentUser()
        {
            return _authenticationService.GetAuthenticatedUser();
        }

        public bool CheckRole(AuthenticationModel model)
        {
            var user = getCurrentUser();
            
            var cacheKey = CacheKey.keyRoleUser + user.UserId ;
            var obj = _cacheManager.Get(cacheKey,()=> _roleService.getRoleFromUser(user.UserId));
         
            foreach(var item in obj)
            {
                var cacheKeyMenu = CacheKey.keyMenuByRole + item.Id;
                var menu = _cacheManager.Get(cacheKeyMenu, () => _roleService.getRoleMenu(item.Id));
                //lay danh sach roleInMenuAction
                var allRoleInMenu = _cacheManager.Get(CacheKey.keyRoleInMenuAction, () => _roleService.getAllRoleMenuAction()).Where(x=>x.Controller == model.Controller && x.Action == model.Action).ToList();
                //kiem tra neu controller va action nay khong co khai bao trong roleInMenuAction thi xem nhu co quyen
                //neu roleInMenuAction co khai bao controller va action nay thi kiem tiep xem co duoc phan quyen khong
                if (allRoleInMenu.Count() == 0)
                    return true;
                var per = menu.Where(x => x.Action == model.Action /*&& x.Area == model.Area*/ && x.Controller == model.Controller).ToList();
                if (per.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void Insert_Logout()
        {
            var user = getCurrentUser();
            SqlParameter IPClient = new SqlParameter("IPClient", _ipClient);
            SqlParameter HostNameClient = new SqlParameter("HostNameClient", _hostNameClient);
            SqlParameter Status = new SqlParameter("Status", "Thành công");
            SqlParameter Active = new SqlParameter("Active", "Logout");
            SqlParameter UserId = new SqlParameter("UserId", user.UserId);
            SqlParameter Note = new SqlParameter("Note", "");
            _dbContext.ExecuteStoredProcedure("sp_LichSuLogin_Insert", IPClient, HostNameClient, Status, Active, UserId, Note);
        }

    }
}

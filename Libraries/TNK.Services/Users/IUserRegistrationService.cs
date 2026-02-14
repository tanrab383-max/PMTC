using TNK.Core.Domain;
using TNK.Data;
using TNK.Model.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Users
{
    public interface IUserRegistrationService
    {
        bool ValidateUser(string username, string password,string storeId);
        TNK.Core.Domain.User getCurrentUser();
        bool CreateUser(Core.Domain.User info);
        bool CheckRole(AuthenticationModel model);
        bool ChangePassWord(string username, string password, string passwordnews);
        string ResetPassWord(int userId);
        void SetIPClient(string ip);
        void SetHostNameClient(string host);
        void Insert_Logout();
        User GetUser(string username, string storeId);

    }
}

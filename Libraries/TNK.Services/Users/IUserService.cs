using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using TNK.Core.Domain.View;

namespace TNK.Services.Users
{
    public interface IUserervice
    {
        TNK.Core.Domain.User get(string username, string key);
        List<Core.Domain.User> get();
        TNK.Core.Domain.User getByID(int id);
        bool Update(Core.Domain.User user, Guid userId);
        bool Delete(int id,Guid userId);
        TNK.Core.Domain.User getByUserName(string username);
        DataTable getData();
        DataTable getData(DateTime FromDate,DateTime ToDate);
        User getByUserId(Guid UserId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Model
{
    public class LoginModel
    {
        public List<Login> GetLogging { get; set; }
        public Login Item { get; set; }
        public List<User> Users { get; set; }
    }
}

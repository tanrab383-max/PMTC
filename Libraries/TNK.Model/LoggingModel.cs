using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Model
{
    public class LoggingModel
    {
        public List<Logging> GetLogging { get; set; }
        public Logging Item { get; set; }
        public List<User> Users { get; set; }
    }
}

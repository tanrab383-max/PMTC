using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class DataHistory :BaseEntity
    {
        public Guid Id { get; set; }
        public string SessionId { get; set; }
        public Guid ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string TableName { get; set; }
        public string MaPhieu { get; set; }
        public string XmlValue { get; set; }
        public string Action { get; set; }
        public string UserChang { get; set; }
    }
}

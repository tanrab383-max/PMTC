using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class Config:BaseEntity
    {
        public string ConfigCode { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigDescritption { get; set; }
        public Guid? UniqId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ConfigType { get; set; }
    }
}

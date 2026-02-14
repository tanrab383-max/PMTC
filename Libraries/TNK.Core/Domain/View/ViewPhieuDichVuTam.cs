using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewPhieuDichVuTam:BaseEntity
    {
        public string REPAIRORDERNO { get; set; }
        public string REGISTERNO { get; set; }
        public string CAROWNERNAME { get; set; }
        public string CAROWNERTEL { get; set; }
        public string FRAMENO { get; set; }
        public string CF_TYPE { get; set; }
        public DateTime RO_DATE { get; set; }
    }
}

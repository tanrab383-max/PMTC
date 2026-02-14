using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewCCOMX : BaseEntity
    {
        public string MaPhieuChi { get; set; }
        public DateTime NgayChi { get; set; }
        public string DoiTac { get; set; }
        public float SoTienChi { get; set; }
        public string GhiChu { get; set; }
    }
}

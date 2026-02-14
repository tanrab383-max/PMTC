using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ChiTietPhieuDichVuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietPhieuDichVu>
    {
        public ChiTietPhieuDichVuMapping()
        {
            this.ToTable("ChiTietPhieuDichVu");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ChiTietNhapKhoPhuTungMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietNhapKhoPhuTung>
    {
        public ChiTietNhapKhoPhuTungMapping()
        {
            this.ToTable("ChiTietNhapKhoPhuTung");
        }
    }
}

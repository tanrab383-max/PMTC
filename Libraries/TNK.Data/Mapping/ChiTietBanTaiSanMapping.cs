using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ChiTietBanTaiSanMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietBanTaiSan>
    {
        public ChiTietBanTaiSanMapping()
        {
            this.ToTable("ChiTietBanTaiSan");
        }
    }
}

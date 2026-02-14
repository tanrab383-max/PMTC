using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class PhieuThuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.PhieuThu>
    {
        public PhieuThuMapping()
        {
            this.ToTable("PhieuThu");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}

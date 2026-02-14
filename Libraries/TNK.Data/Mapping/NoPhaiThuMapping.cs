using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class NoPhaiThuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.NoPhaiThu>
    {
        public NoPhaiThuMapping()
        {
            this.ToTable("NoPhaiThu");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuNo);
        }
    }
}

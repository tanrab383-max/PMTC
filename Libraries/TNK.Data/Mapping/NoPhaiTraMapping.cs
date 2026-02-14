using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class NoPhaiTraMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.NoPhaiTra>
    {
        public NoPhaiTraMapping()
        {
            this.ToTable("NoPhaiTra");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuNo);
        }
    }
}

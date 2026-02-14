using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class DotGopVonMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.DotGopVon>
    {
        public DotGopVonMapping()
        {
            this.ToTable("DotGopVon");
            this.HasKey(x => x.MaKhoiTao);
            this.Ignore(x => x.Id);
        }
    }
}

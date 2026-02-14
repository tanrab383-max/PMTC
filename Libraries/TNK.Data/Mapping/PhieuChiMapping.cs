using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class PhieuChiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.PhieuChi>
    {
        public PhieuChiMapping()
        {
            this.ToTable("PhieuChi");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

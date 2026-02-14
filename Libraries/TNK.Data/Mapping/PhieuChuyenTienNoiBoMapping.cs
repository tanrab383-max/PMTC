using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class PhieuChuyenTienNoiBoMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.PhieuChuyenTienNoiBo>
    {
        public PhieuChuyenTienNoiBoMapping()
        {
            this.ToTable("PhieuChuyenTienNoiBo");
            this.HasKey(x => x.MaPhieu);
            this.Ignore(x => x.Id);
        }
    }
}

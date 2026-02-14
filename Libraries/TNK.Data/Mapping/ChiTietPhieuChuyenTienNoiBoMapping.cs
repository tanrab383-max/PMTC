using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ChiTietPhieuChuyenTienNoiBoMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietPhieuChuyenTienNoiBo>
    {
        public ChiTietPhieuChuyenTienNoiBoMapping()
        {
            this.ToTable("ChiTietPhieuChuyenTienNoiBo");
            this.Ignore(x => x.TempChi);
            this.Ignore(x => x.TempThu);
        }
    }
}

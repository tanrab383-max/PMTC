using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewPhieuThuiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewPhieuThu>
    {
        public ViewPhieuThuiMapping()
        {
            this.ToTable("ViewPhieuThu");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}

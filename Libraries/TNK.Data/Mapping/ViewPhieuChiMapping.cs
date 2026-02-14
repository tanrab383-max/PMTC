using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewPhieuChiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewPhieuChi>
    {
        public ViewPhieuChiMapping()
        {
            this.ToTable("ViewPhieuChi");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

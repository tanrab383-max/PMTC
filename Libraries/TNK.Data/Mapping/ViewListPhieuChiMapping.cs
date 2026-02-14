using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public class ViewListPhieuChiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewListPhieuChi>
    {
        public ViewListPhieuChiMapping()
        {
            this.ToTable("ViewListPhieuChi");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

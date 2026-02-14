using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewCTAUNMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewCTAUN>
    {
        public ViewCTAUNMapping()
        {
            this.ToTable("ViewCTAUN");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

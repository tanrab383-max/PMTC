using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewCHCNOMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewCHCNO>
    {
        public ViewCHCNOMapping()
        {
            this.ToTable("ViewCHCNO");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

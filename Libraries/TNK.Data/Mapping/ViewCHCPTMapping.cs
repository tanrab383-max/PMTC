using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewCHCPTMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewCHCPT>
    {
        public ViewCHCPTMapping()
        {
            this.ToTable("ViewCHCPT");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

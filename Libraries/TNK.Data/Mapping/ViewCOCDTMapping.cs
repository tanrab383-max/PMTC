using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewCOCDTMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewCOCDT>
    {
        public ViewCOCDTMapping()
        {
            this.ToTable("ViewCOCDT");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

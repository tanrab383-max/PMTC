using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTDIVUMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTDIVU>
    {
        public ViewTDIVUMapping()
        {
            this.ToTable("ViewTDIVU");
            this.Ignore(x => x.Id);
            this.HasKey(u => u.MaPhieuThu);
        }
    }
}

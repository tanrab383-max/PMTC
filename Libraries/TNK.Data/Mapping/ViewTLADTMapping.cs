using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTLADTMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTLADT>
    {
        public ViewTLADTMapping()
        {
            this.ToTable("ViewTLADT");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}

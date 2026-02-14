using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTCOCMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTCOC>
    {
        public ViewTCOCMapping()
        {
            this.ToTable("ViewTCOC");
            this.Ignore(x => x.Id);
            //this.Ignore(x => x.ConLai);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}

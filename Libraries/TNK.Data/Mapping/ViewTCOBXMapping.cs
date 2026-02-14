using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTCOBXMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTCOBX>
    {
        public ViewTCOBXMapping()
        {
            this.ToTable("ViewTCOBX");
            this.Ignore(x => x.Id);
            //this.Ignore(x => x.ConLai);
            this.HasKey(x => x.MaPhieuThu);
        }
    }
}

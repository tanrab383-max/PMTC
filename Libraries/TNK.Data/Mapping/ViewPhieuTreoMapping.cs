using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewPhieuTreoMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewPhieuTreo>
    {
        public ViewPhieuTreoMapping()
        {
            this.ToTable("ViewPhieuTreo");
        }
    }
}

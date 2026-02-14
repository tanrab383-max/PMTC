using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewPhieuDichVuTamMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewPhieuDichVuTam>
    {
        public ViewPhieuDichVuTamMapping()
        {
            this.ToTable("ViewPhieuDichVuTam");
        }
    }
}

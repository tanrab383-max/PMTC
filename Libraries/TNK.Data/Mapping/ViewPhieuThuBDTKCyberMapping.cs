using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain.View;

namespace TNK.Data.Mapping
{
    public partial class ViewPhieuThuBDTKCyberMapping : TNKEntityTypeConfiguration<ViewPhieuBDTKTamCybers>
    {
        public ViewPhieuThuBDTKCyberMapping()
        {
            this.ToTable("ViewPhieuBDTKTamCybers");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.SoChungTu);
        }
    }
}

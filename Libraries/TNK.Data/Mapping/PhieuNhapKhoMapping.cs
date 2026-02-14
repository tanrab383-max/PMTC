using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class PhieuNhapKhoMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.PhieuNhapKho>
    {
        public PhieuNhapKhoMapping()
        {
            this.ToTable("PhieuNhapKho");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPN);
        }
    }
}

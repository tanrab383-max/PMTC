using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class PhieuQuyetToanMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.PhieuQuyetToan>
    {
        public PhieuQuyetToanMapping()
        {
            this.ToTable("PhieuQuyetToan");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieu);
        }
    }
}

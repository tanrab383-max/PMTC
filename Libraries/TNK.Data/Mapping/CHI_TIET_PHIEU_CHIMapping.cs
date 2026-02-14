using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Data.Mapping
{
    public partial class CHI_TIET_PHIEU_CHIMapping : TNKEntityTypeConfiguration<CHI_TIET_PHIEU_CHI>
    {
        public CHI_TIET_PHIEU_CHIMapping()
        {
            this.ToTable("CHI_TIET_PHIEU_CHI");
        }
    }
}

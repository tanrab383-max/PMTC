using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;
namespace TNK.Data.Mapping
{
    public partial class ChiTietBanPhuKienMapping : TNKEntityTypeConfiguration<ChiTietBanPhuKien>
    {
        public ChiTietBanPhuKienMapping()
        {
            this.ToTable("ChiTietBanPhuKien");
            this.HasKey(x => x.Ma);
            this.Ignore(x => x.Id);
        }
    }
}

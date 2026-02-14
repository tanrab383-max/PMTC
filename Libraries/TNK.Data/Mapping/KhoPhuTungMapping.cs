using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class KhoPhuTungMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.KhoPhuTung>
    {
        public KhoPhuTungMapping()
        {
            this.ToTable("KhoPhuTung");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaKho);
        }
    }
}

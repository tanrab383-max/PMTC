using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Data.Mapping
{
    public partial class DieuChinhKhoPhuTungMapping : TNKEntityTypeConfiguration<DieuChinhKhoPhuTung>
    {
        public DieuChinhKhoPhuTungMapping()
        {
            this.ToTable("DieuChinhKhoPhuTung");
            this.HasKey(x => x.ID);
            this.Ignore(x => x.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Data.Mapping
{
    public class ChiTietBaoDuongTietKiemMapping : TNKEntityTypeConfiguration<ChiTietBaoDuongTietKiem>
    {
        public ChiTietBaoDuongTietKiemMapping()
        {
            this.ToTable("ChiTietBaoDuongTietKiem");
            this.HasKey(x => x.Ma);
            this.Ignore(x => x.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class NhanVienMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.NhanVien>
    {
        public NhanVienMapping()
        {
            this.ToTable("NhanVien");
            this.HasKey(x => x.MaNhanVien);
            this.Ignore(x => x.Id);
        }
    }
}

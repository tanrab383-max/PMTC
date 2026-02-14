using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewChiTietXuatNhapKhoPhuTungMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewChiTietXuatNhapKhoPhuTung>
    {
        public ViewChiTietXuatNhapKhoPhuTungMapping()
        {
            this.ToTable("ViewChiTietXuatNhapKhoPhuTung");
             
        }
    }
}

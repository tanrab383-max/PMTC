using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewTaiKhoanTien_NganHangMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewTaiKhoanTien_NganHang>
    {
        public ViewTaiKhoanTien_NganHangMapping()
        {
            this.ToTable("ViewTaiKhoanTien_NganHang");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaNganHang);
        }
    }
}

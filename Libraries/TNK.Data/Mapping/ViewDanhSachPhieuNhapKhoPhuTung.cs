using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewDanhSachPhieuNhapKhoPhuTungMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewDanhSachPhieuNhapKhoPhuTung>
    {
        public ViewDanhSachPhieuNhapKhoPhuTungMapping()
        {
            this.ToTable("ViewDanhSachPhieuNhapKhoPhuTung");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MaPhieuChi);
        }
    }
}

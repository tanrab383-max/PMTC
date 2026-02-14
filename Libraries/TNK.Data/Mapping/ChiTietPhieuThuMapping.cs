using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class ChiTietPhieuThuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietPhieuThu>
    {
        public ChiTietPhieuThuMapping()
        {
            this.ToTable("CHI_TIET_PHIEU_THU");
            this.Ignore(x => x.Temp);
        }
    }
}

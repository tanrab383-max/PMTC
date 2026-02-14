using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class ChiTietPhieuChiMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietPhieuChi>
    {
        public ChiTietPhieuChiMapping()
        {
            this.ToTable("ChiTietPhieuChi");
            this.Ignore(x => x.Temp);
        }
    }
}

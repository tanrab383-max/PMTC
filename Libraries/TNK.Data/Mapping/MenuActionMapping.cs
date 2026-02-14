using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class MenuActionMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.MenuAction>
    {
        public MenuActionMapping()
        {
            this.ToTable("MenuAction");

        }
    }
}

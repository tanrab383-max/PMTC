using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class RoleInMenuActionMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.RoleInMenuAction>
    {
        public RoleInMenuActionMapping()
        {
            this.ToTable("RoleInMenuAction");
        }
    }
}

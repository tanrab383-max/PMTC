using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class RoleInMenuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.RoleInMenu>
    {
        public RoleInMenuMapping()
        {
            this.ToTable("RoleInMenu");
        }
    }
}

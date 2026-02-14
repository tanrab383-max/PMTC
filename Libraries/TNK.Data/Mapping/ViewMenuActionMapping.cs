using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewMenuActionMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewMenuAction>
    {
        public ViewMenuActionMapping()
        {
            this.ToTable("ViewMenuAction");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.MenuActionId);
        }
    }
}

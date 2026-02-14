using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewGetListLichSuThaoTacMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewGetListLichSuThaoTac>
    {
        public ViewGetListLichSuThaoTacMapping()
        {
            this.ToTable("ViewGetListLichSuThaoTac");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.ID);
        }
    }
}

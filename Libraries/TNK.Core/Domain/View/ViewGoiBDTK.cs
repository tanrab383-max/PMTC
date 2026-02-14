using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
	[Serializable]
	public partial class ViewGoiBDTK : BaseEntity
	{
		[Display(Name = "ma")]
		[Required]
		public string Ma { get; set; }
		[Display(Name = "tenchuongtrinh")]
		public string TenChuongTrinh { get; set; }
		[Display(Name = "giaban")]
		public double GiaBan { get; set; }
		[Display(Name = "giatrisudung")]
		public double GiaTriSuDung { get; set; }

	}
}

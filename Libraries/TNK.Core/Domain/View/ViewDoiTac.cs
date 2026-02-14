using System;
using System.ComponentModel.DataAnnotations;

namespace TNK.Core.Domain.View
{
    [Serializable]
    public partial class ViewDoiTac : BaseEntity
    {
        [Display(Name ="madoitac")]
		[Required]
		public string madoitac { get; set; }
		[Display(Name ="maviettat")]
		public string maviettat { get; set; }
		[Display(Name ="TenDoiTac")]
		public string TenDoiTac { get; set; }
		[Display(Name ="GhiChu")]
		public string GhiChu { get; set; }
		[Display(Name ="TyLeHoaHong")]
		public double TyLeHoaHong { get; set; }
		[Display(Name ="LoaiDoiTac")]
		public string LoaiDoiTac { get; set; }
		[Display(Name ="TenLoaiDoiTac")]
		public string TenLoaiDoiTac { get; set; }
        public double? HanMucVay { get; set; }
		[Display(Name = "DoiTacNoiBo")]
		public string DoiTacNoiBo { get; set; }


	}
}

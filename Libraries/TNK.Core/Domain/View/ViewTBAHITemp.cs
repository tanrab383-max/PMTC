using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTBAHITemp : BaseEntity
    {

        public string SO_HOP_DONG { get; set; }
        public DateTime NGAY_KY_HOP_DONG { get; set; }
        public string ChungTu { get; set; }
        public string TEN_KHACH_HANG { get; set; }
        public string SO_KHUNG { get; set; }
        public decimal ThucBan { get; set; }
        public decimal HoaHong { get; set; }
        public decimal TienTraBaoHiem { get; set; }
        public decimal GiaBan { get; set; }
        public decimal GiamGia { get; set; }
        public string TaiTuc { get; set; }
        public string Type { get; set; }
        public string MaPhieuThu { get; set; }
        public int? Ma_NCC_Bao_Hiem { get; set; }
        public string TenDoiTac { get; set; }
        public string MaDoiTac { get; set; }
        public decimal ThanhToan { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace TNK.Core.Domain.View
{ 
    public class ViewPhieuThuCyber : BaseEntity
    {
        public string SttRec { get; set; }
        public string SoChungTu {  get; set; }
        public DateTime NgayThu {  get; set; }
        public string HoTen { get; set; }
        public string Dien_Thoai{ get; set; }
        public string Dia_Chi { get; set; }
        public string CTBH { get; set; }
        public string SohopDong { get; set; }
        public string So_Khung { get; set; }
        public string BienSo { get; set; }
        public double TienTraBH { get; set; }
        public double Hoahong { get; set; }
        public double ThucBan { get; set; }
        public double GiaVon { get; set; }
        public double GiaBan { get; set; }
        public double GiamGia { get; set; }
        public string MaTaiKhoan { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public string ThongTinKhac { get; set; }
        public string DoiTac { get; set; }
        public double TongCong { get; set; }
        public double ConLai { get; set; }
        public string LoaiXe { get; set; }
        public string MaXe { get; set; }
        public string MauXe { get; set; }
        public string ThanhToan { get; set; }
        public string Ten_LoaiThu { get; set; }
        public string Ma_LoaiThu { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewKhoXe:BaseEntity
    {
        public new Guid Id { get; set; }
        public string SoKhung{get;set;}
        public string VIN{get;set;}
        public string SoMay{get;set;}
        public string SoHoaDon { get; set; }
        public string SoTMSS { get; set; }
        public string MaPhieuNhap{get;set;}
        public DateTime NgayNhapKho { get; set; }
        public DateTime? NgayXuatKho { get; set; }
        public string MaLoaiXe{get;set;}
        public string MaPhieuXuat{get;set;}
        public string NhaCungCap{get;set;}
        public double GiaVon{get;set;}
        public double GiaNiemYet{get;set;}
        public double? GiaBan{get;set;}
        public string TinhTrang{get;set;}
        public string Loai{get;set;}
        public string GhiChu{get;set;}
        public bool IsDeleted{get;set;}
        public Guid? CreatedBy{get;set;}
        public DateTime? CreatedDate{get;set;}
        public Guid? UpdatedBy{get;set;}
        public DateTime? UpdatedDate{get;set;}
        public string TenModel{get;set;}
        public string TenDoiXe{get;set;}
        public string TenGrad{get;set;}
        public string TenMau{get;set;}
        public string CodeModel { get; set; }
        public string CodeDoiXe { get; set; }
        public string CodeGrad { get; set; }
        public string CodeMau { get; set; }
        public string Hang { get; set; }
        public int Model { get; set; }
        public int MaMau { get; set; }
        public string PhanLoaiNhap { get; set; }
        public string TheChap { get; set; }
        public DateTime? NgayTheChap { get; set; }
        public DateTime? NgayHetTheChap { get; set; }
    }
}

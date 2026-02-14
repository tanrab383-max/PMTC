using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Services.GopVon
{
    public interface IGopVonService
    {
        string DGVon_Insert(TNK.Core.Domain.DotGopVon item, List<TNK.Core.Domain.ChiTietGopVon> listCTGV);
        DataTable ListPTGopVon(DateTime from, DateTime to, string query,string dvgv, int p, ref int total, int pageSize);
        DataTable ListPTGopVon(DateTime from, DateTime to, string doitac, string query,string Loai);
        List<DotGopVon> ListDotGopVon(DateTime from, DateTime to, string query, int p, ref int total, int pageSize);
        List<ChiTietGopVon> XemCTGV(string MaKhoiTao);
        DotGopVon XemDotGopVon(string MaKhoiTao);
        string DeleteGopVon(string maKhoiTao);
        string DeletePTGopVon(string maKhPhieuThu);
        string DeletePCRutVon(string MaPhieuChi);
        List<DotGopVon> ListDotGopVon();
        List<DotGopVon> ListDotRutVon();
        ChiTietGopVon Item_CTGV(Guid Id);
        string DotGVCuoi();
        DataTable ListDotGopVon(DateTime From,DateTime To,string LoaiGV);
        double CountDGV();
        List<ChiTietGopVon> ListCTGV();
        List<ChiTietGopVon> ListCTRV();
        List<DotGopVon> ListDGV();
        DataTable ListTongHop();
        double TongTienGop(string id);

        double TongTienRut(string id);
        string DeleteDotGopVon(string maKhoiTao, Guid user);
    }
}

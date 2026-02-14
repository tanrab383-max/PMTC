using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Log;
using TNK.Services.Manager;
namespace TNK.Services.GopVon
{
    public class GopVonService :IGopVonService
    {
        IAuthenticationService _authenticationService;
        IRepository<DotGopVon> _dotGopVonRepository;
        IRepository<ChiTietGopVon> _chiTietGopVonRepository;
        IRepository<PhieuThu> _phieuThuRepository;
        IRepository<PhieuChi> _phieuChiRepository;
        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        ISoKetChuyenService _soKetChuyenService;
        public GopVonService(IAuthenticationService _authenticationService,
                               IRepository<TNK.Core.Domain.DotGopVon> _dotGopVonRepository,
                               IRepository<Core.Domain.ChiTietGopVon> _chiTietGopVonRepository,
                               IRepository<PhieuThu> _phieuThuRepository, 
                               IRepository<PhieuChi> _phieuChiRepository,
                               IDbContext _dbContext,
                               ICacheManager _cacheManager,
                               ILogger _log,
                               ISoKetChuyenService _soKetChuyenService
                               )
        {
            this._authenticationService = _authenticationService;
            this._dotGopVonRepository = _dotGopVonRepository;
            this._chiTietGopVonRepository = _chiTietGopVonRepository;
            this._phieuThuRepository = _phieuThuRepository;
            this._dbContext = _dbContext;
            this._cacheManager = _cacheManager;
            this._log = _log;
            this._phieuChiRepository = _phieuChiRepository;
            this._soKetChuyenService = _soKetChuyenService;
        }
        //phan chinh : get, insert, update & delete
        #region
        public string DGVon_Insert(TNK.Core.Domain.DotGopVon DGV, List<TNK.Core.Domain.ChiTietGopVon> LCTGV)
        {
            try
            {
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(DGV.NgayGopVon).Days >= 0)
                {
                    return "Không tạo được đợt góp vốn rơi vào khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";
                }

                DateTime Day = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                string timeKhoiTao = Convert.ToString(Day.ToString("dd/MM/yyyy HH:mm:ss"));
                string  maKhoiTao = timeKhoiTao.Substring(8, 2) + timeKhoiTao.Substring(3, 2) + timeKhoiTao.Substring(0, 2) + timeKhoiTao.Substring(11, 2) + timeKhoiTao.Substring(14, 2) + timeKhoiTao.Substring(17, 2);
                DGV.MaKhoiTao = maKhoiTao;
                DGV.CreatedBy = uid;
                DGV.CreatedDate = Day;
                DGV.IsActive = true;
                DGV.IsDeleted = false;
                DGV.UpdatedBy = uid;
                DGV.UpdatedDate = Day;
                
                //DGV.LoaiGopVon = "";
                //DGV.LoaiGopVon = "R";
                _dotGopVonRepository.Insert(DGV);
                double tyle = 0;
                double TongTien = 0;
                
                if (LCTGV != null)
                {
                    foreach (var item in LCTGV)
                    {
                        TongTien += item.SoTien;
                    }
                    TongTien += TongGopVon();
                    for (var i = 0; i < LCTGV.Count; i++)
                    {
                        LCTGV[i].Id = Guid.NewGuid();
                        LCTGV[i].CreatedBy = uid;
                        LCTGV[i].CreatedDate = Day;
                        LCTGV[i].IsActive = true;
                        //LCTGV[i].IsDeleted = false;
                        LCTGV[i].MaKhoiTao = maKhoiTao;
                        LCTGV[i].SoTienConLai = 0;
                        //CTGV[i].SoTienThucTe = 0;
                        LCTGV[i].UpdatedBy = uid;
                        LCTGV[i].UpdatedDate = Day;
                        //doiTac = LCTGV[i].MaDoiTac;
                        if(LCTGV[i].LoaiPhieu == "R")
                        {
                            if (LCTGV[i].SoTienDotTruoc == 0)
                            {
                                LCTGV[i].SoTienHienTai = LCTGV[i].SoTien;
                            }
                            else
                            {
                                LCTGV[i].SoTienHienTai = TongTienDoiTac(LCTGV[i].MaDoiTac) + LCTGV[i].SoTien;
                            }
                            double tongTienRut = 0;
                            List<ChiTietGopVon> lst = _chiTietGopVonRepository.Table.Where(x => x.IsDeleted == false && x.LoaiPhieu == "R").ToList();
                            if(lst.Count > 0)
                            {
                                tongTienRut = _chiTietGopVonRepository.Table.Where(x => x.IsDeleted == false && x.LoaiPhieu == "R").Sum(x => x.SoTien);
                            }
                            if(tongTienRut > 0)
                            {
                                tyle = (double)LCTGV[i].SoTienHienTai / tongTienRut;
                                LCTGV[i].TyLe = Math.Round(tyle, 4) * 100;
                            }
                            else
                            {
                                tyle = 1;
                                LCTGV[i].TyLe = Math.Round(tyle, 4) * 100;
                            }
                        }
                        else
                        {
                            if (LCTGV[i].SoTienDotTruoc == 0)
                            {
                                LCTGV[i].SoTienHienTai = LCTGV[i].SoTien;
                            }
                            else
                            {
                                LCTGV[i].SoTienHienTai = TongTienDoiTac(LCTGV[i].MaDoiTac) + LCTGV[i].SoTien;
                            }
                            tyle = (double)LCTGV[i].SoTienHienTai / TongTien;
                            LCTGV[i].TyLe = Math.Round(tyle, 4) * 100;
                        }
                        _chiTietGopVonRepository.Insert(LCTGV[i]);
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                _log.WriteLog("GopVonService.DGVon_Insert: " + ex.Message.ToString());
                return ex.Message;
            }
        }

        double TongGopVon()
        {
            var list = _chiTietGopVonRepository.Table.Where(x => x.IsDeleted == false).ToList();
            if(list.Count > 0)
            {
               double Total = _chiTietGopVonRepository.Table.Where(x => x.IsDeleted == false).Sum(x => x.SoTien);
                return Total;
            }
            else
            {
                return 0;
            }
        }
        
        #endregion
        //phan danh sach phieu gop von
        public DataTable ListPTGopVon(DateTime from, DateTime to, string query, string dvgv, int p, ref int total, int pageSize)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("fromDate", from);
                SqlParameter toDate = new SqlParameter("toDate", to);
                SqlParameter Query = new SqlParameter("query", query);
                SqlParameter maDoiTac = new SqlParameter("maDoiTac", dvgv);
                DataTable dt = _dbContext.ExecuteStoredProcedureDataTable("sp_ListPhieuThuGopVon", fromDate, toDate, Query,maDoiTac);
                return dt;
            }
            catch(Exception ex)
            {
                _log.WriteLog("GopVonService.ListPTGopVon:" + ex.Message.ToString());
                return null;
            }
        }

        public DataTable ListPTGopVon(DateTime from, DateTime to, string doitac, string query,string Loai)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("fromDate", from);
                SqlParameter toDate = new SqlParameter("toDate", to);
                SqlParameter Query = new SqlParameter("query", query);
                SqlParameter maDoiTac = new SqlParameter("maDoiTac", doitac);
                SqlParameter LoaiPhieu = new SqlParameter("LoaiPhieu", Loai);
                DataTable dt = _dbContext.ExecuteStoredProcedureDataTable("sp_ListPhieuThuGopVon", fromDate, toDate, Query, maDoiTac,LoaiPhieu);
                return dt;
            }
            catch (Exception ex)
            {
                _log.WriteLog("GopVonService.ListPTGopVon:" + ex.Message.ToString());
                return null;
            }
        }
        // danh sach dot gop von
        public List<DotGopVon> ListDotGopVon(DateTime from, DateTime to, string query, int p, ref int total, int pageSize)
        {
            try
            {
                var list = _dotGopVonRepository.Table.Where(x => x.NgayGopVon >= from && x.NgayGopVon <= to && x.IsDeleted == false).OrderByDescending(x=>x.NgayGopVon).ToList();
                total = list.Count;
                return list.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("GopVonService.ListDotGopVon:" + ex.Message.ToString());
                return null;
            }
        }
        public DataTable ListDotGopVon(DateTime From, DateTime To,string LoaiGopVon)
        {
            try
            {
                SqlParameter spfromDate = new SqlParameter("fromDate", From);
                SqlParameter sptoDate = new SqlParameter("toDate", To);
                SqlParameter spLoaiGV = new SqlParameter("loaigopvon", LoaiGopVon);
                DataTable dt = _dbContext.ExecuteStoredProcedureDataTable("sp_ListDotGopVon", spfromDate, sptoDate, spLoaiGV);
                return dt;
            }
            catch (Exception ex)
            {
                _log.WriteLog("GopVonService.ListDotGopVon:" + ex.Message.ToString());
                return null;
            }
        }
        // Xem chi tiết và xóa chi tiết góp vốn và đợt góp vốn
        public List<ChiTietGopVon> XemCTGV(string MaKhoiTao)
        {
            try
            {
                if(MaKhoiTao != "")
                {
                    return _chiTietGopVonRepository.Table.Where(x => x.MaKhoiTao == MaKhoiTao && x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ToList();
                }
                return null;
            }
            catch(Exception ex)
            {
                _log.WriteLog("GopVonService.XemCTGV:" + ex.Message.ToString());
                return null;
            }
        }
        public DotGopVon XemDotGopVon(string MaKhoiTao)
        {
            try
            {
                if(MaKhoiTao != "")
                {
                    return _dotGopVonRepository.Table.Where(x => x.MaKhoiTao == MaKhoiTao && x.IsDeleted == false).FirstOrDefault();
                }
                return null;
            }
            catch(Exception ex)
            {
                _log.WriteLog("GopVonService.XemDotGopVon:" + ex.Message.ToString());
                return null;
            }
        }
        public string DeleteGopVon(string maKhoiTao)
        {
            try
            {
                
                var obj = _dotGopVonRepository.Table.FirstOrDefault(x => x.MaKhoiTao == maKhoiTao && x.IsDeleted == false);
                if (obj == null)
                    return "Không tìm thấy đợt góp vốn hoặc đã bị xóa trước đó";
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedDate = DateTime.Now;
                obj.UpdatedBy = uid;
                obj.IsDeleted = true;
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayGopVon).Days >= 0)
                {
                    return "Không xóa được đợt góp vốn rơi vào khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";
                }
                _dotGopVonRepository.Update(obj);
                List<ChiTietGopVon> item = _chiTietGopVonRepository.Table.Where(x=>x.MaKhoiTao == maKhoiTao && x.IsDeleted == false).ToList();
                if(item != null)
                {
                    var uid1 = _authenticationService.GetAuthenticatedUser().UserId;
                    for (int i = 0; i< item.Count;i++)
                    {
                        item[i].UpdatedDate = DateTime.Now;
                        item[i].UpdatedBy = uid1;
                        item[i].IsDeleted = true;
                        _chiTietGopVonRepository.Update(item[i]);
                    }
                }
                return "";
            }
            catch(Exception ex)
            {
                _log.WriteLog("GopVonService.DeleteGopVon:" + ex.Message.ToString());
                return ex.Message;
            }
        }
        public string DeleteDotGopVon(string maKhoiTao,Guid user)
        {
            try
            {
                SqlParameter pMaKhoiTaoDotGopVon = new SqlParameter("maKhoiTaoDotGopVon", maKhoiTao);
                SqlParameter pUser = new SqlParameter("user", user);
                SqlParameter pMessage = new SqlParameter("message", "");
                pMessage.Size = 1000;
                pMessage.Direction = ParameterDirection.Output;
                _dbContext.ExecuteStoredProcedure("sp_DeleteChiTietDotGopVon", pMaKhoiTaoDotGopVon, pUser,pMessage);                
                return pMessage.Value.ToString();
            }
            catch (Exception ex)
            {
                _log.WriteLog("GopVonService.DeleteGopVon:" + ex.Message.ToString());
                return "Lỗi:" + ex;
            }
        }

        public string DeletePTGopVon(string maPhieuThu)
        {
            try
            {
                var obj = _phieuThuRepository.Table.FirstOrDefault(x=>x.MaPhieuThu == maPhieuThu && x.IsDeleted == false);

                if (obj == null)
                    return "Không tìm thấy phiếu thu hoặc đã bị xóa trước đó";
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayThu).Days >= 0)
                {
                    return "Không thể xóa phiếu thu góp vốn rơi vào khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";
                }
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = DateTime.Now;
                obj.IsDeleted = true;
                _phieuThuRepository.Update(obj);
                return "";
            }
            catch(Exception ex)
            {
                _log.WriteLog("GopVonService.DeletePTGopVon:" + ex.Message.ToString());
                return ex.Message;
            }
        }

        public string DeletePCRutVon(string MaPhieuChi)
        {
            try
            {
                var obj = _phieuChiRepository.Table.FirstOrDefault(x => x.MaPhieuChi == MaPhieuChi && x.IsDeleted == false);
                if (obj == null)
                    return "Không tìm thấy phiếu chi hoặc đã bị xóa trước đó";
                DateTime ngayKetChuyenCuoi = _soKetChuyenService.NgayKetChuyenCuoiCung();
                if (ngayKetChuyenCuoi.Subtract(obj.NgayChi).Days >= 0)
                {
                    return "Không thể xóa phiếu chi rút vốn rơi vào khoảng thời gian đã kết chuyển. Vui lòng liên hệ Phòng Kiểm toán.";
                }
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                obj.UpdatedBy = uid;
                obj.UpdatedDate = DateTime.Now;
                obj.IsDeleted = true;
                _phieuChiRepository.Update(obj);
                return "";
            }
            catch (Exception ex)
            {
                _log.WriteLog("GopVonService.DeletePCRutVon:" + ex.Message.ToString());
                return ex.Message;
            }
        }

        // danh sach dot gop von
        public List<DotGopVon> ListDotGopVon()
        {
            return _dotGopVonRepository.Table.Where(x=>x.IsDeleted == false && x.LoaiGopVon == "").OrderByDescending(x=>x.TenKhoiTao).ToList();
        }
        public List<DotGopVon> ListDotRutVon()
        {
            return _dotGopVonRepository.Table.Where(x => x.IsDeleted == false && x.LoaiGopVon == "R").OrderByDescending(x => x.TenKhoiTao).ToList();
        }
        public ChiTietGopVon Item_CTGV(Guid Id)
        {
            return _chiTietGopVonRepository.Table.FirstOrDefault(x=>x.Id == Id);
        }
        public string DotGVCuoi()
        {
            var list = _dotGopVonRepository.Table.Where(x=>x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return list.MaKhoiTao;
        }
        public double CountDGV()
        {
            var list = _dotGopVonRepository.Table.Where(x => x.IsDeleted == false).ToList();
            return list.Count;
        }
        public double TongTienDoiTac(string id)
        {
            if(id != null)
            {
                double total = _chiTietGopVonRepository.Table.Where(x => x.IsDeleted == false && x.MaDoiTac == id).Sum(x => x.SoTien);
                return total;
            }
            else
            {
                return 0;
            }
        }
        public List<ChiTietGopVon> ListCTGV()
        {
            return _chiTietGopVonRepository.Table.Where(x => x.IsDeleted == false && x.LoaiPhieu != "R").ToList();
        }
        public List<ChiTietGopVon> ListCTRV()
        {
            return _chiTietGopVonRepository.Table.Where(x => x.IsDeleted == false && x.LoaiPhieu == "R").ToList();
        }
        public List<DotGopVon> ListDGV()
        {
            List<DotGopVon> lst = _dotGopVonRepository.Table.Where(x => x.IsDeleted == false).ToList();
            return lst;
        }
        public DataTable ListTongHop()
        {
            try
            {
                DataTable dt = _dbContext.ExecuteStoredProcedureDataTable("sp_GopVonTotal");
                return dt;
            }
            catch (Exception ex)
            {
                _log.WriteLog("GopVonService.ListTongHop:" + ex.Message.ToString());
                return null;
            }
        }
        public double TongTienGop(string id)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("maDoiTac", id);
                DataTable dt = _dbContext.ExecuteStoredProcedureDataTable("sp_GopVonTotal",param);
                if (dt is null || dt.Rows.Count == 0)
                    return 0;
                return double.Parse(dt.Rows[0]["TienChuaGop"].ToString());
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.ToString());
                return 0;
            }
            //double tongTien = _chiTietGopVonRepository.Table.Where(x => x.MaDoiTac == id && x.IsDeleted == false && x.LoaiPhieu != "R").Sum(x => x.SoTien);
            //double tiendagop = 0;
            //List<PhieuThu> dt = _phieuThuRepository.Table.Where(x=>x.DoiTac == id && x.MaLoaiPhieu == "TGOVO" && x.IsDeleted == false).ToList();
            //if(dt == null)
            //{
            //    tiendagop = 0;
            //}
            //else
            //{
            //    foreach(var item in dt)
            //    {
            //        tiendagop += item.SoTienThu;
            //    }
            //}
            //return tongTien - tiendagop;
        }

        public double TongTienRut(string id)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("maDoiTac", id);
                DataTable dt = _dbContext.ExecuteStoredProcedureDataTable("sp_GopVonTotal", param);
                if (dt is null || dt.Rows.Count == 0)
                    return 0;
                return double.Parse( dt.Rows[0]["ChuaRut"].ToString());
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.ToString());
                return 0;
            }
            //double tongTien = _chiTietGopVonRepository.Table.Where(x => x.MaDoiTac == id && x.IsDeleted == false && x.LoaiPhieu == "R").Sum(x => x.SoTien);
            //double tiendarut = 0;
            //List<PhieuChi> dt = _phieuChiRepository.Table.Where(x => x.DoiTac == id && x.MaLoaiPhieu == "CRUVO" && x.IsDeleted == false).ToList();
            //if (dt == null)
            //{
            //    tiendarut = 0;
            //}
            //else
            //{
            //    foreach (var item in dt)
            //    {
            //        tiendarut += item.SoTienChi;
            //    }
            //}
            //return tongTien - tiendarut;
        }
    }
}

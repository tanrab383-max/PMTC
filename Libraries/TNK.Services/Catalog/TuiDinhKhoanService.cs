using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.View;
using TNK.Data;
using TNK.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Services.Authentication;
using TNK.Core.Caching;
using TNK.Services.Log;

namespace TNK.Services.Catalog
{
    public class TuiDinhKhoanService : ITuiDinhKhoanService
    {
        IRepository<TuiDinhKhoan> _tuiDinhKhoanRepository;
        IRepository<ViewNoPhaiTra> _viewNoPhaiTraRepository;
        IRepository<ViewNoPhaiThu> _viewNoPhaiThuRepository;
        IRepository<ViewPhieuChi> _viewPhieuChiRepository;
        IRepository<PhieuThu> _phieuThuRepository;
        IRepository<ViewChiTietXuatNhapKhoPhuTung> _viewCtXNKPTRepository;

        IAuthenticationService _authenticationService;

        IDbContext _dbContext;
        ICacheManager _cacheManager;
        ILogger _log;
        public TuiDinhKhoanService(IRepository<TuiDinhKhoan> _tuiDinhKhoanRepository
            , IAuthenticationService _authenticationService
            , IRepository<ViewNoPhaiTra> _viewNoPhaiTraRepository
            , IRepository<ViewNoPhaiThu> _viewNoPhaiThuRepository
            , IRepository<ViewChiTietXuatNhapKhoPhuTung> _viewCtXNKPTRepository
            ,IRepository<ViewPhieuChi> _viewPhieuChiRepository
            , IRepository<PhieuThu> _phieuThuRepository
            , IDbContext _dbContext          
            , ICacheManager _cacheManager
            , ILogger _log
            )
        {
            this._tuiDinhKhoanRepository = _tuiDinhKhoanRepository;
            this._viewNoPhaiThuRepository = _viewNoPhaiThuRepository;
            this._viewNoPhaiTraRepository = _viewNoPhaiTraRepository;
            this._viewCtXNKPTRepository = _viewCtXNKPTRepository;
            this._viewPhieuChiRepository = _viewPhieuChiRepository;
            this._phieuThuRepository = _phieuThuRepository;
            this._dbContext = _dbContext;  
            this._authenticationService = _authenticationService;    
            this._cacheManager = _cacheManager;
            this._log = _log;

        }
       
        public List<TuiDinhKhoan> GetTuiDinhKhoanList(string loai = "")
        {
            try
            {
                List<TuiDinhKhoan> ls = null;
                if (loai == "")
                {
                    ls = _tuiDinhKhoanRepository.Table.Where(x => x.IsDeleted == false).OrderByDescending(x => Math.Abs(x.SoTienDangCo.Value)).ToList();
                }
                else
                    ls = _tuiDinhKhoanRepository.Table.Where(x => x.IsDeleted == false && x.LoaiTui == loai).OrderByDescending(x => Math.Abs(x.SoTienDangCo.Value)).ToList();
                return ls;
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetTuiDinhKhoanList:(" + loai + ") " + ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Lay danh sach kho de nhap phu tung
        /// </summary>
        /// <returns></returns>
        public List<ViewNguonKhuyenMai> GetKhoNhapPhuTung(bool isForInsert = true)
        {
            try
            {
                SqlParameter pIsForInsert = new SqlParameter("isForInsert", isForInsert);
                return _dbContext.ExecuteStoredProcedureList<ViewNguonKhuyenMai>("sp_SelectKhoNhapPhuTung", pIsForInsert).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetKhoNhapPhuTung " + ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Danh sách nợ phải trả
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<ViewNoPhaiTra> GetListNoPhaiTra(string query, DateTime from, DateTime to, int p, ref int total, int pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                    return _viewNoPhaiTraRepository.Table.Where(x => x.SoTienConLai > 0
                        && x.CreatedDate >= from
                        && x.CreatedDate <= to
                        && (x.ChungTuThu.Contains(query)
                                || x.MaPhieuNo.Contains(query)
                                || x.MaPhieuPhatSinh.Contains(query)
                           )
                        ).ToList();
                else
                    return _viewNoPhaiTraRepository.Table.Where(x => x.SoTienConLai > 0
                      && x.CreatedDate >= from
                      && x.CreatedDate <= to
                      ).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetListNoPhaiTra:("+query+","+from+ "," + to + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                return null;
            }

        }
        /// <summary>
        /// Danh sách nợ phải thu
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<ViewNoPhaiThu> GetListNoPhaiThu(string query, DateTime from, DateTime to, int p, ref int total, int pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                    return _viewNoPhaiThuRepository.Table.Where(x => x.SoTienConLai > 0
                        && x.CreatedDate >= from
                        && x.CreatedDate <= to
                        && (x.ChungTuThu.Contains(query)
                                || x.MaPhieuNo.Contains(query)
                                || x.MaPhieuPhatSinh.Contains(query)
                           )
                        ).ToList();
                else
                    return _viewNoPhaiThuRepository.Table.Where(x => x.SoTienConLai > 0
                      && x.CreatedDate >= from
                      && x.CreatedDate <= to
                      ).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetListNoPhaiThu:(" + query + "," + from + "," + to + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Chi tiết xuất nhập kho phụ tùng
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<ViewChiTietXuatNhapKhoPhuTung> GetListChiTietXuatNhapKhoPhuTung(string query,string maKho, DateTime from, DateTime to, int p, ref int total, int pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                    return _viewCtXNKPTRepository.Table.Where(x => x.CreatedDate >= from
                        && x.CreatedDate <= to
                        && x.MaPhieu.Contains(query)
                        && x.MaKho.ToUpper() == maKho.ToUpper()
                        ).ToList();
                else
                    return _viewCtXNKPTRepository.Table.Where(x => x.CreatedDate >= from
                      && x.CreatedDate <= to
                       && x.MaKho.ToUpper() == maKho.ToUpper()
                      ).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetListChiTietXuatNhapKhoPhuTung:(" + query + "," + from + "," + to + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Danh sách phiếu chi Hàng trên đường+
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<ViewPhieuChi> GetListHangTrenDuong(string query, DateTime from, DateTime to, int p, ref int total, int pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                    return _viewPhieuChiRepository.Table.Where(x => x.CreatedDate >= from
                        && x.CreatedDate <= to
                        && x.MaPhieuChi.Contains(query)
                        && x.MaLoaiPhieu == "CMHTD"

                        ).ToList();
                else
                    return _viewPhieuChiRepository.Table.Where(x => x.CreatedDate >= from
                      && x.CreatedDate <= to
                      && x.MaLoaiPhieu == "CMHTD"
                      ).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetListHangTrenDuong:(" + query + "," + from + "," + to + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                return null;
            }
        }

        public List<PhieuThu> GetListPhieuThu(string query, string maTui, DateTime from, DateTime to, int p, ref int total, int pageSize)
        {
            try
            {
                string maLoai = "";
                switch (maTui)
                {
                    case "COCBX":
                        maLoai = "TCOBX";
                        break;
                    case "COCDV":
                        maLoai = "TCODV";
                        break;
                    case "DTBX":
                        maLoai = "TBAXE";
                        break;
                    case "DTDV":
                        maLoai = "TDIVU";
                        break;
                    case "DTKHAC":
                        maLoai = "TKHAC,TLDAT";
                        break;
                }
                if (!string.IsNullOrEmpty(query))
                    return _phieuThuRepository.Table.Where(x => x.CreatedDate >= from
                        && x.CreatedDate <= to
                        && x.MaPhieuThu.Contains(query)
                        && x.MaLoaiPhieu.ToUpper() == maLoai.ToUpper()
                        ).Where(x => x.IsDeleted == false).ToList();
                else
                    return _phieuThuRepository.Table.Where(x => x.CreatedDate >= from
                      && x.CreatedDate <= to
                       && maLoai.ToUpper().Contains(x.MaLoaiPhieu.ToUpper())
                      ).Where(x => x.IsDeleted == false).ToList();
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetListPhieuThu:(" + query + "," + from + "," + to + "," + p + "," + total + "," + pageSize + ") " + ex.Message.ToString());
                return null;
            }
        }
        public TuiDinhKhoan GetTuiDinhKhoan(string maTui)
        {
            try
            {
                List<TuiDinhKhoan> ls = _tuiDinhKhoanRepository.Table.Where(x => x.MaTui.ToUpper() == maTui.ToUpper()).ToList();
                if (ls.Count > 0)
                    return ls[0];
                return null;
            }
            catch(Exception ex)
            {
                _log.WriteLog("LoaiXeService.GetTuiDinhKhoan:(" + maTui + ") " + ex.Message.ToString());
                return null;
            }
        }
        public List<TNK.Core.Domain.TuiDinhKhoan> ThongKeDinhKhoan()
        {
            try
            {
                List<TNK.Core.Domain.TuiDinhKhoan> list = _tuiDinhKhoanRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x=>x.TenTui).ToList();
                if (list.Count > 0)
                    return list;
                return null;
            }
            catch(Exception ex)
            {
                _log.WriteLog("TuiDinhKhoanService.ThongKeDinhKhoan:" + ex.Message.ToString());
                return null;
            }
        }
        // phần về treeview của thống kê định khoản
        public TuiDinhKhoan get_tien()
        {
            TuiDinhKhoan list = _tuiDinhKhoanRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false && x.MaTui == "TIEN").FirstOrDefault();
            return list;
        }
        public TuiDinhKhoan get_vay()
        {
            TuiDinhKhoan list =  _tuiDinhKhoanRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false && x.MaTui == "VAY").FirstOrDefault();
            return list;
        }
        public TuiDinhKhoan get_taisan()
        {
            TuiDinhKhoan list = _tuiDinhKhoanRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false && x.MaTui == "TAISAN").FirstOrDefault();
            return list;
        }
        public TuiDinhKhoan get_nguonvon()
        {
            TuiDinhKhoan list = _tuiDinhKhoanRepository.Table.Where(x => x.IsActive == true && x.IsDeleted == false && x.MaTui == "NGUONVON").FirstOrDefault();
            return list;
        }
        public List<TuiDinhKhoan> getByParent_tien(string id)
        {
            try
            {
                List<TuiDinhKhoan> list =_tuiDinhKhoanRepository.Table.Where(x => x.MaTuiCha == id && x.IsDeleted == false).OrderBy(x=>x.TenTui).ToList();
                if (list != null)
                    return list;
                return null;
            }
            catch(Exception ex)
            {
                _log.WriteLog("TuiDinhKhoanService.getByParent:" + ex.Message.ToString());
                return null;
            }
        }

        
       
        public List<TuiDinhKhoan> getByParent_vay(string id)
        {
            try
            {
                List<TuiDinhKhoan> list = _tuiDinhKhoanRepository.Table.Where(x => x.MaTuiCha == id && x.IsDeleted == false).OrderBy(x=>x.TenTui).ToList();
                if (list != null)
                    return list;
                return null;
            }
            catch (Exception ex)
            {
                _log.WriteLog("TuiDinhKhoanService.getByParent:" + ex.Message.ToString());
                return null;
            }
        }


        public List<TuiDinhKhoan> getByParent_taisan(string id)
        {
            try
            {
                List<TuiDinhKhoan> list = _tuiDinhKhoanRepository.Table.Where(x => x.MaTuiCha == id && x.IsDeleted==false).OrderBy(x=>x.TenTui).ToList();
                if (list != null)
                    return list;
                return null;
            }
            catch (Exception ex)
            {
                _log.WriteLog("TuiDinhKhoanService.getByParent:" + ex.Message.ToString());
                return null;
            }
        }


        public List<TuiDinhKhoan> getByParent_nguonvon(string id)
        {
            try
            {
                List<TuiDinhKhoan> list = _tuiDinhKhoanRepository.Table.Where(x => x.MaTuiCha == id).OrderBy(x=>x.TenTui).ToList();
                if (list != null)
                    return list;
                return null;
            }
            catch (Exception ex)
            {
                _log.WriteLog("TuiDinhKhoanService.getByParent:" + ex.Message.ToString());
                return null;
            }
        }
        public string Create(TuiDinhKhoan tuiDinhKhoan)
        {
            try
            {
                _tuiDinhKhoanRepository.Insert(tuiDinhKhoan);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
    }
}

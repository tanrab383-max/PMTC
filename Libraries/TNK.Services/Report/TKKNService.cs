using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using TNK.Core.Domain.Report;
using TNK.Data;
using TNK.Services.Authentication;
using TNK.Services.Common;
using TNK.Services.Log;
using TNK.Core;
namespace TNK.Services.Report
{
    public class TKKNService : ITKKNService
    {
        IAuthenticationService _authenticationService;
        IDbContext _dbContext;
        ILogger _log;
        ICacheManager _cacheManager;
        ICommonService _commonService;
        IRepository<ReportCongNo> _RPCNRepository;


        //protected TNKObjectContext _dbContext
        //{
        //    get { return new TNKObjectContext(); }
        //}



        public TKKNService(IAuthenticationService _authenticationService
            , ICacheManager _cacheManager
            , IDbContext _dbContext
            , ILogger _log
            , ICommonService _commonService
            , IRepository<ReportCongNo> _RPCNRepository
            )
        {
            this._commonService = _commonService;
            this._cacheManager = _cacheManager;
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
            this._log = _log;
            this._RPCNRepository = _RPCNRepository;
        }

        public DataTable BCNgayTienMat(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                //TNKObjectContext dbContext = new TNKObjectContext();
                ///return dbContext.ExecuteStoredProcedureDataTable
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TM", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayTienMat:" + ex.Message);
                TNK.Core.Common.WriteLogError("TKKNService", ex);
            }
            return null;
        }

        public DataTable BCNgayTienMatTU(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TM_TamUng", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayTienMatTU:" + ex.Message);
                TNK.Core.Common.WriteLogError("TKKNService", ex);
            }
            return null;

        }

        public DataTable BCNgayTienMatTienTaiKet(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter from = new SqlParameter("from", FromDate);
                SqlParameter to = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TM_TienTrongKet", from, to);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayTienMatTienTaiKet(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayTienMatGCN(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TM_GCN", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayTienMatGCN(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayNganHang(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayNganHang(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayNganHang_DanhSachNoVayNH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NH_NoVayNganHang", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayNganHang_DanhSachNoVayNH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayXe_KMPKXeChuaGiao(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTable("sp_BaoCaoNgay_Xe_KMPKXeChuaGiao", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BC_NHHGP(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayXe_NoCoc(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe_NoCoc", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayXe_NoCoc(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayChiTietPhieuDichVu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {

            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiTietPhieuDichVu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiTietPhieuDichVu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayChiTietPhieuDichVu_V2(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {

            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiTietPhieuDichVu_V4", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiTietPhieuDichVu_v2(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayChiTietPhieuDichVu_LT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiTietPhieuDichVu_LT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiTietPhieuDichVu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayChiTietPhieuDichVu_NoCoc(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_DV_NoCoc", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiTietPhieuDichVu_NoCoc(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayChiTietPhieuDichVu_NoCoc_LT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_DV_NoCoc_LT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiTietPhieuDichVu_NoCoc(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayDoanhThuDichVu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_DoanhThuDichVu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayDoanhThuDichVu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }



        public DataTable BCNgayChiPhiTM(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_TM", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhiTM(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayChiPhiNH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_NH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhiNH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayChiPhi_NhaCungCap(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_NhaCungCap", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhi_NhaCungCap(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayChiPhi_KhauHao(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_ChiPhi_KhauHao", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhi_KhauHao(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                return null;
            }
        }
        public DataTable BCNgayChiPhi_GoiBDTK(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_GoiBDTK", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhi_NhaCungCap(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayChiPhiQuyPhu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_QuyPhu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhiQuyPhu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayChiPhiChiTren5Trieu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_ChiTren5Trieu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhiChiTren5Trieu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayChiPhiChiTamUngBienNhan(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_TamUngBienNhan", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhiChiTamUngBienNhan(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayChiPhiChiTamUng(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiPhi_TamUng", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhiChiTamUng(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayXNT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_XNT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayXNT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayXNTTong(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_XNT_Tong", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayXNTTong(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayXNT_XECU(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);



                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_XNT_XC", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayXNT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayTongCocXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TongCocXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayTongCocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayKHBoCocXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_KHBoCocXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayKHBoCocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayThuChiCocXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TheoDoiCTCocXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayThuChiCocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayGiaiNgan(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_GiaiNgan", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayGiaiNgan(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayNoBanXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe_NoBanXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayNoBanXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayNoDichVu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoDV", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayNoDichVu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayNoDichVu_LT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoDV_LT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayNoDichVu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayTongCocPT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TongCocPT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayTongCocPT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayThuChiCocPT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiTietCocPT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayTongCocPT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayCocPT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_CocPT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayCocPT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNgayCocXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_CocXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayCocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoTHBH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoTHBH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoTHBH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoBHKM(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoBHKM", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoBHKM(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        //sp_BaoCaoNgay_NoHoaHongBaoHiem
        public DataTable BCNoHHBH(string ConnectionString, DateTime FromDate, DateTime ToDate, string Type)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                SqlParameter type = new SqlParameter("type", Type);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BCNgay_NoHoaHongBH", fromdate, todate, type);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoHHBH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoGHBN(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoGHBH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoGHBN(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoGHBHK(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoGHBHK", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoGHBHK(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNoHHGHBH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoHHGHBH ", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoHHGHBH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable TheoDoiChiTietCocXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TheoDoiCTCocXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.TheoDoiChiTietCocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable CTCocPT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiTietCocPT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.CTCocPT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable CocPT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_CocPT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.CocPT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable CocXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_CocXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.CocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }



        public List<ReportCongNo> GetReportCongNo(DateTime from, DateTime to, int p, ref int total, int pageSize)
        {
            try
            {
                var q = _RPCNRepository.Table.Where(x => x.NgayThu >= from && x.NgayThu <= to).OrderByDescending(x => x.MaPhieuThu).ToList();
                total = q.Count;
                return q.Skip((p - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.GetReportCongNo(" + from + "," + to + "," + p + "," + total + "," + pageSize + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        //phan truc them
        public DataTable dtbBCKhoXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe_DanhSachCoc", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.dtbBCKhoXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString()); ;
                return null;
            }
        }
        //thu coc
        public DataTable dtbChiTraCoc(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe_ChiTraCoc", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.dtbThuCoc(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        //thu no
        public DataTable dtbThuno(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe_ThuNo", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.dtbThuno(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        //thu giai ngan
        public DataTable dtbThuGiaiNgan(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe_GiaiNgan", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.dtbThuGiaiNgan(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable TongChiPhi(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TongChiPhi", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.TongChiPhi(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportTongHop(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportTongHop(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable ReportBCKTTamUng(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("Fromdate", FromDate);
                SqlParameter todate = new SqlParameter("Todate", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TamUng", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportBCKTTamUng(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable ReportBCKTCongNoDVBH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_CN_DV_BH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportBCKTCongNoDVBH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable ReportBCKTXNT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_XNT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportBCKTXNT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable ReportChiTiet(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_ChiTiet", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportChiTiet(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable ReportBCKTCanDoi(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_KetQuaKinhDoanh", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportBCKTCanDoi(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable ReportKQKD(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_KetQuaKinhDoanh", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportKQKD(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayChiTamUng(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("Fromdate", FromDate);
                SqlParameter todate = new SqlParameter("Todate", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TamUng", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiTamUng(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayGiayToXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("Fromdate", FromDate);
                SqlParameter todate = new SqlParameter("Todate", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_GiayToXe", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayGiayToXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        //BCNgayThuHoXang
        public DataTable BCNgayThuHoXang(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("Fromdate", FromDate);
                SqlParameter todate = new SqlParameter("Todate", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoThuHoKhac", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayThuHoXang(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoMuaPTPK(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("Fromdate", FromDate);
                SqlParameter todate = new SqlParameter("Todate", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoMuaPTPK", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoMuaPTPK(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCDoanhThuBHTH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_DoanhThuBHTH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCDoanhThuBHTH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCButToanLui(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("fromdate", FromDate);
                SqlParameter todate = new SqlParameter("todate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCao_CacButToanVeQuaKhu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCButToanLui(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCKhoPhuTungNgay(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter from = new SqlParameter("from", FromDate);
                SqlParameter to = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_KhoPhuTung", from, to);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCKhoPhuTungNgay(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCKhoPhuTungNgayV2(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter from = new SqlParameter("from", FromDate);
                SqlParameter to = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_KhoPhuTung_V2", from, to);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCKhoPhuTungNgayV2(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCXeTon(string ConnectionString, DateTime ToDate)
        {
            try
            {
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_TonKhoXe", toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCXeTon(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportCanDoiKQKD(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_CandoiKQKD", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportCanDoiKQKD(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportBangGopVon(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_GopVon", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportBangGopVon(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }


        public DataTable ReportThanhToanNhapNH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_ThanhToan_Nhap_NganHang", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportThanhToanNhapNH(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportTHXuatBanXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_XuatBanXe", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportTHXuatBanXe(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportTHLoiNhuanKho(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_LoiNhuanKho", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportTHLoiNhuanKho(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportTHChiPhi(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_ChiPhi", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportTHChiPhi(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportTHThueChuaVao(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_TheChuaVao", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportTHThueChuaVao(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportDuThau(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TongHop_TheChuaVao", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportDuThau(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable ReportBCKiemToan_Thang(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromDate = new SqlParameter("from", FromDate);
                SqlParameter toDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_BaoCaoThang", fromDate, toDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.ReportBCKiemToan_Thang(" + FromDate + " , " + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCDuThau(string ConnectionString, DateTime ToDate)
        {
            SqlParameter To = new SqlParameter("To", ToDate);
            return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_ChiPhi_DuThau", To);

        }
        public DataTable BC_MMTB_CCDC(string ConnectionString, DateTime ToDate)
        {
            SqlParameter To = new SqlParameter("To", ToDate);
            return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_MMTB_CCDC", To);
        }
        public DataTable BC_XDSC_Driver(string ConnectionString, DateTime ToDate)
        {
            SqlParameter To = new SqlParameter("To", ToDate);
            return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_TDriver", To);
        }
        public DataTable BC_XDSC(string ConnectionString, DateTime ToDate)
        {
            SqlParameter To = new SqlParameter("To", ToDate);
            return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoKiemToan_XDSC", To);
        }
        public DataTable BCCongNoKhac(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoKhac", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCCongNoKhac(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNoThuHHHB(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_BCNoThuHHHB", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoThuHHHB(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNPTraKhac(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_BCNPTraKhac", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNPTraKhac(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNPTraKhac_NBDTK(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NBDTK", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNPTraKhac_NBDTK(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCCNPTKhac(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_BCCNPTKhac", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCCNPTKhac(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCCNPTKhac_TTBH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoThanhTichBH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCCNPTKhac_TTBH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCHTD_XNT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("FromDate", FromDate);
                SqlParameter todate = new SqlParameter("ToDate", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoHangTrenDuong", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCHTD_XNT(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCChiTietCoc(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                //SqlParameter spFromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter spToDate = new SqlParameter("ToDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BCNgayChiTietCoc", spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCChiTietCoc(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCTienDauTu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter spToDate = new SqlParameter("ToDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BCTienDauTu", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCTienDauTu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCKhoTaiSan(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter spToDate = new SqlParameter("ToDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BCKhoTaiSan", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCKhoTaiSan(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCTongNoBHDV(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_TongNoDV", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCTongNoBHDV(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoHHBanXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_NoHHBanXe", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoHHBanXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoHHTaiXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_NoHHTaiXe", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoHHTaiXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCNoGCN(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_NoGCN", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoGCN(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCBPTC_CNBHDVTheoDoiTac(string ConnectionString, DateTime ToDate)
        {
            try
            {
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoBPTC_CongNoBaoHiemTheoDoiTac", spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_CNBHDVTheoDoiTac(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataSet BCBPTC_CNBHDVChiTiet(string ConnectionString, DateTime ToDate)
        {
            try
            {

                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataSet("sp_BaoCaoBPTC_CongNoBaoHiemChiTiet", spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_BCBPTC_CNBHDVChiTiet(" + ToDate + "):" + ex.Message.ToString());
                return null;
            }
        }

        public DataSet BCBPTC_VLD_ThuChi(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {

                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataSet("sp_BaoCaoBPTC_VLD_ThuChi", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_VLD_ThuChi(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataSet BCBPTC_VLD_Data(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {

                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataSet("sp_BaoCaoBPTC_VLD_Data", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_VLD_Data(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataSet BCBPTC_CanDoiKho(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataSet("sp_BaoCaoBPTC_CanDoiKho", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_CanDoiKho(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCBPTC_CNBHDVTheoLyDoQuaHan(string ConnectionString, DateTime ToDate)
        {
            try
            {
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoBPTC_CongNoBaoHiemTheoLyDoQuaHan", spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_CNBHDVTheoLyDoQuaHan(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCBPTC_SoatXetChungTu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoBPTC_SoatXetChungTu", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_SoatXetChungTuChiTiet(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCBPTC_SoatXetChungTuChiTiet(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoBPTC_SoatXetChungTu_ChiTiet", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_SoatXetChungTuChiTiet(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCBPTC_CTBanXe(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoBPTC_KetQuaKinhDoanhChiTietBanXe", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_ChiTietBanXe(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCBPTC_THBanXe(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoBPTC_KetQuaKinhDoanhTongBanXe", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCBPTC_TongHopBanXe(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCHHBHTH(DateTime FromDate, DateTime ToDate, string Type)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                SqlParameter spType = new SqlParameter("type", Type);
                DataTable dt = _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_ChiTietHHBH", spFromDate, spToDate, spType);
                return dt;
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCHHBHTH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCDoanhThuGHBH(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTable("sp_BaoCaoNgay_DoanhThuGHBH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayCocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCChiTietTGHBH(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTable("sp_BaoCaoNgay_ChiTietTGHBH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayCocXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCChiTietHHGHBH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_ChiTietHHGHBH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCDoanhThuBHTH(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BC_NHHGP(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_HoaHongGopNganHang", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BC_NHHGP(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BC_NHHGP_TH(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_HoaHongGopNganHang_TH", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BC_NHHGP(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public List<ReportList> GetReportLists(string connectionString, Guid userId)
        {
            try
            {
                SqlParameter userID = new SqlParameter("userID", userId);

                return _dbContext.ExecuteStoredProcedureList<ReportList>("sp_GetReportsByRole", userID).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable BC_DTGiayToXe(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BCDoanhThuGTX", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BC_DTGTX(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BC_DTThuHoKhac(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BCDoanhThuHoKhac", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BC_DTThuHoKhac(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BC_DTBaoDuongTietKiem(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BCCTDoanhThuBDTK", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.sp_BCCTDoanhThuBDTK(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTableCollection BC_NNHHGP(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableMulti(ConnectionString, "sp_BCNHHGP", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BC_NNHHGP(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayChiPhiChiTiet(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("usp_BaoCaoNgay_ChiTietCP", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayChiPhiChiTiet(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayXeLoiNhuan(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_Xe_LoiNhuan", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayXe(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCQuanLyXePTC(DateTime ToDate)
        {
            try
            {
                SqlParameter spToDate = new SqlParameter("toDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_QuanLyXe_PTC", spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCQuanLyPTC(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTableCollection BCNoChiPhi_XeCu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableMulti(ConnectionString, "sp_BaoCaoNgay_NoChiPhi_XeCu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoChiPhi_XeCu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTableCollection BCPBChiPhi_XeCu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableMulti(ConnectionString, "sp_BaoCaoNgay_ChiPhiXeCu_PhanBo", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoChiPhi_XeCu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNoCPXC(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);

                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_BaoCaoNgay_NoCPXeCu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNoHHPTD(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTableCollection BCKM_HuaTang(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableMulti(ConnectionString, "sp_BCNgayKMHuaTang", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCKm_HuaTang(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCKM_HuaTang_XE(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("from", FromDate);
                SqlParameter spToDate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgayKM_HuaTang_Xe", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCKM_HuaTang_XE(" + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCTienDauTu(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter spToDate = new SqlParameter("ToDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BCTienDauTu", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCTienDauTu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCKhoTaiSan(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter spToDate = new SqlParameter("ToDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BCKhoTaiSan", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCKhoTaiSan(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }

        public DataTable BCDauTuDuAn(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter spFromDate = new SqlParameter("FromDate", FromDate);
                SqlParameter spToDate = new SqlParameter("ToDate", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BCTienDauTuDuAn", spFromDate, spToDate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCDauTuDuAn(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayPhanBoChiPhi(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_ReportPhanBoChiPhi", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayPhanBoChiPhi(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                return null;
            }
        }
        public DataTable BCPhieuThu(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper(ConnectionString, "sp_ReportPhieuThu", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCPhieuThu(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                TNK.Core.Common.WriteLogError("TKKNService", ex);
                return null;
            }
        }
        public DataTable BCNgayCNO(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgayCONO", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayCNO(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                return null;
            }
        }
        public DataTableCollection BCNgayThuChiHo(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableMulti(ConnectionString, "sp_BaoCaoNgay_ChiHoThuHo", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgayThuChiHo(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                return null;
            }
        }
        public DataTable BCNgay_ChiTietChiPhiHHTX(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTableSqlHelper("sp_BaoCaoNgay_ChiTietChiPhiHHTX", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgay_ChiTietChiPhiHHTX(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                return null;
            }
        }
        public DataTable BCNgay_KTNB_TongTaiSan(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_BaoCaoKTNB_TongTaiSan", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgay_KTNB_TongTaiSan(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                throw ex;
               // return null;
            }
        }
        public DataTable BCKHCDT(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_BaoCaoNgay_BCKHCĐT", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCNgay_KTNB_TongTaiSan(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                throw ex;
                // return null;
            }
        }
        public DataTable BCLoiNhuanPTC(string ConnectionString, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                SqlParameter fromdate = new SqlParameter("from", FromDate);
                SqlParameter todate = new SqlParameter("to", ToDate);
                return _dbContext.ExecuteStoredProcedureDataTable("sp_TongHopLoiNhuanPTC", fromdate, todate);
            }
            catch (Exception ex)
            {
                _log.WriteLog("TKKNService.BCLoiNhuanPTC(" + FromDate + "," + ToDate + "):" + ex.Message.ToString());
                throw ex;
                // return null;
            }
        }
    }
}

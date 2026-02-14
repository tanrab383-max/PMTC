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
using TNK.Services.Common;
using System.Data;
using TNK.Services.Users;
using CRM.Web.Mail;
using AF.Library;
namespace TNK.Services.SoThu
{
    public class SoThuService : ISoThuService
    {
        IRepository<ViewHinhThucThanhToan> _htttRepository;
        IRepository<ChiTietPhieuDichVu> _CTPDVRepository;
        IRepository<DoiTac> _doiTacRepository;
        IRepository<Config> _configRepository;
        IRepository<KhoPhuTung> _khoPhuTung;
        IAuthenticationService _authenticationService;
        IDbContext _dbContext;
        ILogger _log;
        ICacheManager _cacheManager;
        ICommonService _commonService;
        IRepository<ViewLoaiXe> _viewLoaiXeRepository;
        IRepository<PhieuThu> _phieuThuRepository;
        IUserervice _userService;
        AF.Library.Logger logger = new AF.Library.Logger("SoThuService");
        public SoThuService(IRepository<ViewHinhThucThanhToan> _htttRepository
            , IAuthenticationService _authenticationService
            , IRepository<DoiTac> _doiTacRepository
            , IRepository<ViewPhieuDichVuTam> _VPDVTRepository
            , IRepository<REPAIR_ORDER_PYS> _PDVTRepository
            , IRepository<ChiTietPhieuDichVu> _CTPDVRepository
            , IRepository<NoPhaiTra> _noPhaiTraRepository
            , IRepository<Config> _configRepository
            , IRepository<KhoPhuTung> _khoPhuTung
            , ICacheManager _cacheManager
            , IDbContext _dbContext
            , ILogger _log
            , IRepository<ViewLoaiXe> _viewLoaiXeRepository
            , ICommonService _commonService
            , IRepository<PhieuThu> _phieuThuRepository
            , IUserervice _userService
            )
        {
            this._htttRepository = _htttRepository;
            this._doiTacRepository = _doiTacRepository;
            this._CTPDVRepository = _CTPDVRepository;
            this._commonService = _commonService;
            this._cacheManager = _cacheManager;
            this._configRepository = _configRepository;
            this._khoPhuTung = _khoPhuTung;
            this._authenticationService = _authenticationService;
            this._dbContext = _dbContext;
            this._log = _log;
            this._viewLoaiXeRepository = _viewLoaiXeRepository;
            this._phieuThuRepository = _phieuThuRepository;
            this._userService = _userService;
        }
        static int gioGuiEmailLast = Convert.ToInt32(DateTime.Now.Hour.ToString());
        static bool Send = false;
        public string ContentMail(string maPhieuThu)
        {
            string Content = "";
            PhieuThu PT = _phieuThuRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu && x.IsDeleted == false).FirstOrDefault();
            Guid nguoiTao = PT.CreatedBy.Value;
            Guid nguoiSua = PT.UpdatedBy.Value;
            User nguoitacDong = new User();
            if (nguoiTao != nguoiSua)
            {
                nguoitacDong = _userService.getByUserId(nguoiTao);
            }
            else
            {
                nguoitacDong = _userService.getByUserId(nguoiSua);
            }
            Content = "Mã phiếu thu " + maPhieuThu + " do " + nguoitacDong.UserName + " đã thao tác lúc " + (PT.UpdatedDate == null ? PT.CreatedDate : PT.UpdatedDate) + " đã làm mất cân!";
            return Content;
        }
        public List<DoiTac> GetDoiTac(string id)
        {
            try
            {
                SqlParameter loai = new SqlParameter("loai", id);
                SqlParameter isForInsert = new SqlParameter("isForInsert", true);
                var lst = _dbContext.ExecuteStoredProcedureList<DoiTac>("sp_SelectDoiTac", loai, isForInsert).ToList();
                return lst;
            }
            catch (Exception ex)
            {

                //if (ex.InnerException != null)
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + ex.Message.ToString());
                _log.WriteLog("GetDoiTac: ", ex);
            }
            return new List<DoiTac>();
        }


        public List<ViewHinhThucThanhToan> GetHTTT()
        {
            return _htttRepository.Table.OrderBy(x => x.HienThi).ToList();
        }
        public bool UpdateChiTietPhieuDichVu(List<ChiTietPhieuDichVu> model)
        {
            logger.Start("UpdateChiTietPhieuDichVu");            
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                List<KhoPhuTung> lst = _khoPhuTung.Table.ToList();
                foreach (var i in model)
                {
                    var obj = _CTPDVRepository.Table.FirstOrDefault(x => x.Id == i.Id);
                    if (obj != null)
                    {
                        //tinh toan lai gia von theo phan tram gia von duoc dinh nghia duoi database
                        double? phanTramGiaVon = 0;
                        string maKho = i.PhanLoai.ToUpper(); //tu phan loai hien thi chi tiet phieu dich vu moi lay ra duoc ma kho tuong ung do da gop cac kho lai (KHO_PTT+KHO_PTN ==> KHO_PTT,..)  
                        if (lst != null && lst.Count > 0)
                        {
                            if (maKho == "KHO_PKT")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                                maKho = "KHO_PTT";
                            if (maKho == "CĐ")
                                maKho = "SUACHUA";
                            if (maKho == "KHO_VTN" || maKho == "KHO_VTGTGT")
                                maKho = "KHO_PKIEN";
                            if (maKho == "KHO_PKN")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                                maKho = "KHO_PKIEN";
                            if (maKho == "KHO_DTS")
                                maKho = "KHO_DS";
                            var b = lst.FirstOrDefault(x => x.MaKho == maKho);
                            if (b != null)
                                phanTramGiaVon = b.PhanTramGiaVon;
                        }
                        if (phanTramGiaVon == null)
                            phanTramGiaVon = 1;
                        obj.MaKho = maKho;
                        obj.PhanLoai = i.PhanLoai;
                        //kho dong son chua dinh nghia duoc gia von ma tinh theo ty le
                        //hoac them moi thu cong khong co gia von tu Cyber
                        //if (obj.MaKho.ToLower() == "kho_ds" || (obj.MaKho != "SUACHUA" && i.GiaVon == 0 ))
                        //    obj.GiaVon = phanTramGiaVon.Value * i.COST;
                        //else
                        //    obj.GiaVon = i.GiaVon;
                        obj.GiaVon = Math.Round(obj.GiaVon.Value);
                        obj.GCN = i.GCN;
                        obj.UpdatedDate = d;
                        obj.UpdatedBy = uid;
                        logger.Info(">>CTDV");
                        logger.Param("obj.MaPhieuThu", obj.MaPhieuThu);
                        logger.Param("obj.MaKho",obj.MaKho);
                        logger.Param("obj.JOBSNAME", obj.JOBSNAME);
                        logger.Param("obj.COST", obj.COST);
                        logger.Param("obj.VAT", obj.VAT);
                        logger.Param("obj.GiaVon", obj.GiaVon);
                        logger.Info("<<CTDV");
                        _CTPDVRepository.Update(obj);
                    }

                }
                logger.End("UpdateChiTietPhieuDichVu");
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException!= null)
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + e.Message.ToString());
                _log.WriteLog("UpdateChiTietPhieuDichVu: ", e);
                logger.End("UpdateChiTietPhieuDichVu");
                return false;
            }
        }
        public bool UpdateChiTietPhieuDichVu_v1(List<ChiTietPhieuDichVu> model,string id)
        {
            logger.Start("UpdateChiTietPhieuDichVu");
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                List<KhoPhuTung> lst = _khoPhuTung.Table.ToList();
                foreach (var i in model)
                {
                    var obj = _CTPDVRepository.Table.FirstOrDefault(x => x.Id == i.Id);
                    if (obj != null)
                    {
                        //tinh toan lai gia von theo phan tram gia von duoc dinh nghia duoi database
                        double? phanTramGiaVon = 0;
                        string maKho = i.PhanLoai.ToUpper(); //tu phan loai hien thi chi tiet phieu dich vu moi lay ra duoc ma kho tuong ung do da gop cac kho lai (KHO_PTT+KHO_PTN ==> KHO_PTT,..)  
                        if (lst != null && lst.Count > 0)
                        {
                            if (maKho == "KHO_PKT")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                                maKho = "KHO_PTT";
                            if (maKho == "CĐ")
                                maKho = "SUACHUA";
                            if (maKho == "KHO_VTN" || maKho == "KHO_VTGTGT")
                                maKho = "KHO_PKIEN";
                            if (maKho == "KHO_PKN")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                                maKho = "KHO_PKIEN";
                            if (maKho == "KHO_DTS")
                                maKho = "KHO_DS";
                            var b = lst.FirstOrDefault(x => x.MaKho == maKho);
                            if (b != null)
                                phanTramGiaVon = b.PhanTramGiaVon;
                        }
                        if (phanTramGiaVon == null)
                            phanTramGiaVon = 1;
                        obj.MaKho = maKho;
                        obj.PhanLoai = i.PhanLoai;
                        //kho dong son chua dinh nghia duoc gia von ma tinh theo ty le
                        //hoac them moi thu cong khong co gia von tu Cyber
                        //if (i.GiaVon == 0)
                        //    obj.GiaVon = phanTramGiaVon.Value * i.COST;
                        //else
                        //    obj.GiaVon = i.GiaVon;
                        obj.GiaVon = Math.Round(i.GiaVon.Value);
                        obj.GCN = i.GCN;
                        obj.UpdatedDate = d;
                        obj.UpdatedBy = uid;
                        obj.VAT = i.VAT;
                        logger.Info(">>CTDV");
                        logger.Param("obj.MaPhieuThu", obj.MaPhieuThu);
                        logger.Param("obj.MaKho", obj.MaKho);
                        logger.Param("obj.JOBSNAME", obj.JOBSNAME);
                        logger.Param("obj.COST", obj.COST);
                        logger.Param("obj.VAT", obj.VAT);
                        logger.Param("obj.GiaVon", obj.GiaVon);
                        logger.Info("<<CTDV");
                        _CTPDVRepository.Update(obj);
                    }
                    else
                    {
                        InsertChiTietPhieuDichVu_New(i, id);
                    }
                }
               
                logger.End("UpdateChiTietPhieuDichVu");
                return true;
            }
            catch (Exception e)
            {
                //if(e.InnerException!= null)
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + e.Message.ToString());
                _log.WriteLog("UpdateChiTietPhieuDichVu: ", e);
                logger.End("UpdateChiTietPhieuDichVu");
                return false;
            }
        }
        public bool UpdateChiTietPhieuDichVu_New(List<ChiTietPhieuDichVu> model,string maPhieuThu)
        {
            logger.Start("UpdateChiTietPhieuDichVu_New");
            try
            {
                DateTime d = DateTime.Now;
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                List<KhoPhuTung> lst = _khoPhuTung.Table.ToList();
                List<ChiTietPhieuDichVu> list2 = _CTPDVRepository.Table.Where(x => x.MaPhieuThu == maPhieuThu && x.IsDeleted == false).ToList();
                var list1 = new List<ChiTietPhieuDichVu>();
                foreach (var i in model)
                {
                    var obj = _CTPDVRepository.Table.FirstOrDefault(x => x.Id == i.Id);
                    string maKho = i.PhanLoai.ToUpper(); //tu phan loai hien thi chi tiet phieu dich vu moi lay ra duoc ma kho tuong ung do da gop cac kho lai (KHO_PTT+KHO_PTN ==> KHO_PTT,..)  
                    if (obj != null)
                    {
                        list1.Add(obj);
                        //tinh toan lai gia von theo phan tram gia von duoc dinh nghia duoi database
                        double? phanTramGiaVon = 0;
                        if (maKho == "KHO_PKT")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                            maKho = "KHO_PTT";
                        if (maKho == "CĐ")
                            maKho = "SUACHUA";
                        if (maKho == "KHO_VTN" || maKho == "KHO_VTGTGT")
                            maKho = "KHO_PKIEN";
                        if (maKho == "KHO_PKN")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                            maKho = "KHO_PKIEN";
                        if (maKho == "KHO_DTS")
                            maKho = "KHO_DS";
                        if (lst != null && lst.Count > 0)
                        {
                            var b = lst.FirstOrDefault(x => x.MaKho == maKho);
                            if (b != null)
                                phanTramGiaVon = b.PhanTramGiaVon;
                        }
                        if (phanTramGiaVon == null)
                            phanTramGiaVon = 1;
                        obj.MaKho = maKho;
                        obj.PhanLoai = i.PhanLoai;
                        //hoac them moi thu cong khong co gia von tu Cyber
                        //if (obj.MaKho.ToLower() == "kho_ds" || (obj.MaKho != "SUACHUA" && i.GiaVon == 0))
                        //    obj.GiaVon = phanTramGiaVon.Value * i.COST;
                        //else if(obj.MaKho.ToLower() == ("KHO_DS").ToLower() && i.GiaVon == 0)
                        //{
                        //    obj.GiaVon = i.COST;
                        //}
                        //else
                        //obj.GiaVon = i.GiaVon;
                        obj.GiaVonVAT = Math.Round(i.GiaVonVAT.Value);
                        obj.GCN = i.GCN;
                        obj.UpdatedDate = d;
                        obj.UpdatedBy = uid;
                        obj.COST = i.COST;
                        obj.DISCOUNT = i.DISCOUNT;
                        obj.IsDeleted = i.IsDeleted;
                        obj.JOBSNAME = i.JOBSNAME;
                        obj.VAT = i.VAT;
                        _CTPDVRepository.Update(obj);
                        logger.Info(">>CTDV");
                        logger.Param("obj.MaPhieuThu", obj.MaPhieuThu);
                        logger.Param("obj.MaKho", obj.MaKho);
                        logger.Param("obj.JOBSNAME", obj.JOBSNAME);
                        logger.Param("obj.COST", obj.COST);
                        logger.Param("obj.VAT", obj.VAT);
                        logger.Param("obj.GiaVon", obj.GiaVon);
                        logger.Info("<<CTDV");

                    }
                    else
                    {
                        InsertChiTietPhieuDichVu_New(i, maPhieuThu);
                    }
                }
                List<ChiTietPhieuDichVu> list_end = new List<ChiTietPhieuDichVu>();
                foreach (var temp in list2.ToList())
                {
                    if (list1.Contains(temp) == false)
                    {
                        list_end.Add(temp);
                    }
                }
                foreach (var temp_new in list_end)
                {
                    temp_new.IsDeleted = true;
                    temp_new.IsActive = false;
                    _CTPDVRepository.Update(temp_new);
                }
                logger.End("UpdateChiTietPhieuDichVu_New");
                return true;
            }
            catch (Exception e)
            {
                //if (e.InnerException != null)
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("UpdateChiTietPhieuDichVu: " + e.Message.ToString());
                _log.WriteLog("UpdateChiTietPhieuDichVu: " , e);
                logger.Error(e);
                logger.End("UpdateChiTietPhieuDichVu_New");
                return false;
            }
           
        }
        public bool InsertChiTietPhieuDichVu(List<ChiTietPhieuDichVu> model, string id)
        {
            logger.Start("InsertChiTietPhieuDichVu");
            try
            {

                List<KhoPhuTung> lst = _khoPhuTung.Table.ToList();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                foreach (var item in model)
                {
                    string maKho = item.PhanLoai.ToUpper(); //tu phan loai hien thi chi tiet phieu dich vu moi lay ra duoc ma kho tuong ung do da gop cac kho lai (KHO_PTT+KHO_PTN ==> KHO_PTT,..)  
                    if (maKho == "KHO_PKT")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                        maKho = "KHO_PTT";
                    if (maKho == "CĐ")
                        maKho = "SUACHUA";
                    if (maKho == "KHO_VTN" || maKho == "KHO_VTGTGT")
                        maKho = "KHO_PKIEN";
                    if (maKho == "KHO_PKN")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                        maKho = "KHO_PKIEN";
                    if (maKho == "KHO_DTS")
                        maKho = "KHO_DS";
                    //tinh toan lai gia von theo phan tram gia von duoc dinh nghia duoi database
                    KhoPhuTung kpt = lst.Find(x => x.MaKho == maKho);

                    double? phanTramGiaVon = 0;
                    if (kpt != null)
                        phanTramGiaVon = kpt.PhanTramGiaVon;
                    //neu khong phai kho sua chua thi cap nhat gia cong ngoai = 0
                    if (item.MaKho.ToLower() != "suachua")
                        item.GCN = 0;
                    item.MaKho = maKho;
                    //kho dong son chua dinh nghia duoc gia von ma tinh theo ty le
                    //hoac them moi thu cong khong co gia von tu Cyber
                    //if (item.MaKho.ToLower() == "kho_ds" ) //|| ( item.MaKho != "SUACHUA" && item.GiaVon == 0))
                    //    item.GiaVon = phanTramGiaVon.Value * item.COST;
                    //item.GiaVon = phanTramGiaVon.Value * item.COST;
                    //if ((item.GiaVon == null || item.GiaVon == 0) && item.MaKho.ToLower() != "kho_ds")
                    //    item.GiaVon = Math.Round(item.COST * phanTramGiaVon.Value);
                       // item.GiaVon = Math.Round(item.GiaVon.Value);
                    item.MaPhieuThu = id;
                    //thieu phan item.Id;
                    item.CreatedBy = uid;
                    item.CreatedDate = d;
                    item.UpdatedBy = uid;
                    item.UpdatedDate = d;
                    item.IsDeleted = false;
                    item.IsActive = true;
                    //if(item.VAT != 0.05)
                    //    item.VAT = 0.1;
                    _CTPDVRepository.Insert(item);
                    logger.Info(">>CTDV");
                    logger.Param("item.MaPhieuThu", item.MaPhieuThu);
                    logger.Param("item.MaKho", item.MaKho);
                    logger.Param("item.JOBSNAME", item.JOBSNAME);
                    logger.Param("item.COST", item.COST);
                    logger.Param("item.VAT", item.VAT);
                    logger.Param("item.GiaVon", item.GiaVon);
                    logger.Info("<<CTDV");
                }
                logger.End("UpdateChiTietPhieuDichVu_New");
                return true;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("InsertChiTietPhieuDichVu: " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("InsertChiTietPhieuDichVu: " + ex.Message.ToString());
                _log.WriteLog("InsertChiTietPhieuDichVu: " , ex);
                logger.Error(ex);
                logger.End("UpdateChiTietPhieuDichVu_New");
                return false;
            }
        }
        public bool InsertChiTietPhieuDichVu_Import(List<ChiTietPhieuDichVu> model, string id)
        {
            logger.Start("InsertChiTietPhieuDichVu_Import");
            try
            {

                List<KhoPhuTung> lst = _khoPhuTung.Table.ToList();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                foreach (var item in model)
                {
                    string maKho = item.PhanLoai.ToUpper(); //tu phan loai hien thi chi tiet phieu dich vu moi lay ra duoc ma kho tuong ung do da gop cac kho lai (KHO_PTT+KHO_PTN ==> KHO_PTT,..)  
                    if (maKho == "KHO_PKT")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                        maKho = "KHO_PTT";
                    if (maKho == "CĐ")
                        maKho = "SUACHUA";
                    if (maKho == "KHO_VTN" || maKho == "KHO_VTGTGT")
                        maKho = "KHO_PKIEN";
                    if (maKho == "KHO_PKN")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                        maKho = "KHO_PKIEN";
                    if (maKho == "KHO_DTS")
                        maKho = "KHO_DS";
                    //tinh toan lai gia von theo phan tram gia von duoc dinh nghia duoi database
                    KhoPhuTung kpt = lst.Find(x => x.MaKho == maKho);

                    double? phanTramGiaVon = 0;
                    if (kpt != null)
                        phanTramGiaVon = kpt.PhanTramGiaVon;
                    //neu khong phai kho sua chua thi cap nhat gia cong ngoai = 0
                    if (item.MaKho.ToLower() != "suachua")
                        item.GCN = 0;
                    item.MaKho = maKho;
                    //kho dong son chua dinh nghia duoc gia von ma tinh theo ty le
                    //hoac them moi thu cong khong co gia von tu Cyber
                    //if(item.MaKho.ToLower() == "kho_ds" )// || (item.MaKho != "SUACHUA" && item.GiaVon ==0))
                    //    item.GiaVon = phanTramGiaVon.Value * item.COST;

                    item.GiaVon = Math.Round(item.GiaVon.Value);
                    item.GiaVonVAT = Math.Round(item.GiaVonVAT.Value);
                    item.MaPhieuThu = id;
                    //thieu phan item.Id;
                    item.CreatedBy = uid;
                    item.CreatedDate = d;
                    item.UpdatedBy = uid;
                    item.UpdatedDate = d;
                    item.IsDeleted = false;
                    item.IsActive = true;
                    _CTPDVRepository.Insert(item);
                    logger.Info(">>CTDV");
                    logger.Param("item.MaPhieuThu", item.MaPhieuThu);
                    logger.Param("item.MaKho", item.MaKho);
                    logger.Param("item.JOBSNAME", item.JOBSNAME);
                    logger.Param("item.COST", item.COST);
                    logger.Param("item.VAT", item.VAT);
                    logger.Param("item.GiaVon", item.GiaVon);
                    logger.Info("<<CTDV");
                }
                logger.End("InsertChiTietPhieuDichVu_Import");
                return true;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("InsertChiTietPhieuDichVu: " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("InsertChiTietPhieuDichVu: " + ex.Message.ToString());
                _log.WriteLog("InsertChiTietPhieuDichVu: ", ex);
                logger.Error(ex);
                logger.End("InsertChiTietPhieuDichVu_Import");
                return false;
            }
        }
        public bool InsertChiTietPhieuDichVu_New(ChiTietPhieuDichVu item, string id)
        {
            logger.Start("InsertChiTietPhieuDichVu_New");
            try
            {

                List<KhoPhuTung> lst = _khoPhuTung.Table.ToList();
                var uid = _authenticationService.GetAuthenticatedUser().UserId;
                DateTime d = DateTime.Now;
                //tinh toan lai gia von theo phan tram gia von duoc dinh nghia duoi database
                string maKho = item.PhanLoai.ToUpper();
                if (maKho == "KHO_PKT")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                    maKho = "KHO_PTT";
                if (maKho == "CĐ")
                    maKho = "SUACHUA";
                if (maKho == "KHO_VTN" || maKho == "KHO_VTGTGT")
                    maKho = "KHO_PKIEN";
                if (maKho == "KHO_PKN")  //gop kho phu tung trong va phu tung ngoai thanh kho phu tung trong
                    maKho = "KHO_PKIEN";
                if (maKho == "KHO_DTS")
                    maKho = "KHO_DS";
                KhoPhuTung kpt = lst.Find(x => x.MaKho == maKho);
                double? phanTramGiaVon = 0;
                if (kpt != null)
                    phanTramGiaVon = kpt.PhanTramGiaVon;
                //neu khong phai kho sua chua thi cap nhat gia cong ngoai = 0
                if (item.MaKho.ToLower() != "suachua")
                    item.GCN = 0;
                item.MaKho = maKho;
                // item.GiaVon = phanTramGiaVon.Value * item.COST;
                //kho dong son chua dinh nghia duoc gia von ma tinh theo ty le
                //hoac them moi thu cong khong co gia von tu Cyber
                //if (item.MaKho.ToLower() == "kho_ds" )// || (item.MaKho != "SUACHUA" && item.GiaVon == 0))
                //    item.GiaVon = phanTramGiaVon.Value * item.COST;

                item.GiaVon = Math.Round(item.GiaVon.Value);
                item.MaPhieuThu = id;
                //thieu phan item.Id;
                item.CreatedBy = uid;
                item.CreatedDate = d;
                item.UpdatedBy = uid;
                item.UpdatedDate = d;
                item.IsDeleted = false;
                item.IsActive = true;
                if (item.VAT != 0.05)
                    item.VAT = 0.1;
                _CTPDVRepository.Insert(item);
                logger.Info(">>CTDV");
                logger.Param("item.MaPhieuThu", item.MaPhieuThu);
                logger.Param("item.MaKho", item.MaKho);
                logger.Param("item.JOBSNAME", item.JOBSNAME);
                logger.Param("item.COST", item.COST);
                logger.Param("item.VAT", item.VAT);
                logger.Param("item.GiaVon", item.GiaVon);
                logger.Info("<<CTDV");
                logger.End("InsertChiTietPhieuDichVu_New");
                return true;
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("InsertChiTietPhieuDichVu: " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("InsertChiTietPhieuDichVu: " + ex.Message.ToString());
                _log.WriteLog("InsertChiTietPhieuDichVu: " , ex);
                logger.Error(ex);
                logger.End("InsertChiTietPhieuDichVu_New");
                return false;
            }
        }


        public List<ChiTietPhieuDichVu> GetCTPDV(string id)
        {
            return _CTPDVRepository.Table.Where(x => x.MaPhieuThu == id && x.IsDeleted == false).ToList();
        }
        public string XuLySauKhiInsertPhieuThu(string maPhieuThu,string giaTriTrenGiaoDien,string ipClient ="",string hostNameClient ="")
        {
            try
            {
                SqlParameter MPT = new SqlParameter("MaPhieuThu", maPhieuThu);
                SqlParameter pGiaTriTrenGiaoDien = new SqlParameter("giaTriTrenGiaoDien", giaTriTrenGiaoDien);//xử lý sau khi update
                pGiaTriTrenGiaoDien.Direction = System.Data.ParameterDirection.Input;
                pGiaTriTrenGiaoDien.Size = 10000;
                SqlParameter pIpClient = new SqlParameter("IPClient", ipClient);
                SqlParameter pHostNameClient = new SqlParameter("HostNameClient", hostNameClient);
                SqlParameter Error = new SqlParameter("Error", "");//xử lý sau khi update
                Error.Direction = System.Data.ParameterDirection.Output;
                Error.Size = 4000;
                _dbContext.ExecuteStoredProcedure("sp_XuLySauKhiInsertPhieuThu", MPT,pGiaTriTrenGiaoDien,pIpClient,pHostNameClient, Error);
                if (Error.Value.ToString() == "Không cân" || Error.Value.ToString() == "K")
                {
                    int GioGuiEmail = Convert.ToInt32(DateTime.Now.Hour.ToString());
                    if (gioGuiEmailLast != GioGuiEmail)
                    {
                        MailHelper Mail = new MailHelper();
                        string Content = ContentMail(maPhieuThu);
                        //Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                        //Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                        //Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                        gioGuiEmailLast = GioGuiEmail;
                    }
                    else
                    {
                        if (Send == false)
                        {
                            MailHelper Mail = new MailHelper();
                            string Content = ContentMail(maPhieuThu);
                           // Mail.SendMail("thien.nguyen@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                           // Mail.SendMail("truc.le@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                           // Mail.SendMail("thang.tran@phattien.com", "Lỗi mất cân trên HQ2", Content + " " + Error.Value.ToString());
                            gioGuiEmailLast = GioGuiEmail;
                            Send = true;
                        }
                    }
                }
                return Error.Value.ToString();
            }
            catch (Exception e)
            {
                //if(e.InnerException!= null)
                //    _log.WriteLog("SoThuService.XuLySauKhiInsertPhieuThu: " + e.InnerException.ToString());
                //else
                //    _log.WriteLog("SoThuService.XuLySauKhiInsertPhieuThu: " + e.Message.ToString());
                _log.WriteLog("SoThuService.XuLySauKhiInsertPhieuThu: " , e);
                return null;
            }
        }
        public List<KhoPhuTung> ListKPT()
        {
            try
            {
                List<KhoPhuTung> list = _khoPhuTung.Table.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                return list;
            }
            catch(Exception ex)
            {
                //if (ex.InnerException != null)
                //    _log.WriteLog("SoThuService.ListKPT: " + ex.InnerException.ToString());
                //else
                //    _log.WriteLog("SoThuService.ListKPT: " + ex.Message.ToString());
                _log.WriteLog("SoThuService.ListKPT: " , ex);
                return null;
            }
        }
        public List<ViewLoaiXe> ListLoaiXe()
        {
            return _viewLoaiXeRepository.Table.Where(x => x.IsDeleted == false).ToList();
        }
    }
}

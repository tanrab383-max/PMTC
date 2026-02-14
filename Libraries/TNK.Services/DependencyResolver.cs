using Autofac;
using TNK.Core.Infrastructure;
using TNK.Core.Infrastructure.DependencyManagement;
using TNK.Services.Authentication;
using TNK.Services.Catalog;
using TNK.Services.Configuration;
using TNK.Services.ChiTietBanPhuKiens;
using TNK.Services.ChuyenTien;
using TNK.Services.Common;
using TNK.Services.CTKM;
using TNK.Services.GopVon;
using TNK.Services.KhoXe;
using TNK.Services.LichSuThaoTac;
using TNK.Services.Manager;
using TNK.Services.Menus;
using TNK.Services.NhanVien;
using TNK.Services.No;
using TNK.Services.Report;
using TNK.Services.Roles;
using TNK.Services.Security;
using TNK.Services.SoChi;
using TNK.Services.SoThu;
using TNK.Services.TreoTiens;
using TNK.Services.Users;
using TNK.Services.SoQuyetToan;

namespace TNK.Services
{
    public class DependencyResolver : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<UserRegistrationService>().As<IUserRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserervice>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ConfigService>().As<IConfigService>().InstancePerLifetimeScope();
            builder.RegisterType<SoThuService>().As<ISoThuService>().InstancePerLifetimeScope();
            builder.RegisterType<PhieuThuService>().As<IPhieuThuService>().InstancePerLifetimeScope();
            builder.RegisterType<SoChiService>().As<ISoChiService>().InstancePerLifetimeScope();
            builder.RegisterType<CommonService>().As<ICommonService>().InstancePerLifetimeScope();
            builder.RegisterType<Log.Logger>().As<Log.ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<NoPhaiThuService>().As<INoPhaiThuService>().InstancePerLifetimeScope();
            builder.RegisterType<NoPhaiTraService>().As<INoPhaiTraService>().InstancePerLifetimeScope();
            builder.RegisterType<ChiTietKhuyenMaiService>().As<IChiTietKhuyenMaiService>().InstancePerLifetimeScope();
            builder.RegisterType<ChiTietNhapKhoPhuTungService>().As<IChiTietNhapKhoPhuTungService>().InstancePerLifetimeScope();
            builder.RegisterType<TuiDinhKhoanService>().As<ITuiDinhKhoanService>().InstancePerLifetimeScope();
            builder.RegisterType<GiaoDichTuiTienService>().As<IGiaoDichTuiTienService>().InstancePerLifetimeScope();
            builder.RegisterType<LoaiXeService>().As<ILoaiXeService>().InstancePerLifetimeScope();
            builder.RegisterType<KhoXeService>().As<IKhoXeService>().InstancePerLifetimeScope();
            builder.RegisterType<KhoTaiSanService>().As<IKhoTaiSanService>().InstancePerLifetimeScope();
            builder.RegisterType<KhoPhuTungService>().As<IKhoPhuTungService>().InstancePerLifetimeScope();
            builder.RegisterType<TreoTienService>().As<ITreoTienService>().InstancePerLifetimeScope();
            builder.RegisterType<ChuyenTienService>().As<IChuyenTienService>().InstancePerLifetimeScope();           
            builder.RegisterType<TKKNService>().As<ITKKNService>().InstancePerLifetimeScope();
            builder.RegisterType<DoiTacService>().As<IDoiTacService>().InstancePerLifetimeScope();
            builder.RegisterType<ChiTietBanPhuKienService>().As<IChiTietBanPhuKienService>().InstancePerLifetimeScope();
            builder.RegisterType<SoKetChuyenService>().As<ISoKetChuyenService>().InstancePerLifetimeScope();
            builder.RegisterType<NhanVienService>().As<INhanVienService>().InstancePerLifetimeScope();
            builder.RegisterType<LichSuThaoTacService>().As<ILichSuThaoTacService>().InstancePerLifetimeScope();
            builder.RegisterType<LoggingService>().As<ILoggingService>().InstancePerLifetimeScope();
            builder.RegisterType<GopVonService>().As<IGopVonService>().InstancePerLifetimeScope();
            builder.RegisterType<SoQuyetToanService>().As<ISoQuyetToanService>().InstancePerLifetimeScope();
        }
        





        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
}

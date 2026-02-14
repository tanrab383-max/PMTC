using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNK.Services.Authentication;
using TNK.Services.LichSuThaoTac;

namespace TNK.Controllers
{
    public class LichSuThaoTacController : BasePublicController
    {
      //  int total = 0;
      //  int pageSize = 30;
        private ILichSuThaoTacService _LichSuThaoTacServiceService;
      //  private HttpContextBase _httpContext;
        private IAuthenticationService _authenticationService;
        public LichSuThaoTacController(ILichSuThaoTacService _LichSuThaoTacServiceService,
            HttpContextBase _httpContext,
            IAuthenticationService _authenticationService) : base()
        {
            this._LichSuThaoTacServiceService = _LichSuThaoTacServiceService;
            this._httpContext = _httpContext;
            this._authenticationService = _authenticationService;
        }
        // GET: LichSuThaoTac
        public ActionResult Index(Guid userid = new Guid(), string from = "", string to = "",string tukhoa = "")
        {
            //DateTime f = Convert.ToDateTime(from);
            //DateTime t = Convert.ToDateTime(to);
            //ViewBag.Title = "Danh sách log lỗi";
            ////var list = _LichSuThaoTacServiceService.GetLichSuThaoTac(userid, f, t, tukhoa);
            //ViewBag.Total = list.Count;
            return View();
        }

        protected override void InvokeAction()
        {
            //this._soChiService.SetIPClient(GetIPClient());
            // this._soChiService.SetHostNameClient(GetHostNameClient());
            base.InvokeAction();

        }
    }
}
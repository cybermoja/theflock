using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMS.Web.Helper.Attributes;

namespace FMS.Web.Controllers
{
    [FMSAuthorizeAttribute]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

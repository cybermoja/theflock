using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMS.Web.Helper.Attributes;

namespace FMS.Web.Controllers
{
    [FMSAuthorizeAttribute]
    public class SearchController : BaseController
    {
        public ActionResult Index(string q = "", string origin = "")
        {
            ViewBag.Query = q;
            ViewBag.Origin = origin;
            return View();
        }
    }
}

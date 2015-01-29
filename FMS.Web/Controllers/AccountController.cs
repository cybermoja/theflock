using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMS.Web.Helper.Attributes;
using FMS.Web.Helper.Security;
using FMS.Web.Model;
using System.Web.Security;
using FMS.Service;
using FMS.Web.Helper;
using FMS.Web.Helper.Extensions;
using FMS.Data;

namespace FMS.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [NoCache]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Current.User = null;
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [NoCache]
        public ActionResult Login(string ReturnUrl = "")
        {

            ViewBag.ReturnUrl = ReturnUrl;

            var crypto = new SimpleAes();
            Session["BPKey"] = AesForLogin.Create16DigitString();
            Session["BPKeyIV"] = AesForLogin.Create16DigitString();
            var cookie1 = new HttpCookie("tokenBP1");
            var cookie2 = new HttpCookie("tokenBP2");
            cookie1.Value = crypto.EncryptToString(Session["BPKey"].ToString());
            cookie2.Value = crypto.EncryptToString(Session["BPKeyIV"].ToString());

            Response.Cookies.Remove("tokenBP1");
            Response.Cookies.Remove("tokenBP2");

            Response.Cookies.Add(cookie1);
            Response.Cookies.Add(cookie2);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult _Login(LoginViewModel model)
        {

            DecryptLogin(model);
            ModelState.Clear();

            model = new LoginViewModel
            {
                Password = "test",
                RememberMe = true,
                UserName = "markymark"
            };

            TryValidateModel(model);
            if (ModelState.IsValid)
            {

                var user = new User
                {
                    Groups = "1,2",
                    ID = 1,
                    MemberId = 12343,
                    Roles = "1,2",
                    UserName = "markymark",
                };
                    
                    //_userService.GetLoginDetail(model.UserName, model.Password.ToMdfive());
                if (user != null)
                {

                    Current.User = user;
                    Current.Membership = new Member()
                    {
                        FirstName = "Mark",
                        LastName = "Garcia",
                        Office = MemberOffices.Secretary
                    };

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return Json(new { code = 0 });
                }
            }

            return Json(new { code = -100, message = "The user name or password provided is incorrect." });
        }

        private void DecryptLogin(LoginViewModel model)
        {
            model.UserName = string.Empty;
            model.Password = string.Empty;

            var key = Request.Cookies["tokenBP1"].Value;
            var iv = Request.Cookies["tokenBP2"].Value;
            if (key != null && iv != null)
            {
                try
                {
                    var crypto = new SimpleAes();
                    key = crypto.DecryptString(key);
                    iv = crypto.DecryptString(iv);
                    model.UserName = AesForLogin.DecryptStringAES(key, iv, model.Log1);
                    model.Password = AesForLogin.DecryptStringAES(key, iv, model.Log2);
                }
                catch (Exception)
                {
                    model.Password = string.Empty;
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            else
            {
                model.Password = string.Empty;
                ModelState.AddModelError("", "Your Session is expire please try login again");
            }
        }
    }
}

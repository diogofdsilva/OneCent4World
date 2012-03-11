using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using _1CW_Site.Models;
using _1CW_Site.Visitor;
using OCW.BL;

namespace _1CW_Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly OCWBusinessLayer businessLayer;

        public AccountController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginProfile = businessLayer.Login(model.Email, model.Password);

                if (loginProfile != null)
                {
                    FormsService.SignIn(model.Email, model.RememberMe);

                    TypeVisitor visit = new TypeVisitor();
                    visit.Visit(loginProfile);

                    this.Response.Cookies.Add(new HttpCookie("UserFullName", loginProfile.Email));
                    this.Response.Cookies.Add(new HttpCookie("ProfileType", visit.TypeOfProfile));
                    

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("MyAccount", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public ActionResult LogOnIndex(LogonRegisterContainerModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginProfile = businessLayer.Login(model.LogOn.Email, model.LogOn.Password);

                if (loginProfile != null)
                {
                    FormsService.SignIn(model.LogOn.Email, model.LogOn.RememberMe);

                    this.Response.Cookies.Add(new HttpCookie("UserFullName",loginProfile.Email));

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("MyAccount", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View("LogOn", model.LogOn);
        }


        [HttpPost]
        public ActionResult RegisterIndex(LogonRegisterContainerModel model)
        {
            PersonModel profileModel = new PersonModel();

            profileModel.Email = model.Register.Email;
            profileModel.FirstName = model.Register.Name;
            profileModel.LastName = model.Register.Surename;
            
            ViewBag.ListCountry = businessLayer.ListAllCountries();
            
            return View("../Profile/Create",profileModel);
        }


        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            //this.Response.Cookies.Clear();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

       
        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        [Authorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}

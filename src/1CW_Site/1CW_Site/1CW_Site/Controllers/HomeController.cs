using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _1CW_Site.Models;
using Ninject.Modules;
using OCW.BL;
using OCW.DAL.DTOs;

namespace _1CW_Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly OCWBusinessLayer businessLayer;

        private const int HISTORY_PAGE_SIZE = 10;

        public HomeController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }


        [Authorize]
        public ActionResult MyAccount()
        {
            string userEmail = User.Identity.Name;

            var profile = businessLayer.GetProfileWithAccount(userEmail);

            IEnumerable<Movement> movements = businessLayer.GetMovements(profile.Id).OrderByDescending(m => m.DateMovement).Take(10);

            decimal donations = businessLayer.GetTransactionsbyId(profile.Id).Sum(t => t.Donation);

            ViewBag.Transactions = movements;

            ViewBag.Balance = profile.Account.Value;
            
            ViewBag.ProfileName = profile.ProfileName;

            ViewBag.ProfileType = "Person";

            ViewBag.TotalDonationsValue = donations * 100;

            return View();
        }

        [Authorize]
        public ActionResult History(int page)
        {
            string userEmail = User.Identity.Name;

            var profile = businessLayer.GetProfile(userEmail);

            IEnumerable<Movement> transactions = businessLayer.GetMovements(profile.Id).OrderByDescending(t => t.DateMovement);

            ViewBag.Pages = transactions.Count() / HISTORY_PAGE_SIZE;
            if (transactions.Count() % HISTORY_PAGE_SIZE != 0) ++ViewBag.Pages;

            ViewBag.Page = page;

            ViewBag.StartPage = (ViewBag.Page - 5) < 1 ? (dynamic) 1 : (ViewBag.Page - 5);
            ViewBag.EndPage = (ViewBag.Page + 5) > ViewBag.Pages ? ViewBag.Pages : (ViewBag.Page + 5);

            ViewBag.Transactions = transactions.Skip((page - 1) * HISTORY_PAGE_SIZE).Take(HISTORY_PAGE_SIZE);

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Index(RegisterModel rm)
        {
            if (this.User.Identity.IsAuthenticated)
                return RedirectToAction("MyAccount", "Home");
            
            return View();
        }

        
    }

    public class NServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<OCWBusinessLayer>().To<OCWBusinessLayer>();
        }
    }
}

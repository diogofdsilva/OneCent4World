using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _1CW_Site.Models;
using OCW.BL;
using OCW.DAL.DTOs;

namespace _1CW_Site.Controllers
{
    public class DepositController : Controller
    {
        
        private readonly OCWBusinessLayer businessLayer;

        private const int DEPOSIT_PAGE_SIZE = 10;

        public DepositController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        [Authorize]
        public ActionResult List(int page)
        {
            IEnumerable<Deposit> deposits = businessLayer.GetDepositList(User.Identity.Name).OrderByDescending(t => t.DateMovement);

            ViewBag.Pages = deposits.Count() / DEPOSIT_PAGE_SIZE;
            if (deposits.Count() % DEPOSIT_PAGE_SIZE != 0) ++ViewBag.Pages;

            ViewBag.Page = page;

            ViewBag.StartPage = (ViewBag.Page - 5) < 1 ? (dynamic)1 : (ViewBag.Page - 5);
            ViewBag.EndPage = (ViewBag.Page + 5) > ViewBag.Pages ? ViewBag.Pages : (ViewBag.Page + 5);

            deposits = deposits.Skip((page - 1) * DEPOSIT_PAGE_SIZE).Take(DEPOSIT_PAGE_SIZE);

            return View(deposits.Select(o => new DepositModel
                                             {
                                                 Date = o.Date,
                                                 Id = o.Id,
                                                 Source = o.Source,
                                                 Value = o.Amount
                                             }));
        }

        //
        // GET: /Deposit/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreditCard()
        {
            ViewBag.CardTypes = new[] {"American Express", "Diner's Club", "Discover", "MasterCard", "VISA"};

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreditCard(CreditCardModel model)
        {
            businessLayer.Deposit(User.Identity.Name, model.Value ,model.Issuer);

            return RedirectToAction("List", new { page = 1 });
        }

        [Authorize]
        [HttpPost]
        public ActionResult TestDeposit(DepositModel model)
        {
            businessLayer.Deposit(User.Identity.Name, model.Value,"manual");

            return RedirectToAction("List", new { page = 1 });
        }
        

    }
}

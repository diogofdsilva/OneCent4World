using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _1CW_Site.Models;
using OCW.BL;
using OCW.DAL.DTOs;

namespace _1CW_Site.Controllers
{
    public class WithdrawController : Controller
    {
                
        private readonly OCWBusinessLayer businessLayer;

        private const int WITHDRAW_PAGE_SIZE = 10;

        public WithdrawController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult List(int page)
        {
            IEnumerable<Withdraw> withdraws = businessLayer.GetWithdrawList(User.Identity.Name).OrderByDescending(t => t.DateMovement);

            ViewBag.Pages = withdraws.Count() / WITHDRAW_PAGE_SIZE;
            if (withdraws.Count() % WITHDRAW_PAGE_SIZE != 0) ++ViewBag.Pages;

            ViewBag.Page = page;

            ViewBag.StartPage = (ViewBag.Page - 5) < 1 ? (dynamic)1 : (ViewBag.Page - 5);
            ViewBag.EndPage = (ViewBag.Page + 5) > ViewBag.Pages ? ViewBag.Pages : (ViewBag.Page + 5);

            withdraws = withdraws.Skip((page - 1) * WITHDRAW_PAGE_SIZE).Take(WITHDRAW_PAGE_SIZE);

            return View(withdraws.Select(o => new WithdrawModel()
            {
                Date = o.Date,
                Id = o.Id,
                Destination = o.Destination,
                Value = o.Amount
            }));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(decimal Value)
        {
            businessLayer.Withdraw(businessLayer.GetProfile(User.Identity.Name).Id, Value, "-");

            return RedirectToAction("List", new { page = 1 });
        }

        [HttpGet]
        [Authorize]
        public ActionResult Check()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Check(CheckModel model)
        {
            businessLayer.Withdraw(businessLayer.GetProfile(User.Identity.Name).Id, model.Value, "Check");

            return RedirectToAction("List", new { page = 1 });
        }
    }
}

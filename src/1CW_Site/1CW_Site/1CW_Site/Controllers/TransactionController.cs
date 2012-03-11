using System;
using System.Linq;
using System.Web.Mvc;
using _1CW_Site.Models;
using OCW.BL;
using OCW.DAL.DTOs;

namespace _1CW_Site.Controllers
{
    public class TransactionController : Controller
    {
        private readonly OCWBusinessLayer businessLayer;

        public TransactionController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        //
        // GET: /Transaction/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            var company = businessLayer.GetCompany(id);

            string username = HttpContext.User.Identity.Name;
            var person = businessLayer.GetPerson(username);
            Organization organization = businessLayer.GetOrganizationFromCompany(company.Id);

            var organizations = new SelectList(businessLayer.GetAllOrganizations(), "Id", "Name");
            ViewBag.Organizations = organizations;

            var valueString = this.Request.QueryString["value"];
            decimal value = 0;
            if(valueString != null)
            {
                value = Decimal.Parse(valueString);
            }

            var reference = this.Request.QueryString["reference"];

            TransactionModel transactionModel = new TransactionModel
                                                    {
                                                        CompanyId = company.Id,
                                                        CompanyName = company.Name,
                                                        DonationAmount = OCWBusinessLayer.VALOR_CONTRIBUICAO,
                                                        PersonName = string.Format("{0} {1}",person.FirstName,person.LastName),
                                                        PaidAmount = value,
                                                        OrganizationName = organization.Name,
                                                        OrganizationId = organization.Id,
                                                        CompanyHasImage = company.Image != null && company.Image.Length > 0,
                                                        OrganizationHasImage = organization.Image != null && organization.Image.Length > 0,
                                                        Reference = reference
                                                    };
            

            return View(transactionModel);
        } 

        //
        // POST: /Transaction/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(TransactionModel model)
        {
            try
            {
                model.Date = DateTime.Now;

                var personId = businessLayer.GetPerson(User.Identity.Name).Id;

                businessLayer.Transfer(personId, model.CompanyId, model.OrganizationId, model.PaidAmount, model.DonationAmount, model.Reference);

                return RedirectToAction("MyAccount","Home");
            }
            catch(Exception e)
            {
                return Create(model.CompanyId);
            }
        }


        
        //
        // GET: /Transaction/List
        [Authorize]
        public ActionResult List()
        {
            string username = HttpContext.User.Identity.Name; 

            var x = businessLayer.GetPersonTransactions(username).Select(o => new TransactionModel()
                                                                               {
                                                                                   Date = o.Date,
                                                                                   DonationAmount = o.Donation,
                                                                                   CompanyName = o.Company.Name,
                                                                                   CompanyId = o.Company_id,
                                                                                   OrganizationName = o.Organization.Name,
                                                                                   PaidAmount = o.Value,
                                                                                   PersonName = string.Format("{0} {1}", o.Person.FirstName, o.Person.LastName)
                                                                               });


            return View(x);
        }

        

    }
}

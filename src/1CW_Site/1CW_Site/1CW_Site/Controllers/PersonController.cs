using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _1CW_Site.Models;
using OCW.BL;
using OCW.DAL.DTOs;

namespace _1CW_Site.Controllers
{
    public class PersonController : Controller
    {
        
        private readonly OCWBusinessLayer businessLayer;

        public PersonController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        //
        // GET: /Profile/Details

        public ActionResult Details()
        {
            string name = HttpContext.User.Identity.Name;

            Person v = businessLayer.GetPersonFullInfo(name);

            return View(new PersonModel
                            {
                                Id = v.Id,
                                Address1 = v.Address.Address1,
                                Address2 = v.Address.Address2,
                                City = v.Address.City,
                                Country = v.Address.Country.Name,
                                PostalCode = v.Address.PostalCode,
                                Email = v.Email,
                                FirstName = v.FirstName,
                                LastName = v.LastName,
                                BirthDate = v.Birthdate
                            });
        }

        //
        // GET: /Profile/Create

        public ActionResult Create()
        {
            ViewBag.ListCountry = businessLayer.ListAllCountries();

            return View();
        } 

        //
        // POST: /Profile/Create

        [HttpPost]
        public ActionResult Create(PersonModel model)
        {
            try
            {
                businessLayer.CreatePerson(model.FirstName,model.LastName,model.Email,model.Password,
                    model.Address1,model.Address2,model.BirthDate,model.PostalCode,model.City, model.Country ,"user");

                IFormsAuthenticationService FormsService = new FormsAuthenticationService();
                FormsService.SignIn(model.Email, false);

                Response.Cookies["UserFullName"].Value = model.FirstName + " " + model.LastName;

                return RedirectToAction("MyAccount","Home");
            }
            catch
            {
                ModelState.AddModelError("", "ERROR");
                ViewBag.ListCountry = businessLayer.ListAllCountries();
                return View(model);
            }
        }
        
        //
        // GET: /Profile/Edit
 
        public ActionResult Edit()
        {
            IEnumerable<Country> countries = businessLayer.GetAllCountries();
            Dictionary<int, string> countriesDictionary = new Dictionary<int, string>(countries.Count());
            foreach (Country country in countries)
                countriesDictionary.Add(country.Id, country.Name);

            ViewBag.Countries = countriesDictionary;

            string email = HttpContext.User.Identity.Name;

            Person v = businessLayer.GetPersonFullInfo(email);

            return View(new PersonModel
            {
                Address1 = v.Address.Address1,
                Address2 = v.Address.Address2,
                CountryId = v.Address.Country_id,
                City = v.Address.City,
                Email = v.Email,
                FirstName = v.FirstName,
                LastName = v.LastName,
                RegisteredDate = v.RegisteredDate,
                PostalCode = v.Address.PostalCode
            });
        }

        //
        // POST: /Profile/Edit

        [HttpPost]
        public ActionResult Edit(PersonModel model)
        {
            try
            {
                string email = HttpContext.User.Identity.Name;

                int userId = businessLayer.GetProfile(email).Id;

                businessLayer.EditPerson(userId, model.FirstName, model.LastName, model.Email, model.Address1, model.Address2,
                                             model.PostalCode, model.City, model.CountryId, model.BirthDate);

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }
    }
}

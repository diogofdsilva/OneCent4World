using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1CW_Site.ActionResults;
using OCW.DAL.DTOs;
using _1CW_Site.Models;
using OCW.BL;
using OCW.DAL.Exceptions;

namespace _1CW_Site.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/

        private readonly OCWBusinessLayer businessLayer;

        private const int IMAGE_MAX_WIDTH = 200;


        public CompanyController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        public ActionResult ShowCompanyImage(int id)
        {
            Company c;
            try
            {
                 c = businessLayer.GetCompany(id);
            } catch(RecordNotFoundException<int>)
            {
                return null;
            }

            byte[] imageArray = c.Image;

            if (imageArray == null || imageArray.Length == 0)
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\ecommerce.png";
                Image image = Image.FromFile(file);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                imageArray = ms.ToArray();
            }
            return new ImageResult(imageArray, @"image/png");
        }

        //
        // GET: /Company/Details/
        [Authorize]
        public ActionResult Details()
        {
            string userEmail = User.Identity.Name;
            Company c = businessLayer.GetCompanyFullInfo(userEmail);

            return View(new CompanyModel()
                {
                    Name = c.Name,
                    Address1 = c.Address.Address1,
                    Address2 = c.Address.Address2,
                    City = c.Address.City,
                    Country = c.Address.Country.Name,
                    CountryId = c.Address.Country_id,
                    Email = c.Email,
                    Url = c.Url,
                    PostalCode = c.Address.PostalCode,
                    Tags = c.Tag.Select(tag => tag.Name),
                    Id = c.Id
                });
        }

        //
        // GET: /Company/Create

        public ActionResult Create()
        {
            IEnumerable<Country> countries = businessLayer.GetAllCountries();
            Dictionary<int, string> countriesDictionary = new Dictionary<int, string>(countries.Count());
            foreach (Country country in countries)
                countriesDictionary.Add(country.Id, country.Name);

            ViewBag.Countries = countriesDictionary;

            IEnumerable<Tag> tags = businessLayer.GetAllTags();
            Dictionary<int, string> tagsDictionary = new Dictionary<int, string>(tags.Count());
            foreach (Tag type in tags)
                tagsDictionary.Add(type.Id, type.Name);

            ViewBag.Tags = tagsDictionary;

            return View();
        } 

        //
        // POST: /Company/Create

        [HttpPost]
        public ActionResult Create(CompanyModel model)
        {
            try
            {
                byte[] tempImage;
                
                HttpPostedFileBase file = Request.Files["CompanyImage"];
                if(file != null)
                {
                    Image img = Image.FromStream(file.InputStream);

                    int width = img.Width;
                    int height = img.Height;

                    if (width > IMAGE_MAX_WIDTH)
                    {
                        width = IMAGE_MAX_WIDTH;
                        height = (int)((float)height * width / img.Width);
                    }
                    img = img.GetThumbnailImage(width, height, null, IntPtr.Zero);

                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Png);
                    tempImage = ms.ToArray();
                }
                else
                {
                    tempImage = new byte[]{};
                }

                List<int> tags = new List<int>();
                string[] tagsCheckBoxes;
                if ((tagsCheckBoxes = Request.Form.GetValues("tags")) != null)

                    foreach (var tag in tagsCheckBoxes)
                    {
                        int tagId;
                        int.TryParse(tag, out tagId);
                        tags.Add(tagId);
                    }

                businessLayer.CreateCompany(model.Name, model.Url, tempImage, model.Email, model.Password, model.Address1, model.Address2,
                                             model.PostalCode, model.City, model.CountryId, tags);

                return RedirectToAction("MyAccount", "Home");
            }
            catch
            {
                ViewBag.ListCountry = businessLayer.ListAllCountries();
                return View();
            }
        }
        
        //
        // GET: /Company/Edit
        [Authorize]
        public ActionResult Edit()
        {
            string userEmail = User.Identity.Name;
            Company c = businessLayer.GetCompanyFullInfo(userEmail);

            IEnumerable<Country> countries = businessLayer.GetAllCountries();
            Dictionary<int, string> countriesDictionary = new Dictionary<int, string>(countries.Count());
            foreach (Country country in countries)
                countriesDictionary.Add(country.Id, country.Name);

            ViewBag.Countries = countriesDictionary;

            IEnumerable<Tag> tags = businessLayer.GetAllTags();
            Dictionary<int, string> tagsDictionary = new Dictionary<int, string>(tags.Count());
            foreach (Tag type in tags)
                tagsDictionary.Add(type.Id, type.Name);

            ViewBag.Tags = tagsDictionary;

            return View(new CompanyModel
            {
                Name = c.Name,
                Address1 = c.Address.Address1,
                Address2 = c.Address.Address2,
                City = c.Address.City,
                CountryId = c.Address.Country_id,
                Email = c.Email,
                Url = c.Url,
                PostalCode = c.Address.PostalCode,
                TagsIds = c.Tag.Select(tag => tag.Id)
            });
        }

        //
        // POST: /Company/Edit

        [HttpPost]
        [Authorize]
        public ActionResult Edit(CompanyModel model)
        {
            try
            {
                byte[] tempImage = null;
                
                HttpPostedFileBase file = Request.Files["CompanyImage"];
                if(file != null)
                {
                    Image img = Image.FromStream(file.InputStream);

                    int width = img.Width;
                    int height = img.Height;

                    if (width > IMAGE_MAX_WIDTH)
                    {
                        width = IMAGE_MAX_WIDTH;
                        height = (int)((float)height * width / img.Width);
                    }
                    img = img.GetThumbnailImage(width, height, null, IntPtr.Zero);

                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Png);
                    tempImage = ms.ToArray();
                }

                List<int> tags = new List<int>();
                string[] tagsCheckBoxes;
                if ((tagsCheckBoxes = Request.Form.GetValues("tags")) != null)

                    foreach (var tag in tagsCheckBoxes)
                    {
                        int tagId;
                        int.TryParse(tag, out tagId);
                        tags.Add(tagId);
                    }

                string userEmail = User.Identity.Name;

                int userId = businessLayer.GetProfile(userEmail).Id;

                businessLayer.EditCompany(userId, model.Name, model.Url, tempImage, model.Email, model.Address1, model.Address2,
                                             model.PostalCode, model.City, model.CountryId, tags);

                return RedirectToAction("Details");
            }
            catch(Exception e)
            {
                return View();
            }
        }

        //
        // GET: /Company/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            businessLayer.DeleteCompany(id);
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1CW_Site.ActionResults;
using _1CW_Site.Models;
using OCW.BL;
using OCW.DAL.DTOs;
using OCW.DAL.Exceptions;

namespace _1CW_Site.Controllers
{
    public class ONGController : Controller
    {
        private readonly OCWBusinessLayer businessLayer;

        private const int IMAGE_MAX_WIDTH = 200;

        public ONGController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        [HttpGet]
        public ActionResult ShowOrganizationImage(int id)
        {
            Organization o;
            try
            {
                o = businessLayer.GetOrganization(id);
            }
            catch (RecordNotFoundException<int>)
            {
                return null;
            }

            byte[] imageArray = o.Image;

            if (imageArray == null || imageArray.Length == 0)
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\charity.png";
                Image image = Image.FromFile(file);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                imageArray = ms.ToArray();
            }
            return new ImageResult(imageArray, @"image/png");
        }

        // GET: /ONG/Details

        public ActionResult Details()
        {
            string userEmail = User.Identity.Name;
            Organization o = businessLayer.GetOrganizationFullInfo(userEmail);

            return View(new ONGModel
                            {
                                Address1 = o.Address.Address1,
                                Address2 = o.Address.Address2,
                                City = o.Address.City,
                                CountryName = o.Address.Country.Name,
                                Email = userEmail,
                                Name = o.Name,
                                PostalCode = o.Address.PostalCode,
                                Url = o.Url,
                                Tags = o.Tag.Select(t => t.Name),
                                Id = o.Id
                            });
        }

        //
        // GET: /ONG/Create

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
        // POST: /ONG/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                byte[] tempImage;

                HttpPostedFileBase file = Request.Files["OngImage"];
                if (file != null)
                {
                    Image img = Image.FromStream(file.InputStream);

                    int width = img.Width;
                    int height = img.Height;
                    
                    if(width > IMAGE_MAX_WIDTH)
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
                    tempImage = new byte[] { };
                }

                List<int> tags = new List<int>();

                if (Request.Form.GetValues("tags") != null)

                    foreach (var tag in Request.Form.GetValues("tags"))
                    {
                        int tagId;
                        int.TryParse(tag, out tagId);
                        tags.Add(tagId);
                    }

                businessLayer.CreateOrganization(
                    collection["Name"],
                    collection["Url"],
                    tempImage,
                    collection["Email"],
                    collection["Password"],
                    collection["address1"],
                    collection["address2"],
                    collection["Postalcode"],
                    collection["City"],
                    int.Parse(collection["Country"]),
                    tags);

                return RedirectToAction("LogOn", "Account");
            }
            catch(Exception e)
            {
                return View();
            }
        }
        
        //
        // GET: /ONG/Edit
 
        public ActionResult Edit()
        {
            string userEmail = User.Identity.Name;
            Organization o = businessLayer.GetOrganizationFullInfo(userEmail);

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

            return View(new ONGModel
            {
                Name = o.Name,
                Address1 = o.Address.Address1,
                Address2 = o.Address.Address2,
                City = o.Address.City,
                CountryId = o.Address.Country_id,
                Email = o.Email,
                Url = o.Url,
                PostalCode = o.Address.PostalCode,
                TagsIds = o.Tag.Select(tag => tag.Id)
            });
        }

        //
        // POST: /ONG/Edit

        [HttpPost]
        [Authorize]
        public ActionResult Edit(ONGModel model)
        {
            try
            {
                byte[] tempImage = null;

                HttpPostedFileBase file = Request.Files["OngImage"];
                if (file != null)
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

                businessLayer.EditOrganization(userId, model.Name, model.Url, tempImage, model.Email, model.Address1, model.Address2,
                                             model.PostalCode, model.City, model.CountryId, tags);

                return RedirectToAction("Details");
            }
            catch (Exception e)
            {
                return View();
            }
        }

    }
}

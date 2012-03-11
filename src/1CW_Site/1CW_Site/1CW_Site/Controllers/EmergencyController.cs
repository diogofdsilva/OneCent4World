using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1CW_Site.ActionResults;
using _1CW_Site.Models;
using OCW.BL;
using OCW.DAL.DTOs;
using OCW.DAL.Exceptions;
using OCW.DAL.IndependentLayerEntities;

namespace _1CW_Site.Controllers
{
    public class EmergencyController : Controller
    {
        private readonly OCWBusinessLayer businessLayer;

        public EmergencyController(OCWBusinessLayer businessLayer)
        {
            this.businessLayer = businessLayer;
        }

        //
        // GET: /Emergency/

        [Authorize]
        [HttpGet]
        public ActionResult List(int page)
        {
            return View(businessLayer.GetEmergencyWithOrganization(10, page).Select(model => new EmergencyModel()
                                                                                {
                                                                                    Title = model.Title,
                                                                                    Description = model.Description,
                                                                                    StartDate = model.StartDate,
                                                                                    EndDate = model.EndDate,
                                                                                    Weight = model.Weight,
                                                                                    OrganizationName = model.Organization.Name,
                                                                                    Id = model.Id
                                                                                })
                );
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            IEnumerable<IndependentOrganization> organizations = businessLayer.ListAllOrganizationsIndependentEntities();
            Dictionary<int, string> organizationsDictionary = new Dictionary<int, string>(organizations.Count());
            foreach (IndependentOrganization io in organizations)
            {
                organizationsDictionary.Add(io.Id, io.Name);
            }

            ViewBag.Organizations = organizationsDictionary;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(EmergencyModel model)
        {
            try
            {
                byte[] tempImage;

                HttpPostedFileBase file = Request.Files["Image"];
                if (file != null)
                {
                    Int32 length = file.ContentLength;
                    tempImage = new byte[length];
                    file.InputStream.Read(tempImage, 0, length);
                }
                else
                {
                    tempImage = new byte[] { };
                }

                businessLayer.CreateEmergency(model.Title, model.Description, model.StartDate, model.EndDate,
                                              model.Weight, tempImage, model.Organization);

                return RedirectToAction("List", new { page = 1});
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Emergency e = businessLayer.GetEmergency(id);

            IEnumerable<Organization> organizations = businessLayer.GetAllOrganizations();
            Dictionary<int, string> organizationsDictionary = new Dictionary<int, string>(organizations.Count());
            foreach (Organization io in organizations)
            {
                organizationsDictionary.Add(io.Id, io.Name);
            }

            ViewBag.Organizations = organizationsDictionary;

            return View(new EmergencyModel
                            {
                                Id = e.Id,
                                Title = e.Title,
                                Description = e.Description,
                                StartDate = e.StartDate,
                                EndDate = e.EndDate,
                                Weight = e.Weight,
                                Organization = e.Organization_id,
                            });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EmergencyModel model)
        {
            try
            {
                byte[] tempImage;

                HttpPostedFileBase file = Request.Files["Image"];
                if (file != null)
                {
                    Int32 length = file.ContentLength;
                    tempImage = new byte[length];
                    file.InputStream.Read(tempImage, 0, length);
                }
                else
                {
                    tempImage = null;
                }

                businessLayer.EditEmergency(model.Id, model.Title, model.Description, model.StartDate, model.EndDate,
                                              model.Weight, tempImage, model.Organization);

                return RedirectToAction("List", new { page = 1 });
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            businessLayer.DeleteEmergency(id);

            return RedirectToAction("List", new { page = 1 });
        }

        public ActionResult Active()
        {
            IEnumerable<Emergency> emergencies = businessLayer.GetActiveEmergencies();

            return PartialView(emergencies.Select(o => new EmergencyModel
                                                    {
                                                        Description = o.Description,
                                                        EndDate = o.EndDate,
                                                        Organization = o.Organization_id,
                                                        StartDate = o.StartDate,
                                                        Title = o.Title,
                                                        Weight = o.Weight,
                                                        Id= o.Id,
                                                        HasImage = o.Image != null && o.Image.Length > 0
                                                    }));
        }

        [HttpGet]
        public ActionResult ShowImage(int id)
        {
            Emergency e;
            try
            {
                e = businessLayer.GetEmergency(id);
            }
            catch (RecordNotFoundException<int>)
            {
                return null;
            }
            return new ImageResult(e.Image, @"image/png");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OCW.DAL.DTOs;

namespace _1CW_Site.Models
{
    public class EmergencyModel
    {
        public bool HasImage { get; set; }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime? EndDate { get; set; }
        
        [Required]
        [Display(Name = "Weight")]
        public decimal Weight { get; set;}

        [Required]
        [Display(Name = "Organization")]
        public int Organization { get; set; }

        public string OrganizationName { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }
    }
}
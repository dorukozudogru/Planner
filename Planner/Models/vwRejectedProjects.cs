using System;
using System.ComponentModel.DataAnnotations;

namespace Planner.Models
{
    public class vwRejectedProjects
    {
        public string ProjectId { get; set; }

        [Display(Name = "Proje Adı")]
        public string ProjectName { get; set; }

        [Display(Name = "Proje Açıklaması")]
        public string ProjectDescription { get; set; }

        [Display(Name = "Ret Sebebi")]
        public string RejectCause { get; set; }

        [Display(Name = "Ret Tarihi")]
        public DateTime RejectDate { get; set; }
    }
}
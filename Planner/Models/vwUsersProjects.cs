using System;
using System.ComponentModel.DataAnnotations;

namespace Planner.Models
{
    public class vwUsersProjects
    {
        public string UserId { get; set; }

        [Display(Name = "Adı")]
        public string UserName { get; set; }

        [Display(Name = "Soyadı")]
        public string UserSurname { get; set; }

        [Display(Name = "E-Posta")]
        public string UserEMail { get; set; }

        [Display(Name = "T.C. Kimlik No")]
        public string UserCitizenshipNo { get; set; }

        public string ProjectId { get; set; }

        [Display(Name = "Proje Adı")]
        public string ProjectName { get; set; }

        [Display(Name = "Proje Açıklaması")]
        public string ProjectDescription { get; set; }

        public string FileName { get; set; }

        [Display(Name = "Onaylandı Mı?")]
        public int IsApproved { get; set; }

        [Display(Name = "Desteklendi Mi?")]
        public int IsSupported { get; set; }

        public string Supporter { get; set; }

        public string SupportRequirements { get; set; }

        public string SupportValue { get; set; }

        public DateTime SupportDate { get; set; }
    }
}
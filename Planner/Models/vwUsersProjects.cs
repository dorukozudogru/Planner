using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    public class vwUsersProjects
    {
        public int UserId { get; set; }

        [Display(Name = "Adı")]
        public string UserName { get; set; }

        [Display(Name = "Soyadı")]
        public string UserSurname { get; set; }

        [Display(Name = "E-Posta")]
        public string UserEMail { get; set; }

        [Display(Name = "T.C. Kimlik No")]
        public string UserCitizenshipNo { get; set; }

        public int ProjectId { get; set; }

        [Display(Name = "Proje Adı")]
        public string ProjectName { get; set; }

        public string FileName { get; set; }

        [Display(Name = "Onaylandı Mı?")]
        public int IsApproved { get; set; }

        [Display(Name = "Desteklendi Mi?")]
        public int IsSupported { get; set; }
    }
}
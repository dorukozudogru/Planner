using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Models
{
    [Table("User")]
    public partial class User
    {
        public string Id { get; set; }

        [Display (Name = "E-Posta")]
        [Required]
        [StringLength(80)]
        public string EMail { get; set; }

        [Display(Name = "Ad")]
        [StringLength(80)]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        [StringLength(100)]
        public string Surname { get; set; }

        [Display(Name = "Þifre")]
        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [Display(Name = "Doðum Tarihi")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "T.C. Kimlik Numarasý")]
        [StringLength(11)]
        public string CitizenshipNo { get; set; }

        public bool IsCvUploaded { get; set; }

        public int IsApproved { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastEditDate { get; set; }

        public int LastEditBy { get; set; }
    }
}

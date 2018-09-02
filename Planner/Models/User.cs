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
        [StringLength(80)]
        [Required]
        public string EMail { get; set; }

        [Display(Name = "Ad")]
        [StringLength(80)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        [StringLength(100)]
        [Required]
        public string Surname { get; set; }

        [Display(Name = "�ifre")]
        [StringLength(30)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Do�um Tarihi")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Telefon")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "T.C. Kimlik Numaras�")]
        [StringLength(11)]
        [Required]
        public string CitizenshipNo { get; set; }

        [Display(Name = "�ehir")]
        [StringLength(100)]
        public string City { get; set; }

        [Display(Name = "Okul")]
        [StringLength(100)]
        public string School { get; set; }

        [Display(Name = "B�l�m")]
        [StringLength(100)]
        public string Department { get; set; }

        [Display(Name = "Meslek")]
        [StringLength(100)]
        public string Job { get; set; }

        public bool IsFirm { get; set; }

        public bool IsCvUploaded { get; set; }

        public bool IsEmailVerified { get; set; }

        public int IsApproved { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastEditDate { get; set; }

        public string LastEditBy { get; set; }
    }
}

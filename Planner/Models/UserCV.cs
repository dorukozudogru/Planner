using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Models
{
    [Table("UserCV")]
    public partial class UserCV
    {
        public int Id { get; set; }

        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Display(Name = "CV Adı")]
        public string FileName { get; set; }

        [Display(Name = "Dosya Yolu")]
        public string FilePath { get; set; }

        [Display(Name = "Eklenme Tarihi")]
        public DateTime CreationDate { get; set; }
    }
}
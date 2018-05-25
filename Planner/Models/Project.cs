using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Models
{
    [Table("Project")]
    public partial class Project
    {
        public int Id { get; set; }

        [Display(Name = "Proje Adı")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Proje Açıklaması")]
        [Required]
        public string Description { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public long? FileSize { get; set; }

        public string FileExtension { get; set; }

        [Display(Name = "Onaylandı Mı?")]
        public int IsApproved { get; set; }

        [Display(Name = "Desteklendi Mi?")]
        public int IsSupported { get; set; }

        public string SupportMount { get; set; }

        public int IsApproveChanged { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastEditDate { get; set; }

        public int LastEditBy { get; set; }
    }
}
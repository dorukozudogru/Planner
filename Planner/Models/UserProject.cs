using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Models
{
    [Table("UserProject")]
    public partial class UserProject
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ProjectId { get; set; }

        [Display(Name = "Onaylandý Mý?")]
        public int IsApproved { get; set; }

        [Display(Name = "Desteklendi Mi?")]
        public int IsSupported { get; set; }

        public string SupportRequest { get; set; }

        [Display(Name = "Onayý Deðiþtirildi Mi?")]
        public int IsApproveChanged { get; set; }
    }
}

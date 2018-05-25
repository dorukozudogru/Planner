namespace Planner.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserProject")]
    public partial class UserProject
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        [Display(Name = "Onaylandý Mý?")]
        public int IsApproved { get; set; }

        [Display(Name = "Desteklendi Mi?")]
        public int IsSupported { get; set; }

        public string SupportMount { get; set; }

        [Display(Name = "Onayý Deðiþtirildi Mi?")]
        public int IsApproveChanged { get; set; }

        //public virtual UserProject UserProject1 { get; set; }

        //public virtual UserProject UserProject2 { get; set; }
    }
}

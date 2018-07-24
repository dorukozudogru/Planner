using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    [Table("SupportedProjects")]
    public partial class SupportedProjects
    {
        public int Id { get; set; }

        public string SupportedProjectId { get; set; }

        public string SupporterUserId { get; set; }

        public string SupportRequirements { get; set; }

        public string SupportValue { get; set; }

        public DateTime SupportDate { get; set; }
    }
}
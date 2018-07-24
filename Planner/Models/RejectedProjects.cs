using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    [Table("RejectedProjects")]
    public partial class RejectedProjects
    {
        public int Id { get; set; }

        public string RejectedProjectId { get; set; }

        public string RejectUserId { get; set; }

        public string RejectCause { get; set; }

        public DateTime RejectDate { get; set; }
    }
}
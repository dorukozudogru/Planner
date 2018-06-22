using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Models
{
    [Table("ProjectUserAuthorize")]
    public class ProjectUserAuthorize
    {
        public int Id { get; set; }

        public string ProjectId { get; set; }

        public string AuthorizedUserId { get; set; }

        public DateTime LastEditDate { get; set; }

        public string LastEditBy { get; set; }
    }
}
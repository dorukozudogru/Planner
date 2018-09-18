using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    public class vwAuthorizedUserProject
    {
        public string ProjectId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        public string UserEMail { get; set; }

        public bool UserIsAdmin { get; set; }
    }
}
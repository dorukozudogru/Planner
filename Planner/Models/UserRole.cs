using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int RoleId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public string RoleDefinition { get; set; }

        public string RoleFunctions { get; set; }
    }
}
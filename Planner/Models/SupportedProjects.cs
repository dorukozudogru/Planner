using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    public class SupportedProjects
    {
        public int Id { get; set; }

        public string SupportedProject { get; set; }

        public string Supporter { get; set; }

        public string SupportRequirements { get; set; }

        public string SupportValue { get; set; }

        public DateTime SupportDate { get; set; }
    }
}
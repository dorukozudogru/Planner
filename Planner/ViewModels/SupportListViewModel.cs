using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.ViewModels
{
    public class SupportListViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SupportRequirements { get; set; }
        public string SupportValue { get; set; }
        public DateTime? SupportDate { get; set; }
    }
}
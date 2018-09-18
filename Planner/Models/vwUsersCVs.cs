using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    public class vwUsersCVs
    {
        public string UserId { get; set; }

        public string EMail { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string CitizenshipNo { get; set; }

        public int UserCVId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAT_ELAC.Models
{
    public class Student
    {
        public int MathId { get; set; }
        public int EnglishId { get; set; }
        public IEnumerable<SelectListItem> MathTests { get; set; }
        public IEnumerable<SelectListItem> EnglishTests { get; set; }

        public List<TakenReview> Tests { get; set; }

        public string EnglishPlacement { get; set; }
        public string MathPlacement { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class AdminStudentInfo : UserProfile
    {
        public string EnglishPlacement { get; set; }
        public string MathPlacement { get; set; } 
    }

    public class TakenReview
    {
        public String TestDescription { get; set; }
        public TakenTest Test { get; set; }
    }
}

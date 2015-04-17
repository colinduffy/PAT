using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace PAT_ELAC.Models
{
    public class QuestionContext : DbContext
    {
        public QuestionContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<QuestionModel> Questions { get; set; }
    }

    [Table("Question")]
    public class QuestionModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        [Display(Name = "Topic")]
        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }
 
    }

    public class QM
    {
        public int total { get; set; }
        public int correct { get; set; }
        public QuestionModel question { get; set; }
        public double percent { get; set; }
        public Topic topic { get; set; }
        public QM(int c, int t, QuestionModel q, Topic to)
        {
            correct = c;
            total = t;
            percent = (double)correct / (double)total * 100.0;

            question = q;
            topic = to;
        }
    }

    
    public class QuestionCreateModel
    {
        [Required]
        [Display(Name = "Topic")]
        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }

        [MinLength(1)]
        public AnswerModel[] Answers { get; set; }
        public IEnumerable<SelectListItem> Topics { get; set; }
        public int Answer { get; set; }
     
    }

}
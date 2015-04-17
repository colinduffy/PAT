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

/**
 * AnsweredQuestions stores info for questions answered by users, such as which 
 * question, user, test, and what answer they picked.
 */
namespace PAT_ELAC.Models
{
    public class AnsweredQuestionsContext : DbContext
    {
        public AnsweredQuestionsContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<AnsweredQuestionsModel> AnsweredQuestions { get; set; }
    }

    [Table("AnsweredQuestions")]
    public class AnsweredQuestionsModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        
        [Column("takenId")]
        public int takenId { get; set; }
        [Column("questionId")]
        public int questionId { get; set; }
        [Column("choice")]
        public int choice { get; set; }

    }


}
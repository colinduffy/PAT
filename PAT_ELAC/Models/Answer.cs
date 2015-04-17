using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;


namespace PAT_ELAC.Models
{
    public class AnswerContext : DbContext
    {
        public AnswerContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<AnswerModel> Answers { get; set; }
    }

    [Table("Answer")]
    public class AnswerModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace PAT_ELAC.Models
{
    public class TopicContext : DbContext
    {
        public TopicContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Topic> Topics { get; set; }
    }

    [Table("Topics")]
    public class Topic
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        //TODO: Change this list for Resources model
        //public List<Resource> resources { get; set; }
        //public List<QuestionModel> questions { get; set; }

    }

    public class TopicCreateModel
    {
        public int key { get; set; }
        public string description { get; set; }
        public List<Resource> resources { get; set; }
    }
}
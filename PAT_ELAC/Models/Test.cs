using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAT_ELAC.Models
{
    public class TestContext : DbContext
    {
        public TestContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Test> Tests { get; set; }

    }

    public class TestTopicsContext : DbContext
    { 
        public TestTopicsContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<TestTopics> TestTopics { get; set; }
    }

    public class SubjectContext : DbContext
    {
        public SubjectContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Subject> Subjects { get; set; }
    }


    public class TakenContext : DbContext
    {
        public TakenContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<TakenTest> TakenTests { get; set; }
    }

    [Table("Test")]
    public class Test
    {
        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TestId { get; set; }

        [Range(0, 10)]
        public int Level { get; set; }


        [Required]
        [Range(1, 100)]
        public int Threshold { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public String Description { get; set; }


        public IEnumerable<SelectListItem> Subjects { get; set; }
        public IEnumerable<SelectListItem> Topics { get; set; }

        [MinLength(1)]
        public TestTopics[] TestTopic { get; set; }

       

    //IEnumerable<TestTopics>

    }

    public class TestTopics
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TopicId { get; set; }
        public int Quantity { get; set; }
        public int TestId { get; set; }

        [Range(1, 100)]
        public int QuestionValue { get; set; }
    }

    public class TestCreateModel
    {
        public int TestId { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public Dictionary<int, string> level { get; set; }
        public List<Topic> topics { get; set; }
    }

    [Table("Subject")]
    public class Subject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SubjectId { get; set; }

        public string Description { get; set; }       
    }

    public class TestQuestion
    {
        
        public QuestionModel Question {get; set;}
        public AnswerModel[] Answers {get; set;}

        public int Value { get; set; }
        //public int SelectedAnswerId;
    }

    public class TakeTest
    {
        public int TestId { get; set; }
        public int Threshold { get; set; }
        public TestQuestion[] Questions {get; set;}
        public int[] Answers { get; set; }
    }

    [Table("TakenTests")]
    public class TakenTest
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TakenId { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public bool Passed { get; set; }
    }

    public class ResourceModel
    {
        public List<String> Videos {get; set;}
        public TakenTest TestResult {get; set;}
        public List<Topic> Topics {get; set;}

    }
}
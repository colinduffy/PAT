using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 * Abstract class for holding resources such as videos, links, and practice problems
 * Distinct resource types should inherit this type and its properties
 * @author Peter Benzoni
 * */
namespace PAT_ELAC.Models
{
    public class ResourceContext : DbContext
    {
        public ResourceContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Resource> Resources { get; set; }

    }

    public class Test2Context : DbContext
    {
        public Test2Context()
            : base("DBConnection")
        {
        }

        public DbSet<String> Values { get; set; }

    }


    public class Resource
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResourceId { get; set; }

        [Required]
        [Display(Name = "Topic")]
        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Resource Link")]
        public string Link { get; set; }

        public IEnumerable<SelectListItem> Topics { get; set; }
    }
}
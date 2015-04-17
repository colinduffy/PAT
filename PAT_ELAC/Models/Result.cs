using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAT_ELAC.Models
{
    public class Result
    {
        public int key { get; set; }
        public string userKey{ get; set; }
        public string testKey { get; set; }
        public List<Dictionary<QuestionModel, bool>> level { get; set; }
    }
}
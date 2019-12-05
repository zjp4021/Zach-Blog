using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zach_Blog.Models
{
    public class BlogPost
    {
            public BlogPost()
        {
            Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public string Title { get; set; }
        public string Slug { get; set; }
        
        [AllowHtml]
        public string Abstract { get; set; }
        public string BlogPostBody { get; set; }
        public string ImagePath { get; set; }
        public bool Published { get; set; }
        
        //Navigation
        public virtual ICollection<Comment> Comments { get; set; }
     
        //public BlogPost() 
        //{
        //    this.Comments = new HashSet<Comment>();

        //}



    }
}
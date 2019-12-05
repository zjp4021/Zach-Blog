using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zach_Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int BlogPostId { get; set; }
        public string AuthorId { get; set; }
        public DateTime Created { get; set; }
        public string CommentBody { get; set; }
        public DateTime? Updated { get; set; } 
        public string UpdateReason { get; set; }


        //Virtual Navigation section
        public virtual BlogPost BlogPost { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public object Body { get; internal set; }
    }
}
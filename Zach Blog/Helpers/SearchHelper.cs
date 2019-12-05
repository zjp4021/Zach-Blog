using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zach_Blog.Models;

namespace Zach_Blog.Helpers
{
    public class SearchHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            var result = db.Posts.AsNoTracking().AsQueryable(); 
            if (searchStr != null)
            { 
                result = result.Where(p => p.Title.Contains(searchStr) || 
                p.BlogPostBody.Contains(searchStr) || 
                p.Comments.Any(c => c.CommentBody.Contains(searchStr) || 
                c.Author.FirstName.Contains(searchStr) || 
                c.Author.LastName.Contains(searchStr) || 
                c.Author.DisplayName.Contains(searchStr) || 
                c.Author.Email.Contains(searchStr)));
            }
        
            
            return result.OrderByDescending(p => p.Created);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zach_Blog.Models;
using Zach_Blog.Helpers;
using PagedList;
using PagedList.Mvc;

namespace Zach_Blog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var bloglist = SearchHelper.IndexSearch(searchStr);

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(bloglist.ToPagedList(pageNumber, pageSize));
        }

        //{
        //    var publishedBlogPosts = db.Posts.Where(b => b.Published).ToList();
        //    return View(publishedBlogPosts);
        //}
    

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emailTo = ConfigurationManager.AppSettings["emailfrom"];
                   
                    var from = $"{model.FromEmail}<{emailTo}>";

                    var email = new MailMessage(from, emailTo)
                    {
                        Subject = model.Subject,
                        Body = $"You have received an email from your Blog <br /> {model.Body}",
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }


    };
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zach_Blog.Helpers;
using Zach_Blog.Models;
using PagedList;
using PagedList.Mvc;

namespace Zach_Blog.Controllers
{
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var bloglist = SearchHelper.IndexSearch(searchStr);
            
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            
            return View(bloglist.ToPagedList(pageNumber, pageSize));
        }


        // GET: BlogPosts/Details/5
        public ActionResult Details(string Slug)
        {
            
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.FirstOrDefault(p => p.Slug == Slug);
            
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize(Roles="Admin")]
        
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Title,Abstract,BlogPostBody,ImagePath,Published")] BlogPost blogPost, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                #region Slug
                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if(db.Posts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }
                #endregion

                #region Picture Upload
                if (ImageUploadValidator.IsWebFriendlyImage(picture))
                {
                    var fileName = Path.GetFileName(picture.FileName);

                    fileName = StringUtilities.URLFriendly(Path.GetFileNameWithoutExtension(fileName));
                    fileName += "_" + DateTime.Now.Ticks + Path.GetExtension(picture.FileName);

                    picture.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.ImagePath = "/Uploads/" + fileName;
                }
                #endregion

                blogPost.Slug = Slug;
                blogPost.Created = DateTime.Now;
                db.Posts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        
        public ActionResult Edit([Bind(Include = "Id,Title,Abstract,Slug,BlogPostBody,ImagePath,Published,Created")] BlogPost blogPost, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {

                #region Picture Upload
                if (ImageUploadValidator.IsWebFriendlyImage(picture))
                {
                    var fileName = Path.GetFileName(picture.FileName);

                    fileName = StringUtilities.URLFriendly(Path.GetFileNameWithoutExtension(fileName));
                    fileName += "_" + DateTime.Now.Ticks + Path.GetExtension(picture.FileName);

                    picture.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.ImagePath = "/Uploads/" + fileName;
                }
                #endregion


                blogPost.Updated = DateTime.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","BlogPosts");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

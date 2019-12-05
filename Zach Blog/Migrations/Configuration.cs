namespace Zach_Blog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Zach_Blog.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Zach_Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Zach_Blog.Models.ApplicationDbContext context)
        {
            #region
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin")) 
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            if (!context.Roles.Any(r => r.Name == "Authenticated user")) 
            {
                roleManager.Create(new IdentityRole { Name = "Authenticated user" });
            }

            if (!context.Roles.Any(r => r.Name == "Anonymous user"))
            {
                roleManager.Create(new IdentityRole { Name = "Anonymous user" });
            }
#endregion

            #region User Creation
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "zjp4021@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "zjp4021@gmail.com",
                    Email = "zjp4021@gmail.com",
                    FirstName = "Zach",
                    LastName = "Pruitt",
                    DisplayName = "ZP"
                }, "Abc&123");
            }
            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "EpicMod"
                }, "Abc&123");
            }
            #endregion

            #region Assign roles to Users

            var adminId = userManager.FindByEmail("zjp4021@gmail.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var moderatorId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(moderatorId, "Moderator");
            #endregion

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
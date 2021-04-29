using KJ_API_Projekt.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KJ_API_Projekt.data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GeoMessages> geoMessages { get; set; }


        public async Task Seed(UserManager<MyUser> userManager)
        {
            await Database.MigrateAsync();

            MyUser admin1 = new MyUser() { FirstName = "Kristopher", LastName = "Kram", Email = "test1@hotmail.com", EmailConfirmed = true, UserName = "admin1" };
            MyUser admin2 = new MyUser() { FirstName = "Jakob", LastName = "Larsson", Email = "test2@hotmail.com", EmailConfirmed = true, UserName = "admin2" };

            await userManager.CreateAsync(admin1, "Passw0rd!");
            await userManager.CreateAsync(admin2, "Passw0rd!");

            await SaveChangesAsync();
        }
    }
}

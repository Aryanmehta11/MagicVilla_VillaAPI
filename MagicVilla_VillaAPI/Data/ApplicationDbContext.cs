using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        }
        
        public DbSet<Villa> Villas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Garden ",
                    ImgUrl = "aa.com",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                    Details="It is near Vasai"
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Nalasopara Garden ",
                    ImgUrl = "ur.com",
                    Occupancy = 2,
                    Rate = 2,
                    Sqft = 500,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                    Details = "It is near the best place on Planet Earth"

                },
                new Villa()
                {
                    Id = 3,
                    Name = "Matunga Garden ",
                    ImgUrl = "urmatunga.com",
                    Occupancy = 6,
                    Rate = 250,
                    Sqft = 500,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                    Details = "It is near the best place on Planet Earth after Nalasopara"

                });
        }

    }
}

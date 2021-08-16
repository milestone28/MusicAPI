using Microsoft.EntityFrameworkCore;
using MusicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext>options) : base(options)
        {

        }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Album> Albums { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Song>().HasData(
                
        //        new Song
        //        {
        //            Id = 1,
        //            Title = "This is Title song",
        //                                Duration = "4:35",
        //        },

        //         new Song
        //         {
        //             Id = 2,
        //             Title = "This is Title song 2",
        //             Duration = "4:35",
        //         }


        //        );
        //}
    }
}

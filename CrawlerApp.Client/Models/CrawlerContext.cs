using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlerApp.Client.Models
{    
    public class CrawlerContext : DbContext
    {
        public CrawlerContext(DbContextOptions<CrawlerContext> options) : base(options)
        {
        }

        public DbSet<Link> Links { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Link>().HasKey(vf => new { vf.ID, vf.Address, vf.FoundOn });
        //}
    }    
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlerApp.Client.Models
{    
    public sealed class CrawlerContext : DbContext
    {
        public CrawlerContext(DbContextOptions<CrawlerContext> options) : base(options)
        {
        }

        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Link>(entity => {
                entity.HasIndex(e => new{e.Address, e.FoundOn}).IsUnique();
            });
        }
    }    
}

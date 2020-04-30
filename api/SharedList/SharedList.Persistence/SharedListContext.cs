using Microsoft.EntityFrameworkCore;
using System;
using SharedList.Persistence.Models.Entities;

namespace SharedList.Persistence
{
    public class SharedListContext : DbContext
    {
        public SharedListContext(DbContextOptions<SharedListContext> options) : base(options)
        {

        }

        public DbSet<List> Lists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<List>().ToTable("List");
            modelBuilder.Entity<ListItem>().ToTable("ListItem");
        }
    }
}

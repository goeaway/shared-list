using Microsoft.EntityFrameworkCore;
using System;
using SharedList.Core.Models.Entities;
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
        public DbSet<ListContributor> ListContributors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<List>().ToTable("List");
            modelBuilder.Entity<ListItem>().ToTable("ListItem");
            modelBuilder.Entity<ListContributor>()
                .ToTable("ListContributor")
                .HasKey(table => new {table.ListId, table.UserIdent});
        }
    }
}

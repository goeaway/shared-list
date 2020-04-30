﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedList.Persistence;

namespace SharedList.Migrations.Migrations
{
    [DbContext(typeof(SharedListContext))]
    partial class SharedListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SharedList.Persistence.Models.Entities.List", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("Updated");

                    b.HasKey("Id");

                    b.ToTable("List");
                });

            modelBuilder.Entity("SharedList.Persistence.Models.Entities.ListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Completed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Notes");

                    b.Property<string>("ParentListId");

                    b.Property<DateTime?>("Updated");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ParentListId");

                    b.ToTable("ListItem");
                });

            modelBuilder.Entity("SharedList.Persistence.Models.Entities.ListItem", b =>
                {
                    b.HasOne("SharedList.Persistence.Models.Entities.List", "ParentList")
                        .WithMany("Items")
                        .HasForeignKey("ParentListId");
                });
#pragma warning restore 612, 618
        }
    }
}

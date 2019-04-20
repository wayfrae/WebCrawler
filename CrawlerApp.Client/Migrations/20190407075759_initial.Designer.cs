﻿// <auto-generated />
using System;
using CrawlerApp.Client.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrawlerApp.Client.Migrations
{
    [DbContext(typeof(CrawlerContext))]
    [Migration("20190407075759_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CrawlerApp.Client.Models.Link", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime>("Date");

                    b.Property<string>("FoundOn");

                    b.Property<bool>("IsCrawled");

                    b.Property<string>("Response");

                    b.HasKey("ID");

                    b.ToTable("Links");
                });
#pragma warning restore 612, 618
        }
    }
}
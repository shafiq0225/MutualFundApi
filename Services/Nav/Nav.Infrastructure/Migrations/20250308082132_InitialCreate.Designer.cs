﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nav.Infrastructure.Persistence;

#nullable disable

namespace Nav.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250308082132_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Nav.Domain.Entities.Fund", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("FundId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("FundVisible")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SchemeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SchemeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("SchemeVisible")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Funds");
                });
#pragma warning restore 612, 618
        }
    }
}

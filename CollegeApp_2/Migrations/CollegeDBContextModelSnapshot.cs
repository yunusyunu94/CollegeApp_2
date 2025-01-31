﻿// <auto-generated />
using System;
using CollegeApp_2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CollegeApp_2.Migrations
{
    [DbContext(typeof(CollegeDBContext))]
    partial class CollegeDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CollegeApp_2.Data.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Departments", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DepartmentName = "ECE",
                            Description = "ECE Departmen"
                        },
                        new
                        {
                            Id = 2,
                            DepartmentName = "CSE",
                            Description = "CSE Departmen"
                        });
                });

            modelBuilder.Entity("CollegeApp_2.Data.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adres")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Students", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Adres = "Hyd, INDIA",
                            DOB = new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "yunus@hotmail.com",
                            StudentName = "Yunus"
                        },
                        new
                        {
                            Id = 2,
                            Adres = "Banglore, INDIA",
                            DOB = new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "Anil@hotmail.com",
                            StudentName = "Anil"
                        });
                });

            modelBuilder.Entity("CollegeApp_2.Data.Student", b =>
                {
                    b.HasOne("CollegeApp_2.Data.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentId")
                        .HasConstraintName("FK_Students_Department");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("CollegeApp_2.Data.Department", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}

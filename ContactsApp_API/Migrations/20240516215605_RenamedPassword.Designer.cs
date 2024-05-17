﻿// <auto-generated />
using System;
using ContactsApp_API.Data;
using ContactsApp_API.Data.ContactsContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactsApp_API.Migrations
{
    [DbContext(typeof(ContactsDbContext))]
    [Migration("20240516215605_RenamedPassword")]
    partial class RenamedPassword
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ContactsApp_API.Data.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ContactsApp_API.Data.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("BusinessSubcategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid>("MainCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OtherSubcategory")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessSubcategoryId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("MainCategoryId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("ContactsApp_API.Data.Contact", b =>
                {
                    b.HasOne("ContactsApp_API.Data.Category", "BusinessSubcategory")
                        .WithMany()
                        .HasForeignKey("BusinessSubcategoryId");

                    b.HasOne("ContactsApp_API.Data.Category", "MainCategory")
                        .WithMany()
                        .HasForeignKey("MainCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusinessSubcategory");

                    b.Navigation("MainCategory");
                });
#pragma warning restore 612, 618
        }
    }
}

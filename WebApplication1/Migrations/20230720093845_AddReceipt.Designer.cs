﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Data;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20230720093845_AddReceipt")]
    partial class AddReceipt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Data.Book", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("WebApplication1.Data.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("WebApplication1.Data.FullReceipt", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReceiptId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("discountAmount")
                        .HasColumnType("float");

                    b.Property<float>("discountFloat")
                        .HasColumnType("real");

                    b.Property<double>("singlePrice")
                        .HasColumnType("float");

                    b.Property<int>("totalNumber")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "ReceiptId");

                    b.HasIndex("ReceiptId");

                    b.ToTable("FullReceipt", (string)null);
                });

            modelBuilder.Entity("WebApplication1.Data.Receipt", b =>
                {
                    b.Property<Guid>("ReceiptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CompleteDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReceiptId");

                    b.ToTable("Receipt", (string)null);
                });

            modelBuilder.Entity("WebApplication1.Data.Book", b =>
                {
                    b.HasOne("WebApplication1.Data.Category", "Category")
                        .WithMany("BookList")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("WebApplication1.Data.FullReceipt", b =>
                {
                    b.HasOne("WebApplication1.Data.Book", "Book")
                        .WithMany("FullReceiptList")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_FullReceipt_Book");

                    b.HasOne("WebApplication1.Data.Receipt", "Receipt")
                        .WithMany("FullReceiptList")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_FullReceipt_Receipt");

                    b.Navigation("Book");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("WebApplication1.Data.Book", b =>
                {
                    b.Navigation("FullReceiptList");
                });

            modelBuilder.Entity("WebApplication1.Data.Category", b =>
                {
                    b.Navigation("BookList");
                });

            modelBuilder.Entity("WebApplication1.Data.Receipt", b =>
                {
                    b.Navigation("FullReceiptList");
                });
#pragma warning restore 612, 618
        }
    }
}

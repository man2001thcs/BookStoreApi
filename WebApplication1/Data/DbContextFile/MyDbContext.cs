﻿using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Authentication;
using WebApplication1.Data.RelationData;

namespace WebApplication1.Data.DbContextFile
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSet
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FullReceipt> FullReceiptList { get; set; }
        public DbSet<Receipt> ReceiptList { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable(nameof(Receipt));
                entity.HasKey(receipt => receipt.ReceiptId);
                entity.Property(receipt => receipt.CreatedDate).IsRequired().HasDefaultValueSql("getutcdate()");
                entity.Property(receipt => receipt.UserId).IsRequired();
            });

            modelBuilder.Entity<BookStatus>(entity =>
            {
                entity.ToTable(nameof(BookStatus));
                entity.HasKey(bookStatus => bookStatus.ProductId);
                entity.Property(bookStatus => bookStatus.UpdateDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable(nameof(Voucher));
                entity.HasKey(voucher => voucher.Id);
                entity.Property(voucher => voucher.UpdateDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<FullReceipt>(entity =>
            {
                entity.ToTable("FullReceipt");
                entity.HasKey(e => new { e.ProductId, e.ReceiptId });

                entity.HasOne(e => e.Receipt)
                .WithMany(e => e.FullReceiptList)
                .HasForeignKey(e => e.ReceiptId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_FullReceipt_Receipt");

                entity.HasOne(e => e.Book)
                .WithMany(e => e.FullReceiptList)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_FullReceipt_Book");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.HasOne(e => e.User)
                .WithMany(e => e.ReceiptList)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_User_Receipt");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasOne(e => e.BookStatus)
                .WithOne(e => e.Book)
                .HasForeignKey<BookStatus>(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Book_BookStatus");
            });

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.ToTable(nameof(BookAuthor));
                entity.HasKey(ba => new { ba.BookId, ba.AuthorId });

                entity.HasOne(ba => ba.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(ba => ba.BookId)
                .HasConstraintName("FK_BookAuthor_Book");

                entity.HasOne(ba => ba.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .HasConstraintName("FK_BookAuthor_Author");
            });

            modelBuilder.Entity<BookCategory>(entity =>
            {
                entity.ToTable(nameof(BookCategory));
                entity.HasKey(e => new { e.BookId, e.CategoryId });

                entity.HasOne(e => e.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(e => e.BookId)
                .HasConstraintName("FK_BookCategory_Book");

                entity.HasOne(e => e.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(e => e.CategoryId)
                .HasConstraintName("FK_BookCategory_Category");
            });

            modelBuilder.Entity<BookPublisher>(entity =>
            {
                entity.ToTable(nameof(BookPublisher));
                entity.HasKey(e => new { e.BookId, e.PublisherId });

                entity.HasOne(e => e.Book)
                .WithMany(b => b.BookPublishers)
                .HasForeignKey(e => e.BookId)
                .HasConstraintName("FK_BookPublisher_Book");

                entity.HasOne(e => e.Publisher)
                .WithMany(c => c.BookPublishers)
                .HasForeignKey(e => e.PublisherId)
                .HasConstraintName("FK_BookPublisher_Publisher");
            });

            modelBuilder.Entity<BookVoucher>(entity =>
            {
                entity.ToTable(nameof(BookVoucher));
                entity.HasKey(e => new { e.BookId, e.VoucherId });

                entity.HasOne(e => e.Book)
                .WithMany(b => b.BookVouchers)
                .HasForeignKey(e => e.BookId)
                .HasConstraintName("FK_BookVoucher_Book");

                entity.HasOne(e => e.Voucher)
                .WithMany(c => c.BookVouchers)
                .HasForeignKey(e => e.VoucherId)
                .HasConstraintName("FK_BookVoucher_Voucher");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable(nameof(RefreshToken));

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                .WithMany(e => e.RefreshTokenList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_RefreshToken");
            });
        }
    }
}

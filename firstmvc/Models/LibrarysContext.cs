using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace firstmvc.Models;

public partial class LibrarysContext : DbContext
{
    public LibrarysContext()
    {
    }

    public LibrarysContext(DbContextOptions<LibrarysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; }

    public virtual DbSet<UserLib> UserLibs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\MSSQLSERVER01;Initial Catalog=Librarys;Integrated Security=True;Trust Server Certificate=true;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C207DFF92E36");

            entity.Property(e => e.Isbn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BorrowedBook>(entity =>
        {
            entity.HasKey(e => e.BorrowedId).HasName("PK__Borrowed__565E302C061CAABB");

            entity.Property(e => e.BorrowDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowedBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_BorrowedBook_BookId");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowedBooks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_BorrowedBook_UserId");
        });

        modelBuilder.Entity<UserLib>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserLibs__1788CC4C192DF096");

            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Book).WithMany(p => p.UserLibs)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_UserLib_bookId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

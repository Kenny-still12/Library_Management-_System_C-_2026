using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Models
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static LibraryDbContext Create()
        {
            return new LibraryDbContext();
        }

        public DbSet<Library> Libraries { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<BorrowingTransaction> BorrowingTransactions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BorrowingTransaction>()
                .HasRequired(bt => bt.Book)
                .WithMany(b => b.BorrowingTransactions)
                .HasForeignKey(bt => bt.BookId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BorrowingTransaction>()
                .HasRequired(bt => bt.Member)
                .WithMany(m => m.BorrowingTransactions)
                .HasForeignKey(bt => bt.MemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BorrowingTransaction>()
                .Property(b => b.FineAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Feedback>()
                .HasRequired(f => f.Book)
                .WithMany(b => b.Feedbacks)
                .HasForeignKey(f => f.BookId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Feedback>()
                .HasRequired(f => f.Member)
                .WithMany(m => m.Feedbacks)
                .HasForeignKey(f => f.MemberId)
                .WillCascadeOnDelete(false);
        }
    }
}
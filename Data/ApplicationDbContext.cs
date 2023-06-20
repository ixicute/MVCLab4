using Lab4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Book_Author_RT> BookAuthorsRT { get; set; }
        public DbSet<Customer_Book_RT> CustomerBooksRT { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Ignore<IdentityUserRole<int>>();
            builder.Ignore<IdentityUserToken<int>>();

            builder.Entity<User>()
                .HasOne(u => u.Addresses)
                .WithMany(a => a.Users)
                .HasForeignKey(u => u.FK_AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne(u => u.Roles)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.FK_RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book_Author_RT>()
                .HasOne(ba => ba.Books)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.FK_BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book_Author_RT>()
                .HasOne(ba => ba.Authors)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.FK_AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
                .HasOne(b => b.Availabilities)
                .WithMany(av => av.Books)
                .HasForeignKey(b => b.FK_AvailabilityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Customer_Book_RT>()
                .HasOne(cb => cb.Books)
                .WithMany(b => b.CustomerBooks)
                .HasForeignKey(cb => cb.FK_BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Customer_Book_RT>()
                .HasOne(cb => cb.Users)
                .WithMany(u => u.CustomerBooks)
                .HasForeignKey(cb => cb.FK_CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineAuction.DAL.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

public class OnlineAuctionDbContext : IdentityDbContext<ApplicationUser>
{
    public OnlineAuctionDbContext(DbContextOptions<OnlineAuctionDbContext> options)
        : base(options)
    {
    }

    // DbSet properties for entities
    public DbSet<Product> Products { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var userID1 = Guid.NewGuid().ToString();
        var userID2 = Guid.NewGuid().ToString();
        var userID3 = Guid.NewGuid().ToString();
        var userID4 = Guid.NewGuid().ToString();

        var hasher = new PasswordHasher<ApplicationUser>();

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Name = "Admin",
                UserName = "admin@test.com",
                NormalizedUserName = "admin@test.com".ToUpper(),
                Id = userID1,
                Email = "admin@test.com",
                NormalizedEmail = "admin@test.com".ToUpper(),
                PasswordHash = hasher.HashPassword(null, "Pass@123")
            },
                   new ApplicationUser
                   {
                       Name = "User1",
                       UserName = "user1@test.com",
                       NormalizedUserName = "user1@test.com".ToUpper(),
                       Id = userID2,
                       Email = "user1@test.com",
                       NormalizedEmail = "user1@test.com".ToUpper(),
                       PasswordHash = hasher.HashPassword(null, "Pass@123")
                   },
                    new ApplicationUser
                    {
                        Name = "User2",
                        UserName = "user2@test.com",
                        NormalizedUserName = "user2@test.com".ToUpper(),
                        Id = userID3,
                        Email = "user2@test.com",
                        NormalizedEmail = "user2@test.com".ToUpper(),
                        PasswordHash = hasher.HashPassword(null, "Pass@123")
                    },
                     new ApplicationUser
                     {
                         Name = "User3",
                         UserName = "user3@test.com",
                         NormalizedUserName = "user3@test.com".ToUpper(),
                         Id = userID4,
                         Email = "user3@test.com",
                         NormalizedEmail = "user3@test.com".ToUpper(),
                         PasswordHash = hasher.HashPassword(null, "Pass@123")
                     }
            );

      
        builder.Entity<IdentityRole>().HasData(
       new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
       new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
   );


        builder.Entity<IdentityUserRole<string>>().HasData(
        new IdentityUserRole<string> { UserId = userID1, RoleId = "1" },
        new IdentityUserRole<string> { UserId = userID2, RoleId = "2" },
        new IdentityUserRole<string> { UserId = userID3, RoleId = "2" },
        new IdentityUserRole<string> { UserId = userID4, RoleId = "2" }
       
        );

           ConfigureEntities(builder);
    }

    private void ConfigureEntities(ModelBuilder builder)
    {
        // Configure Product entity
        builder.Entity<Product>()
            .HasKey(p => p.ProductId);

        builder.Entity<Product>()
            .HasOne(p => p.Seller)
            .WithMany()
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
      .Property(b => b.CreatedAt)
      .HasDefaultValueSql("SYSDATETIME()"); // or "SYSDATETIME()" for local time


        // Configure Bid entity
        builder.Entity<Bid>()
            .HasKey(b => b.BidId);

        builder.Entity<Bid>()
            .HasOne(b => b.Product)
            .WithMany(p => p.Bids)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Bid>()
            .HasOne(b => b.Bidder)
            .WithMany()
            .HasForeignKey(b => b.BidderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Bid>()
       .Property(b => b.BidTime)
       .HasDefaultValueSql("SYSDATETIME()"); // or "SYSDATETIME()" for local time

        // Configure Auction entity
        builder.Entity<Auction>()
            .HasKey(a => a.AuctionId);

        builder.Entity<Auction>()
            .HasOne(a => a.Product)
            .WithOne()
            .HasForeignKey<Auction>(a => a.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        

      

       
    }

   
}

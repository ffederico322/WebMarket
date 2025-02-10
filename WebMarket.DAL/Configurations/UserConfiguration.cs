using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMarket.Domain.Entity;

namespace WebMarket.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Login).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        
        builder.HasMany<Order>(x => x.Orders)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany<Cart>(x => x.Carts)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<UserRole>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
                l => l.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)
            );
        
        /*builder.HasData(new List<User>()
        {
            new User()
            {
                Id = 1,
                Login = "Federico322",
                Password = new string("12345"),
                Email = "federico322@gmail.com"    
            }
        });*/
    }
}
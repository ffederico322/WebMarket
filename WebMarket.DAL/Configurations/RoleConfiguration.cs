using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMarket.Domain.Entity;

namespace WebMarket.DAL.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(role => role.Id).ValueGeneratedOnAdd();
        builder.Property(role => role.Name).IsRequired().HasMaxLength(50);
        
        builder.HasData(new List<Role>()
        {
            new Role()
            {
                Id = 1,
                Name = "User" 
            },
            new Role()
            {
                Id = 2,
                Name = "Admin" 
            },
            new Role()
            {
                Id = 3,
                Name = "Moderator" 
            }
            
        });
    }
}
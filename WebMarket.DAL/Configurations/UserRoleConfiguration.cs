﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMarket.Domain.Entity;

namespace WebMarket.DAL.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasData(new List<UserRole>
        {
            new UserRole()
            {
                UserId = 1,
                RoleId = 2
            }
        });
    }
}
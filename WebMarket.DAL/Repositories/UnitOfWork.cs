using Microsoft.EntityFrameworkCore.Storage;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Interfaces.Databases;
using WebMarket.Domain.Interfaces.Repositories;

namespace WebMarket.DAL.Repositories;

public class UnitOfWork(
    ApplicationDbContext dbContext,
    IBaseRepository<User> users,
    IBaseRepository<Role> roles,
    IBaseRepository<UserRole> userRoles) : IUnitOfWork
{

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await dbContext.Database.BeginTransactionAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public IBaseRepository<User> Users { get; } = users ?? throw new ArgumentNullException(nameof(users));
    public IBaseRepository<Role> Roles { get; } = roles ?? throw new ArgumentNullException(nameof(roles));
    public IBaseRepository<UserRole> UserRoles { get; } = userRoles ?? throw new ArgumentNullException(nameof(userRoles));

}
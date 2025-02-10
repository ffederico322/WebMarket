using Microsoft.EntityFrameworkCore.Storage;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Interfaces.Repositories;

namespace WebMarket.Domain.Interfaces.Databases;

public interface IUnitOfWork : IStateSaveChanges
{
    Task<IDbContextTransaction> BeginTransactionAsync();

    IBaseRepository<User> Users { get; }
    
    IBaseRepository<Role> Roles { get; }
    
    IBaseRepository<UserRole> UserRoles { get; }
}








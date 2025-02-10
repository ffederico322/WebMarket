using WebMarket.Domain.Interfaces.Databases;

namespace WebMarket.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> : IStateSaveChanges
{
    IQueryable<TEntity> GetAll();

    Task<TEntity> GetByIdAsync(long id);
    
    Task<TEntity> CreateAsync(TEntity entity);
    
    TEntity Update(TEntity entity);
    
    void Remove(TEntity entity);
}
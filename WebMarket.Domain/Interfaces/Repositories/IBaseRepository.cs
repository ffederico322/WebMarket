namespace WebMarket.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity>
{
    IQueryable<TEntity> GetAll();
    
    Task<TEntity> GetByIdAsync(long id);
    
    Task<TEntity> CreateAsync(TEntity entity);
    
    Task<TEntity> UpdateAsync(TEntity entity);
    
    Task<TEntity> RemoveAsync(TEntity entity);
}
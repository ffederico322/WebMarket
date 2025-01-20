using Microsoft.EntityFrameworkCore;
using WebMarket.Domain.Interfaces.Repositories;

namespace WebMarket.DAL.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : class
{
    public IQueryable<TEntity> GetAll()
    {
        return dbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(long id)
    {
        return await dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");
        
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");
        
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
        
        return entity;
    }

    public async Task<TEntity> RemoveAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");
        
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
        
        return entity;
    }

}
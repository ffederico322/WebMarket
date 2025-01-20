using Microsoft.EntityFrameworkCore;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Interfaces.Repositories;

namespace WebMarket.DAL.Repositories;

public class CategoryRepository(ApplicationDbContext dbcontext) : BaseRepository<Category>(dbcontext), ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext = dbcontext;

    public async Task<bool> ExistsAsync(int categoryId)
    {
        return await _dbContext.Set<Category>().AnyAsync(c => c.Id == categoryId);
    }
}
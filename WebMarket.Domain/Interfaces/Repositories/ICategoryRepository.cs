using WebMarket.Domain.Entity;

namespace WebMarket.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<bool> ExistsAsync(int id);
}
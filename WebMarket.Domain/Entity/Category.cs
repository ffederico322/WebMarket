using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class Category : IEntityId<long>
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Product> Products { get; set; }
}


using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class Product : IEntityId<long>
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public long CategoryId { get; set; }
    
    public Category Category { get; set; }
    
    public string Description { get; set; }
    
    public string Image { get; set; }
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
}


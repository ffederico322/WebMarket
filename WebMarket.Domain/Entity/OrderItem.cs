using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class OrderItem : IEntityId<long>
{
    public long Id { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
    
    public long ProductId { get; set; }
    
    public Product Product { get; set; }
    
    public long OrderId { get; set; }
    
    public Order Order { get; set; }
}
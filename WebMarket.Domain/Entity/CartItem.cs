using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class CartItem : IEntityId<long>
{
    public long Id { get; set; }
    
    public long CartId { get; set; }
    
    public Cart Cart { get; set; }
    
    public long ProductId { get; set; }
    
    public Product Product { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
}
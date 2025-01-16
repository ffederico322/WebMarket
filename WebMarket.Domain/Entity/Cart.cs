using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class Cart : IEntityId<long>
{
    public long Id { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public List<CartItem> CartItems { get; set; }
    
    public long UserId { get; set; }
    
    public User User { get; set; }
    
    public Order Order { get; set; }
}


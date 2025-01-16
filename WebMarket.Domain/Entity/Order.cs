using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class Order : IEntityId<long>, IAuditable
{
    public long Id { get; set; }
    
    public bool IsActive { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public long UserId { get; set; }
    
    public User User { get; set; }

    public long CartId { get; set; }
    
    public Cart Cart { get; set; }
    
    public List<OrderItem> OrderItems { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}


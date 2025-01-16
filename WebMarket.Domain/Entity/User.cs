using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class User : IEntityId<long>
{
    public long Id { get; set; }
    
    public string Login { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public List<Cart> Carts { get; set; }
    
    public List<Order> Orders { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
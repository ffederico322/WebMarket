using WebMarket.Domain.Interfaces;

namespace WebMarket.Domain.Entity;

public class UserToken : IEntityId<long>
{
    public long Id { get; set; }
    
    public string RefreshToken { get; set; }
    
    // будет действовать 7 дней 
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public long UserId { get; set; }
    
    public User User { get; set; }
}
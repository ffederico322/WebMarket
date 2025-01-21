namespace WebMarket.Domain.Enum;

public enum ErrorCodes
{
    // Products 0 - 9
    ProductsNotFound = 0,
    InvalidProductDataError = 1,
    InvalidProductName = 2,
    InvalidProductPrice = 3,
    InvalidProductStock = 4,
    InvalidProductDescription = 5,
    ProductNotFound = 6,
    
    
    // Category 
    CategoryNotFound = 10,
    CategoriesNotFound = 11,
    
    // Cart
    CartNotFound = 12,
    CartItemNotFound = 13,
    EmptyCart = 14,
    
    // Order
    OrdersNotFound = 15,
    OrderAlreadyCancelled = 16,
    
    // Entity 50 - 60
    EntityNotFound = 50,
    
    // Остальные 80 - 90
    NoDataProvided = 80,
    
    
    InternalServerError = 100
    
}
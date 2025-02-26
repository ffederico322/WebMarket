using MockQueryable.Moq;
using Moq;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Interfaces.Repositories;

namespace WebMarket.Tests.Configurations;

public class MockRepositoriesGetter
{
    public static Mock<IBaseRepository<Product>> GetMockProductRepository()
    {
        var mock = new Mock<IBaseRepository<Product>>();

        var products = GetProducts().BuildMockDbSet();
        mock.Setup(u => u.GetAll()).Returns(() => products.Object);
        return mock;
    }

    public static Mock<IBaseRepository<Category>> GetMockCategoryRepository()
    {
        var mock = new Mock<IBaseRepository<Category>>();

        var categories = GetCategories().BuildMockDbSet();
        mock.Setup(u => u.GetAll()).Returns(() => categories.Object);
        return mock;
    }

    public static IQueryable<Product> GetProducts()
    {
        return new List<Product>()
        {
            new Product()
            {
            Id = 1,
            Name = "Product 1",
            CategoryId = 1,
            Description = "Product 1 description",
            Image = "product1.jpg",
            Price = 200,
            Stock = 100,
            },
            new Product()
            {
                Id = 1,
                Name = "Product 2",
                CategoryId = 2,
                Description = "Product 2 description",
                Image = "product2.jpg",
                Price = 100,
                Stock = 100,
            }
        }.AsQueryable();
    }

    public static IQueryable<Category> GetCategories()
    {
        return new List<Category>()
        {
            new Category()
            {
                Id = 1,
                Name = "Category 1",
            },
            new Category()
            {
                Id = 2,
                Name = "Category 2",
            }
        }.AsQueryable();
    }

}



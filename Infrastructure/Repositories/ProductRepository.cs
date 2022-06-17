using Dapper;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Data;

namespace ProductAPI.Infrastructure.Repositories; 

public class ProductRepository : IProductRepository{
    
    private readonly SqlServerConnection _connection;

    public ProductRepository(SqlServerConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<Product>> GetAsync()
    {
        using (var cnn = _connection.CreateConnection())
        {
            var sql = $@"SELECT *
                            FROM product p
                            inner JOIN product_category pc ON p.id = pc.product_id  
                            inner JOIN category c ON pc.category_id = c.id";

            IEnumerable<Product> products = cnn.Query<Product, Category, Product>(sql, (p, c) =>
                    {
                        Dictionary<string, Product> productCategory = new Dictionary<string, Product>();
                        Product product;
                        if (!productCategory.TryGetValue(p.Id, out product))
                        {
                            productCategory.Add(p.Id, product = p);
                        }

                        if (product.Category == null)
                            product.Category = new List<Category>();
                        product.Category.Add(c);
                        return product;
                    },
                    splitOn: "id"
                ).GroupBy(p => p.Id)
                .Select(group =>
                {
                    Product product = group.First();
                    product.Category = group.Select(p => p.Category.Single()).ToList();
                    return product;
                });
            return products.ToList();
        }
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> CreateAsync(Product character)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> UpdateAsync(Product character)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}
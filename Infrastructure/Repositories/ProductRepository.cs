using Dapper;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Data;

namespace ProductAPI.Infrastructure.Repositories {
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
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"SELECT *
                        FROM product p
                        inner JOIN product_category pc ON p.id = pc.product_id
                        inner JOIN category c ON pc.category_id = c.id
                        where p.id = @id";

                IEnumerable<Product> product = cnn.Query<Product, Category, Product>(sql, (p, c) =>
                        {
                            var categories = new Dictionary<string, Product>();
                            Product product;
                            if (!categories.TryGetValue(p.Id, out product))
                            {
                                categories.Add(p.Id, product = p);
                            }

                            if (product.Category == null)
                                product.Category = new List<Category>();
                            product.Category.Add(c);
                            return product;
                        },
                        new {Id = id}
                    ).GroupBy(p => p.Id)
                    .Select(group =>
                    {
                        Product product = group.First();
                        product.Category  = group.Select(p => p.Category.Single()).ToList();
                        return product;
                    });
                return product.First();
            }
        }

        public async Task<Product> CreateAsync(Product product)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"INSERT INTO product (id,title,description,isAvailable,ageLimit) 
                        values (@id,@title,@description,@isAvailable,@ageLimit)";

                var newProduct = await cnn.ExecuteAsync(sql, product);
                if (newProduct > 0)
                {
                    return product;
                }
            }
            return null;
        }

        public async Task<Product> UpdateAsync(Product character)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"DELETE 
                   FROM product p
                   WHERE p.id = @id";

                var deletePExecuteAsync = await cnn.ExecuteAsync(sql);
                if (deletePExecuteAsync > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
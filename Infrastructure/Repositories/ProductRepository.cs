using Dapper;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories.Interfaces;

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
                            LEFT JOIN product_category pc ON p.id = pc.productId  
                            LEFT JOIN category c ON pc.categoryId = c.id";

                IEnumerable<Product> products = cnn.Query<Product, Category, Product>(sql, (p, c) =>
                        {
                            Dictionary<string, Product> productCategory = new Dictionary<string, Product>();
                            Product product;
                            if (!productCategory.TryGetValue(p.Id, out product))
                            {
                                productCategory.Add(p.Id, product = p);
                            }

                            // if (product.Category == null)
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
                        LEFT JOIN product_category pc ON p.id = pc.productId
                        LEFT JOIN category c ON pc.categoryId = c.id
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
                    $@"INSERT INTO product (id,title,description,isActive,unitPrice,unitsInStock) 
                        values (@id,@title,@description,@isActive,@unitPrice,@unitsInStock)";

                var newProduct = await cnn.ExecuteAsync(sql, product);
                if (newProduct > 0)
                {
                    return product;
                }
            }
            return null;
        }
        
        public async Task<Product> AddCategoryAsync(Product product,Category category)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var categorySql =
                            $@"INSERT INTO product_category(productId,categoryId)
                        VALUES (@productId,@categoryId);";

                        var productCategory = await cnn.ExecuteAsync(categorySql, new
                        {
                            ProductId = product.Id,
                            CategoryId = category.Id
                        });
                        if (productCategory > 0)
                            return product;
            }

            return null;
        }
        
        

        public async Task<Product> UpdateAsync(Product product)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = $@"update product
                        SET 
                            title = @title,
                            description = @description,
                            isActive = @isActive,
                            unitPrice = @unitPrice,
                            unitsInStock = @unitsInStock
                        where id = @id;";

                var affectedRows = await cnn.ExecuteAsync(sql, new
                {
                    id = product.Id,
                    title = product.Title,
                    description = product.Description,
                    isActive = product.IsActive,
                    unitPrice = product.UnitPrice,
                    unitsInStock = product.UnitsInStock
                    
                });
                
              // if its null how can I choose from it :(
              if (affectedRows > 0)
                  return product;
              
                throw new ArgumentNullException(nameof(product));
            }
        }

        public async Task<bool> RemoveCategoryAsync(string categoryId)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var categorySql =
                    $@"DELETE FROM product_category WHERE categoryId = @categoryId 
                                ";

                var productCategory = await cnn.ExecuteAsync(categorySql, new
                {
                    CategoryId = categoryId
                });
                if (productCategory > 0)
                    return true;
            }
            throw new ArgumentNullException(nameof(categoryId));
        }
        
        
        public async Task<bool> DeleteAsync(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"DELETE 
                   FROM product
                   WHERE id = @id";

                var deletePExecuteAsync = await cnn.ExecuteAsync(sql,new
                {
                    Id = id 
                });
                if (deletePExecuteAsync > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
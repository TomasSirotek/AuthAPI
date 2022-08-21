using Dapper;
using Microsoft.IdentityModel.Tokens;
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
                var sql = @"select * from product";

                IEnumerable<Product> products = await cnn.QueryAsync<Product>(sql);
                if (!products.IsNullOrEmpty())
                {
                    return products.ToList();
                }
                throw new ArgumentNullException(nameof(products));
            }
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"select * from product as p where p.id = @id";

                Product product = await cnn.QueryFirstAsync<Product>(sql, new {Id = id});
                if (product != null)
                {
                    return product;
                }
                throw new ArgumentNullException(nameof(product));
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
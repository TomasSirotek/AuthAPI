using Dapper;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Infrastructure.Repositories {
    public class CategoryRepository : ICategoryRepository {
        
        private readonly SqlServerConnection _connection;

        public CategoryRepository(SqlServerConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<List<Category>> GetAsync()
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"select * from category";

                IEnumerable<Category> newCategories = await cnn.QueryAsync<Category>(sql);
                if (!newCategories.IsNullOrEmpty())
                {
                    return newCategories.ToList();
                }
                throw new ArgumentNullException(nameof(newCategories));
            }
        }

        public async Task<Category> GetByIdAsync(string id)
        {

            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"select * from category as c where c.id = @id";

                Category category = await cnn.QueryFirstAsync<Category>(sql, new {Id = id});
                if (category != null)
                {
                    return category;
                }
                throw new ArgumentNullException(nameof(category));
            }
        }

        public async Task<Category> CreateAsync(Category category)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"INSERT INTO category (id,name) 
                        values (@id,@name)";

                var newCategory = await cnn.ExecuteAsync(sql, category);
                if (newCategory > 0)
                {
                    return category;
                }
                throw new ArgumentNullException(nameof(category));
            }
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = $@"update category
                        SET name = @name 
                        where id = @id;";

                var affectedRows = await cnn.ExecuteAsync(sql, new
                {
                    id = category.Id,
                    name = category.Name
                });

                if (affectedRows > 0)
                    return category;
                throw new ArgumentNullException(nameof(category));
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"DELETE 
                   FROM category
                   WHERE id = @id";

                var deletePExecuteAsync = await cnn.ExecuteAsync(sql,new
                {
                    Id = id
                });
                if (deletePExecuteAsync > 0)
                {
                    return true;
                }
                throw new ArgumentNullException(nameof(id));
            }
        }
    }
}
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Services {
    public class ProductService : IProductService{
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productRepository.GetAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(Product product,List<string> category)
        {
            List<Category> categoryProduct = new();
            List<Category> productCategory = categoryProduct;
            foreach (var name in category)
            {
                var categoryList = new Category
                {
                    Name = name
                };
                productCategory.Add(categoryList);
            }
            product.Category = productCategory;

            Product newProduct = await _productRepository.CreateAsync(product);
            if (newProduct != null)
            {
                // Check for existing category and asign it to the product
            }
            throw new Exception($"Could not create product with Id: {product.Id}");;

        }

        public Task<Product> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}
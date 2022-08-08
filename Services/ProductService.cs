using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services {
    public class ProductService : IProductService{
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository,ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;

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
                // Check for existing category
                foreach (string name in category)
                {
                    Category checkCategory = await _categoryRepository.GetByNameAsync(name);
                    if (checkCategory != null)
                    {
                        bool resultAddCategory =
                            await _productRepository.CreateCategoryAsync(newProduct.Id,checkCategory.Id);
                    }

                }

                Product productTest = await _productRepository.GetByIdAsync(product.Id);
                
                return productTest;
                // if exists asign each of one to the Product
                // find category by name from categoryRepository
                // if exist asign each categoryId and productId into SQL from R

            }
            throw new Exception($"Could not create product with Id: {product.Id}");;

        }

        public async Task<Product> UpdateAsync(Product product)
        {
            if (product == null) 
                throw new Exception("Product cannot be empty");
            // try to update
            Product productUpdated = await _productRepository.UpdateAsync(product);
            if (productUpdated == null) 
                throw new Exception("Product could not be created");
            // fetch updated role
            Product updatedProduct = await _productRepository.GetByIdAsync(product.Id);
            // if done return back 
            return updatedProduct;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}
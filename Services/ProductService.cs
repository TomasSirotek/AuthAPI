using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

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

        public async Task<Product> CreateAsync(Product product)
        {
            Product createdProduct = await _productRepository.CreateAsync(product);

            return createdProduct;
        }

        public async Task<Product> UpdateAsync(PutProductModel productModel)
        {
            Product fetchedProduct = await _productRepository.GetByIdAsync(productModel.Id);
            if(fetchedProduct == null) throw new Exception($"Could not find product with Id : {productModel.Id}");
            
            // make new one 
            Product updatedProduct = new Product()
            {
                Id = productModel.Id,
                Title = productModel.Title,
                Description = productModel.Description,
                Image = productModel.Image,
                IsActive = productModel.IsActive,
                UnitPrice = productModel.UnitPrice,
                UnitsInStock = productModel.UnitsInStock
            };
            // update product 
            Product productUpdated = await _productRepository.UpdateAsync(updatedProduct);
            if (productUpdated == null) throw new Exception("Could not update product ");
            
            return productUpdated;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}

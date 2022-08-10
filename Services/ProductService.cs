using ProductAPI.Domain.BindingModels.Category;
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
            
            Product createdProduct = await _productRepository.CreateAsync(product);
            // Check for existing category
            foreach (Category categoryName in product.Category)
            {
                Category fetchedCategory = await _categoryRepository.GetByNameAsync(categoryName.Name);
                if (fetchedCategory == null)  throw new Exception($"Category with name: {categoryName.Name} does not exist");
                Product  productResult = await _productRepository.AddCategoryAsync(createdProduct, fetchedCategory);
                if (productResult == null)  throw new Exception($"Could not add category to with name: {categoryName.Name} does not exist");

            }
            
            return createdProduct;
            
            throw new Exception($"Could not create product with Id: {product.Id}");;

        }

        public async Task<Product> UpdateAsync(Product product)
        {
            product.Category.Clear();
            
          
            Product productUpdated = await _productRepository.UpdateAsync(product);
            if (productUpdated == null) throw new Exception("Could not product ");

          
       
            foreach (Category category in product.Category)
            {
                Category fetchedCategory = await _categoryRepository.GetByIdAsync(product.Id);

                if(fetchedCategory == null)  throw new Exception($"Could not find category with name {category.Name}");
                Product  productResult = await _productRepository.AddCategoryAsync(productUpdated, fetchedCategory);
                if (productResult == null)  throw new Exception($"Could not add category to with name: {category.Name} does not exist");
            }
                
            return productUpdated;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}

// if (newProduct != null)
// {
//     // Check for existing category
//     foreach (string name in category)
//     {
//         Category checkCategory = await _categoryRepository.GetByNameAsync(name);
//         if (checkCategory != null)
//         {
//             bool resultAddCategory =
//                 await _productRepository.CreateCategoryAsync(newProduct.Id,checkCategory.Id);
//         }
//
//     }
//
//     Product productTest = await _productRepository.GetByIdAsync(product.Id);
//     
//     return productTest;
//     // if exists asign each of one to the Product
//     // find category by name from categoryRepository
//     // if exist asign each categoryId and productId into SQL from R
//
// }
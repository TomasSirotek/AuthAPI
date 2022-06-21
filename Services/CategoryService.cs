using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services {
    public class CategoryService : ICategoryService {
        
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        public async Task<List<Category>> GetAsync()
        {
            return await _categoryRepository.GetAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            Category createCategory = await _categoryRepository.CreateAsync(category);
            if (createCategory == null)  throw new Exception("Category cannot be empty");
            Category fetchedCategory = await _categoryRepository.GetByIdAsync(category.Id);
            return fetchedCategory;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            if (category == null) 
                throw new Exception("Product cannot be empty");
            // try to update
            Category productUpdated = await _categoryRepository.UpdateAsync(category);
            if (productUpdated == null) 
                throw new Exception("Product could not be created");
            // fetch updated role
            Category updatedRole = await _categoryRepository.GetByIdAsync(category.Id);
            // if done return back 
            return updatedRole;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
    }
}
using Data;
using Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private ReadLaterDataContext _ReadLaterDataContext;
        public CategoryService(ReadLaterDataContext readLaterDataContext) 
        {
            _ReadLaterDataContext = readLaterDataContext;            
        }

        public async Task<Category> CreateCategory(Category category, string username)
        {
            category.CreatedBy = username;
            await _ReadLaterDataContext.AddAsync(category);
            await _ReadLaterDataContext.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategory(Category category, string username)
        {
             category.CreatedBy = username;
             _ReadLaterDataContext.Update(category);
            await _ReadLaterDataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories(string username)
        {
            return  _ReadLaterDataContext.Categories.Where(n => n.CreatedBy == username).ToList();
        }

        public async Task<Category> GetCategory(int Id, string username)
        {
            return _ReadLaterDataContext.Categories.Where(c => c.ID == Id).Where(n => n.CreatedBy == username).FirstOrDefault();
        }

        public async Task<Category> GetCategory(string Name, string username)
        {
            return _ReadLaterDataContext.Categories.Where(c => c.Name == Name).Where(n => n.CreatedBy == username).FirstOrDefault();
        }

        public async Task DeleteCategory(Category category)
        {
            _ReadLaterDataContext.Categories.Remove(category);
            await _ReadLaterDataContext.SaveChangesAsync();
        }
    }
}

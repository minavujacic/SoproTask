using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(Category category, string username);
        Task<IEnumerable<Category>> GetCategories(string username);
        Task<Category> GetCategory(int Id, string username);
        Task<Category>GetCategory(string Name, string username);
        Task UpdateCategory(Category category, string username);
        Task DeleteCategory(Category category);
    }
}

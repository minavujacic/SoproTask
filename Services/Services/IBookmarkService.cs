using Entity;
//using Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookmarkService
    {
        Task<IEnumerable<Bookmark>> GetBookmarks(string username);
        Task<Bookmark> GetBookmark(int id, string username);
        Task<int> CreateBookmark(Bookmark bookmark, string createdBy);
        Task UpdateBookmark(int id, Bookmark bookmark);
        Task DeleteBookmark(int id);
        bool CheckExist(int id);

    }
}

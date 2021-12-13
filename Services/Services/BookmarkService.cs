using Data;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly ReadLaterDataContext _context;

        public BookmarkService(ReadLaterDataContext context)
        {
            _context = context;
        }
        public async Task<int> CreateBookmark(Bookmark bookmark, string createdBy)
        {
            var _bookmark = new Bookmark()
            {
                URL = bookmark.URL,
                ShortDescription = bookmark.ShortDescription,
                CategoryId = bookmark.CategoryId,
                CreateDate = DateTime.Now,
                CreatedBy = createdBy
            };

            await _context.Bookmark.AddAsync(_bookmark);
            await _context.SaveChangesAsync();
            return _bookmark.ID;
        }

        public async Task DeleteBookmark(int id)
        {
            var bookmark = await _context.Bookmark.FirstOrDefaultAsync(n => n.ID == id);
            if (bookmark == null)
                throw new Exception("Not found");

            _context.Bookmark.Remove(bookmark);
            await _context.SaveChangesAsync();
       }

        public async Task<Bookmark> GetBookmark(int id, string username)
        {
            return await _context.Bookmark.Include(x => x.Category).Where(n => n.ID == id).Where(u => u.CreatedBy == username).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Bookmark>> GetBookmarks(string username)
        {
           return await _context.Bookmark.Include(x => x.Category).Where(u => u.CreatedBy == username).ToListAsync();
        }

        public async Task UpdateBookmark(int id, Bookmark bookmark)
        {
            var _bookmark = await _context.Bookmark.FirstOrDefaultAsync(n => n.ID == id);
            if (bookmark == null)
                throw new Exception("Not found");

            _bookmark.URL = bookmark.URL;
            _bookmark.ShortDescription = bookmark.ShortDescription;
            _bookmark.CategoryId = bookmark.CategoryId;

            await _context.SaveChangesAsync();
        }

        public bool CheckExist(int id)
        {         
            return  _context.Bookmark.Any(e => e.ID == id);
        }
    }
}

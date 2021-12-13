using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using Services;
using Microsoft.AspNetCore.Authorization;

namespace ReadLater5.Controllers
{
    [Authorize]
    public class BookmarksController : Controller
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly ReadLaterDataContext _context;

        public BookmarksController(IBookmarkService bookmarkService, ReadLaterDataContext context)
        {
            _bookmarkService = bookmarkService;
            _context = context;
        }

        // GET: Bookmarks
        public async Task<IActionResult> Index()
        {
            var result = await _bookmarkService.GetBookmarks(User.Identity.Name);
            return View(result);
        }

        // GET: Bookmarks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmark = await _bookmarkService.GetBookmark(id, User.Identity.Name);
            if (bookmark == null)
            {
                return NotFound();
            }

            return View(bookmark);
        }

        // GET: Bookmarks/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(n => n.CreatedBy == User.Identity.Name), "ID", "Name");
            return View();
        }

        // POST: Bookmarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,URL,ShortDescription,CategoryId")] Bookmark bookmark)
        {
            var username = User.Identity.Name;
            if (ModelState.IsValid)
            {
                await _bookmarkService.CreateBookmark(bookmark, username);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(n => n.CreatedBy == username), "ID", "Name", bookmark.CategoryId);
            return View(bookmark);
        }

        // GET: Bookmarks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var username = User.Identity.Name;

            var bookmark = await _bookmarkService.GetBookmark(id, username);
            if (bookmark == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(n => n.CreatedBy == username), "ID", "Name", bookmark.CategoryId);
            return View(bookmark);
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,URL,ShortDescription,CategoryId")] Bookmark bookmark)
        {
            if (id != bookmark.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _bookmarkService.UpdateBookmark(id, bookmark);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookmarkExists(bookmark.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(n => n.CreatedBy == User.Identity.Name), "ID", "Name", bookmark.CategoryId);
            return View(bookmark);
        }

        // GET: Bookmarks/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmark = await _bookmarkService.GetBookmark(id, User.Identity.Name);
            if (bookmark == null)
            {
                return NotFound();
            }

            return View(bookmark);
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookmark = await _bookmarkService.GetBookmark(id, User.Identity.Name);
            await _bookmarkService.DeleteBookmark(bookmark.ID);
            return RedirectToAction(nameof(Index));
        }

        private bool BookmarkExists(int id)
        {
            return _bookmarkService.CheckExist(id);
        }
    }
}

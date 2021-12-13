using Entity;
using Entity.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookmark>>> GetBookmarks()
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var bookmarks =  await _bookmarkService.GetBookmarks(username);
            return Ok(bookmarks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bookmark>> GetBookmark(int id)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var bookmark = await _bookmarkService.GetBookmark(id, username);
            if (bookmark == null)
            {
                return NotFound();
            }
            return Ok(bookmark);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBookmark([FromBody]BookmarkDto bookmark)
        {
            var _bookmark = new Bookmark()
            {
                URL = bookmark.URL,
                ShortDescription = bookmark.ShortDescription,
                CategoryId = bookmark.CategoryId,
            };
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var bookmarkId = await _bookmarkService.CreateBookmark(_bookmark, username);

            return Ok(bookmarkId);
        }

        [HttpPut("update-bookmark/{id}")]
        public async Task<IActionResult> UpdateBookmark(int id, [FromBody]BookmarkDto bookmark)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var result = _bookmarkService.GetBookmark(id, username);

            if (result == null)
            {
                return NotFound();
            }
            else{
                var _bookmark = new Bookmark()
                {
                    URL = bookmark.URL,
                    ShortDescription = bookmark.ShortDescription,
                    CategoryId = bookmark.CategoryId,
                };
                await _bookmarkService.UpdateBookmark(id, _bookmark);
                return Ok();
            }          
        }

        [HttpDelete("delete-bookmark/{id}")]
        public async Task<IActionResult> DeleteBookmark(int id)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (id == null)
            {
                return NotFound();
            }

            var bookmark = await _bookmarkService.GetBookmark(id, username);
            if (bookmark == null)
            {
                return NotFound();
            }

            await _bookmarkService.DeleteBookmark(id);
            return Ok();
        }

    }
}

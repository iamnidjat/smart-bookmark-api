using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBookmarkApi.Services.Interfaces;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }
    }
}

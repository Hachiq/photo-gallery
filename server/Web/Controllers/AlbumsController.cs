using Core.Contracts;
using Core.Exceptions;
using Core.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController(IAlbumService _albumService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbum([FromRoute] int id)
        {
            var response = await _albumService.GetAlbumAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums([FromQuery] int page, [FromQuery] int? userId)
        {
            if (userId.HasValue)
            {
                var response = await _albumService.GetAlbumsAsync(page, userId.Value);
                return Ok(response);
            }
            else
            {
                var response = await _albumService.GetAlbumsAsync(page);
                return Ok(response);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAlbum(CreateAlbumRequest request)
        {
            try
            {
                var response = await _albumService.CreateAlbumAsync(request);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

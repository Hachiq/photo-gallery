using Core.Contracts;
using Core.Exceptions;
using Core.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController(
        IAlbumService _albumService,
        ILogger<AlbumsController> _logger) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbum([FromRoute] int id, [FromQuery] int page)
        {
            var response = await _albumService.GetAlbumAsync(id, page);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums([FromQuery] int page, [FromQuery] int? userId)
        {
            try
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
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex,
                    "Get albums request failed: user id {id}", userId);
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
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

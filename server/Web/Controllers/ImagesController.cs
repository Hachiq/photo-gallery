using Azure.Core;
using Core.Constants;
using Core.Contracts;
using Core.Exceptions;
using Core.Requests;
using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController(
        IImageService _imageService,
        ILogger<ImagesController> _logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetByAlbum([FromQuery] int albumId, [FromQuery] int page)
        {
            var response = await _imageService.GetByAlbumIdAsync(albumId, page);
            return Ok(response);
        }

        [HttpGet("first")]
        public async Task<IActionResult> GetFirst([FromQuery] int albumId)
        {
            try
            {
                var response = await _imageService.GetFirstAsync(albumId);
                return Ok(response);
            }
            catch (ImageNotFoundException)
            {
                return Ok(new EmptyCollectionResponse());
            }
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddImageRequest request)
        {
            try
            {
                var userId = HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == Token.Id)?.Value ?? throw new UnauthorizedAccessException();

                await _imageService.AddAsync(request, Convert.ToInt32(userId));
                return Ok();
            }
            catch (Exception ex) when (ex is AlbumNotFoundException or InvalidFileException or UnauthorizedAccessException)
            {
                _logger.LogError(ex,
                    "Add request failed: file {file}, album id {id}",
                    request.File.FileName,
                    request.AlbumId);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _imageService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex) when (ex is ImageNotFoundException or InvalidFileException)
            {
                _logger.LogError(ex,
                    "Delete request failed: image id {id}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("like")]
        public async Task<IActionResult> Like(LikeRequest request)
        {
            try
            {
                await _imageService.LikeAsync(request);
                return Ok();
            }
            catch (AlreadyRatedException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex) when (ex is ImageNotFoundException or UserNotFoundException)
            {
                _logger.LogError(ex,
                    "Like request failed: image id {imageId}, user id {userId}",
                    request.ImageId,
                    request.UserId);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("dislike")]
        public async Task<IActionResult> Dislike(LikeRequest request)
        {
            try
            {
                await _imageService.DislikeAsync(request);
                return Ok();
            }
            catch (AlreadyRatedException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex) when (ex is ImageNotFoundException or UserNotFoundException)
            {
                _logger.LogError(ex,
                    "Like request failed: image id {imageId}, user id {userId}",
                    request.ImageId,
                    request.UserId);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("unlike")]
        public async Task<IActionResult> Unlike(LikeRequest request)
        {
            try
            {
                await _imageService.UnlikeAsync(request);
                return Ok();
            }
            catch (Exception ex) when (ex is ImageNotFoundException or UserNotFoundException)
            {
                _logger.LogError(ex,
                    "Like request failed: image id {imageId}, user id {userId}",
                    request.ImageId,
                    request.UserId);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("reactions/{id}")]
        public async Task<IActionResult> GetReactions([FromRoute] int id)
        {
            try
            {
                var response = await _imageService.GetReactionsAsync(id);
                return Ok(response);
            }
            catch (ImageNotFoundException ex)
            {
                _logger.LogError(ex,
                    "Reactions request failed: image id {id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}

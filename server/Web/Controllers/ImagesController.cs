using Azure.Core;
using Core.Contracts;
using Core.Exceptions;
using Core.Requests;
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
        public async Task<IActionResult> GetByAlbum([FromQuery] int albumId)
        {
            var response = await _imageService.GetByAlbumIdAsync(albumId);
            return Ok(response);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddImageRequest request)
        {
            try
            {
                await _imageService.AddAsync(request);
                return Ok();
            }
            catch (Exception ex) when (ex is AlbumNotFoundException or InvalidFileException)
            {
                _logger.LogError(ex,
                    "Add request failed: file {file}, album id {id}",
                    request.File.FileName,
                    request.AlbumId);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
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

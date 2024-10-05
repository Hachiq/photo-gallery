﻿using Core.Contracts;
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
        public async Task<IActionResult> GetAlbums([FromQuery] int? userId)
        {
            if (userId.HasValue)
            {
                var response = await _albumService.GetAlbumsAsync(userId.Value);
                return Ok(response);
            }
            else
            {
                var response = await _albumService.GetAlbumsAsync();
                return Ok(response);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAlbum(CreateAlbumRequest request)
        {
            try
            {
                await _albumService.CreateAlbumAsync(request);
                return Ok();
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

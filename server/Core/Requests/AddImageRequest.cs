using Microsoft.AspNetCore.Http;

namespace Core.Requests;

public record AddImageRequest(IFormFile File, int AlbumId);

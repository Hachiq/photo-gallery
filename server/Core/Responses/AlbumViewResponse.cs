using Core.DTOs;
using Core.Entities;

namespace Core.Responses;

public record AlbumViewResponse(AlbumDTO Album, PagedResponse<Image> Images);

using Core.Entities;
using Core.Requests;
using Core.Responses;

namespace Core.Contracts;

public interface IAlbumService
{
    Task<PagedResponse<Album>> GetAlbumsAsync(int page);
    Task<PagedResponse<Album>> GetAlbumsAsync(int page, int userId);
    Task<Album> GetAlbumAsync(int albumId);
    Task<int> CreateAlbumAsync(CreateAlbumRequest model);
}

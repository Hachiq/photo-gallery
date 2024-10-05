using Core.Entities;
using Core.Requests;

namespace Core.Contracts;

public interface IAlbumService
{
    Task<IEnumerable<Album>> GetAlbumsAsync();
    Task<IEnumerable<Album>> GetAlbumsAsync(int userId);
    Task<Album> GetAlbumAsync(int albumId);
    Task CreateAlbumAsync(CreateAlbumRequest model);
}

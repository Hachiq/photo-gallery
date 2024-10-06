using Core.Constants;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.Requests;
using Core.Responses;

namespace Services.Implementations;

class AlbumService(IRepository _db) : IAlbumService
{
    public async Task<PagedResponse<Album>> GetAlbumsAsync(int page)
    {
        var albums = await _db.GetAllAsync<Album>();
        var orderedAlbums = albums.OrderByDescending(a => a.CreatedAt)
                                  .Skip((page - 1) * Common.PageSize)
                                  .Take(5);

        var totalRecords = albums.Count();
        return new PagedResponse<Album>(orderedAlbums, totalRecords);
    }

    public async Task<PagedResponse<Album>> GetAlbumsAsync(int page, int userId)
    {
        var albums = await _db.GetAllAsync<Album>(a => a.UserId == userId);
        var orderedAlbums = albums.OrderByDescending(a => a.CreatedAt)
                                  .Skip((page - 1) * Common.PageSize)
                                  .Take(5);

        var totalRecords = albums.Count();
        return new PagedResponse<Album>(orderedAlbums, totalRecords);
    }

    public async Task<Album> GetAlbumAsync(int albumId)
    {
        var album = await _db.GetByIdAsync<Album>(albumId) ?? throw new AlbumNotFoundException();
        return album;
    }
    public async Task<int> CreateAlbumAsync(CreateAlbumRequest model)
    {
        var user = await _db.GetByIdAsync<User>(model.UserId) ?? throw new UserNotFoundException();

        var album = new Album
        {
            Title = model.Title,
            User = user,
            CreatedAt = DateTime.Now,
        };

        await _db.AddAsync(album);
        await _db.SaveChangesAsync();

        return (int)album.Id;
    }
}

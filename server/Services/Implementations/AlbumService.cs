using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.Requests;

namespace Services.Implementations;

class AlbumService(IRepository _db) : IAlbumService
{
    public async Task<IEnumerable<Album>> GetAlbumsAsync()
    {
        return await _db.GetAllAsync<Album>();
    }

    public async Task<IEnumerable<Album>> GetAlbumsAsync(int userId)
    {
        return await _db.GetAllAsync<Album>(a => a.UserId == userId);
    }

    public async Task<Album> GetAlbumAsync(int albumId)
    {
        var album = await _db.GetByIdAsync<Album>(albumId) ?? throw new AlbumNotFoundException();
        return album;
    }
    public async Task CreateAlbumAsync(CreateAlbumRequest model)
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
    }
}

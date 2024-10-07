using Core.Constants;
using Core.Contracts;
using Core.DTOs;
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
        var user = await _db.GetByIdAsync<User>(userId) ?? throw new UserNotFoundException();

        var albums = await _db.GetAllAsync<Album>(a => a.User == user);
        var orderedAlbums = albums.OrderByDescending(a => a.CreatedAt)
                                  .Skip((page - 1) * Common.PageSize)
                                  .Take(5);

        var totalRecords = albums.Count();
        return new PagedResponse<Album>(orderedAlbums, totalRecords);
    }

    public async Task<AlbumViewResponse> GetAlbumAsync(int albumId, int page)
    {
        var album = await _db.GetByIdAsync<Album>(albumId, a => a.Images) ?? throw new AlbumNotFoundException();
        var orderedImages = album.Images.OrderByDescending(i => i.UploadedAt)
                                  .Skip((page - 1) * Common.PageSize)
                                  .Take(5);

        var totalRecords = album.Images.Count;

        var dto = new AlbumDTO
        {
            Id = albumId,
            Title = album.Title,
            CreatedAt = album.CreatedAt,
            UserId = album.UserId
        };

        var paged = new PagedResponse<Image>(orderedImages, totalRecords);

        return new AlbumViewResponse(dto, paged);
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

    public async Task DeleteAlbumAsync(int id)
    {
        var album = await _db.GetByIdAsync<Album>(id, a => a.Images) ?? throw new AlbumNotFoundException();

        foreach (var item in album.Images)
        {
            if (File.Exists(item.FullPath))
            {
                File.Delete(item.FullPath);
            }
            else
            {
                throw new InvalidFileException();
            }

            _db.Delete(item);
        }

        _db.Delete(album);
        await _db.SaveChangesAsync();
    }
}

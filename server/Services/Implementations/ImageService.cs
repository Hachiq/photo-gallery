using Azure.Core;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.Requests;
using Core.Responses;

namespace Services.Implementations;

class ImageService(IRepository _db) : IImageService
{
    public async Task<IEnumerable<Image>> GetByAlbumIdAsync(int albumId)
    {
        var images = await _db.GetAllAsync<Image>(i => i.AlbumId == albumId);
        return images;
    }
    public async Task AddAsync(AddImageRequest model)
    {
        var album = await _db.GetByIdAsync<Album>(model.AlbumId) ?? throw new AlbumNotFoundException();

        if (model.File is null || model.File.Length is 0)
        {
            throw new InvalidFileException();
        }

        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(uploadsFolderPath))
        {
            Directory.CreateDirectory(uploadsFolderPath);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
        var filePath = Path.Combine(uploadsFolderPath, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await model.File.CopyToAsync(fileStream);
        }

        var image = new Image
        {
            Album = album,
            FilePath = filePath,
            UploadedAt = DateTime.Now,
            LikeCount = 0,
            DislikeCount = 0
        };

        await _db.AddAsync(image);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var image = await _db.GetByIdAsync<Image>(id) ?? throw new ImageNotFoundException();

        if (File.Exists(image.FilePath))
        {
            File.Delete(image.FilePath);
        }
        else
        {
            throw new InvalidFileException();
        }

        _db.Delete(image);
        await _db.SaveChangesAsync();
    }

    public async Task LikeAsync(LikeRequest request)
    {
        var existingLike = await _db.FindAsync<Like>(l =>
            l.UserId == request.UserId &&
            l.ImageId == request.ImageId);

        if (existingLike is not null)
        {
            throw new AlreadyRatedException();
        }

        var image = await _db.GetByIdAsync<Image>(request.ImageId) ?? throw new ImageNotFoundException();
        var user = await _db.GetByIdAsync<User>(request.UserId) ?? throw new UserNotFoundException();

        var like = new Like
        {
            Image = image,
            User = user,
            IsLike = true
        };

        image.LikeCount++;
        image.Likes.Add(like);
        user.Likes.Add(like);

        _db.Update(image);
        await _db.AddAsync(like);
        await _db.SaveChangesAsync();
    }

    public async Task DislikeAsync(LikeRequest request)
    {
        var existingLike = await _db.FindAsync<Like>(l =>
            l.UserId == request.UserId &&
            l.ImageId == request.ImageId);

        if (existingLike is not null)
        {
            throw new AlreadyRatedException();
        }

        var image = await _db.GetByIdAsync<Image>(request.ImageId) ?? throw new ImageNotFoundException();
        var user = await _db.GetByIdAsync<User>(request.UserId) ?? throw new UserNotFoundException();

        var like = new Like
        {
            Image = image,
            User = user,
            IsLike = false
        };

        image.DislikeCount++;
        image.Likes.Add(like);
        user.Likes.Add(like);

        _db.Update(image);
        await _db.AddAsync(like);
        await _db.SaveChangesAsync();
    }

    public async Task UnlikeAsync(LikeRequest request)
    {
        var image = await _db.GetByIdAsync<Image>(request.ImageId) ?? throw new ImageNotFoundException();
        var user = await _db.GetByIdAsync<User>(request.UserId) ?? throw new UserNotFoundException();

        var like = await _db.FindAsync<Like>(l => l.UserId == request.UserId && l.ImageId == request.ImageId) ?? throw new Exception();

        if (like.IsLike)
        {
            image.LikeCount--;
        }
        else
        {
            image.DislikeCount--;
        }
        image.Likes.Remove(like);
        user.Likes.Remove(like);

        _db.Update(image);
        _db.Delete(like);
        await _db.SaveChangesAsync();
    }

    public async Task<ReactionsResponse> GetReactionsAsync(int id)
    {
        var image = await _db.GetByIdAsync<Image>(id) ?? throw new ImageNotFoundException();

        return new ReactionsResponse(image.LikeCount, image.DislikeCount);
    }
}

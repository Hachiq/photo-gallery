﻿using Core.Constants;
using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.Requests;
using Core.Responses;

namespace Services.Implementations;

class ImageService(IRepository _db) : IImageService
{
    public async Task<PagedResponse<Image>> GetByAlbumIdAsync(int albumId, int page)
    {
        var images = await _db.GetAllAsync<Image>(i => i.AlbumId == albumId);
        var orderedImages = images.OrderByDescending(i => i.UploadedAt)
                                  .Skip((page - 1) * Common.PageSize)
                                  .Take(5);

        var totalRecords = images.Count();

        return new PagedResponse<Image>(orderedImages, totalRecords);
    }

    public async Task<Image> GetFirstAsync(int albumId)
    {
        var images = await _db.GetAllAsync<Image>(i => i.AlbumId == albumId);
        var first = images.OrderByDescending(i => i.UploadedAt).FirstOrDefault() ?? throw new ImageNotFoundException();
        return first;
    }

    public async Task AddAsync(AddImageRequest model, int userId)
    {
        var album = await _db.GetByIdAsync<Album>(model.AlbumId) ?? throw new AlbumNotFoundException();

        if (userId != album.UserId)
        {
            throw new UnauthorizedAccessException();
        }

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
        var relativePath = Path.Combine("images", fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await model.File.CopyToAsync(fileStream);
        }

        var image = new Image
        {
            Album = album,
            FullPath = filePath,
            RelativePath = relativePath,
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

        if (File.Exists(image.FullPath))
        {
            File.Delete(image.FullPath);
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

        var image = await _db.GetByIdAsync<Image>(request.ImageId) ?? throw new ImageNotFoundException();
        var user = await _db.GetByIdAsync<User>(request.UserId) ?? throw new UserNotFoundException();

        if (existingLike is not null && !existingLike.IsLike)
        {
            image.LikeCount++;
            image.DislikeCount--;
            _db.Update(image);

            existingLike.IsLike = true;
            _db.Update(existingLike);

            await _db.SaveChangesAsync();

            throw new AlreadyRatedException();
        }

        if (existingLike is not null && existingLike.IsLike)
        {
            image.LikeCount--;
            image.Likes.Remove(existingLike);
            _db.Update(image);

            _db.Delete(existingLike);
            await _db.SaveChangesAsync();

            throw new AlreadyRatedException();
        }

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

        var image = await _db.GetByIdAsync<Image>(request.ImageId) ?? throw new ImageNotFoundException();
        var user = await _db.GetByIdAsync<User>(request.UserId) ?? throw new UserNotFoundException();

        if (existingLike is not null && existingLike.IsLike)
        {
            image.LikeCount--;
            image.DislikeCount++;
            _db.Update(image);

            existingLike.IsLike = false;
            _db.Update(existingLike);

            await _db.SaveChangesAsync();

            throw new AlreadyRatedException();
        }

        if (existingLike is not null && !existingLike.IsLike)
        {
            image.DislikeCount--;
            image.Likes.Remove(existingLike);
            _db.Update(image);

            _db.Delete(existingLike);
            await _db.SaveChangesAsync();

            throw new AlreadyRatedException();
        }

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

    public async Task<ReactionsResponse> GetReactionsAsync(int id)
    {
        var image = await _db.GetByIdAsync<Image>(id) ?? throw new ImageNotFoundException();

        return new ReactionsResponse(image.LikeCount, image.DislikeCount);
    }
}

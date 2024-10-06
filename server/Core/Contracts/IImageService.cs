using Core.Entities;
using Core.Requests;
using Core.Responses;

namespace Core.Contracts;

public interface IImageService
{
    Task<IEnumerable<Image>> GetByAlbumIdAsync(int albumId);
    Task AddAsync(AddImageRequest model);
    Task DeleteAsync(int id);
    Task LikeAsync(LikeRequest request);
    Task DislikeAsync(LikeRequest request);
    Task UnlikeAsync(LikeRequest request);
    Task<ReactionsResponse> GetReactionsAsync(int id);
}

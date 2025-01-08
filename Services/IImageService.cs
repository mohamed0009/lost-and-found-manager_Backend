using Microsoft.AspNetCore.Http;

namespace LostAndFound.API.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile image);
        void DeleteImage(string imageUrl);
    }
}

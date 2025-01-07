using Microsoft.AspNetCore.Http;

namespace LostAndFound.API.Services
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile file);
        void DeleteImage(string fileName);
    }

    public class FileService : IFileService
    {
        private readonly string _uploadDirectory;

        public FileService(IWebHostEnvironment environment)
        {
            _uploadDirectory = Path.Combine(environment.WebRootPath, "uploads");
            if (!Directory.Exists(_uploadDirectory))
                Directory.CreateDirectory(_uploadDirectory);
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_uploadDirectory, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
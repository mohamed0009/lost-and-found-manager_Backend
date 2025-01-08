using Microsoft.AspNetCore.Hosting;

namespace LostAndFound.API.Services
{
    public class ImageService : IImageService
    {
        private readonly string _uploadDirectory;
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadDirectory = Path.Combine(environment.WebRootPath, "uploads", "items");

            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("Aucun fichier fourni");
            }

            // Vérification du type de fichier
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Type de fichier non autorisé");
            }

            // Création d'un nom de fichier unique
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadDirectory, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Retourne le chemin relatif de l'image
                return $"/uploads/items/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la sauvegarde de l'image : {ex.Message}");
            }
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            try
            {
                var fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
                var filePath = Path.Combine(_uploadDirectory, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                // Log l'erreur mais ne pas l'arrêter
                Console.WriteLine($"Erreur lors de la suppression de l'image : {ex.Message}");
            }
        }
    }
}

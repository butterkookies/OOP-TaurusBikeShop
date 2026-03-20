namespace WebApplication.Utilities
{
    public static class FileUploadHelper
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        /// <summary>
        /// Saves an uploaded image file to wwwroot/images and returns the relative URL.
        /// </summary>
        public static async Task<string?> SaveImageAsync(IFormFile file, IWebHostEnvironment env, string subFolder = "products")
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.Length > MaxFileSizeBytes)
                throw new InvalidOperationException("File size exceeds 5 MB limit.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(extension))
                throw new InvalidOperationException($"File type '{extension}' is not allowed.");

            var uploadDir = Path.Combine(env.WebRootPath, "images", subFolder);
            Directory.CreateDirectory(uploadDir);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadDir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/{subFolder}/{fileName}";
        }

        /// <summary>
        /// Deletes an image from wwwroot given its relative URL path.
        /// </summary>
        public static void DeleteImage(string? relativeUrl, IWebHostEnvironment env)
        {
            if (string.IsNullOrEmpty(relativeUrl)) return;

            var fullPath = Path.Combine(env.WebRootPath, relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}

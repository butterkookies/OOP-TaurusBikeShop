public interface IPhotoService
{
    Task<string> UploadAsync(IFormFile file);
    Task DeleteAsync(string publicId);
}
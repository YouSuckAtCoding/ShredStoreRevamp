
namespace ShredStorePresentation.Services.Images
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile image);
        bool DeleteImage(string image);
    }
}
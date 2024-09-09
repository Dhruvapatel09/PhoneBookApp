namespace ClientApplicationCore.Infrastructure
{
    public interface IImageUpload
    {
        string AddImageFileToPath(IFormFile imageFile);
    }
}

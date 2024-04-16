namespace ShredStorePresentation.Services.Images
{
    public class ImageService : IImageService
    {

        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageService(IWebHostEnvironment _hostEnvironment)
        {
            this._hostEnvironment = _hostEnvironment;

        }
        public async Task<string> UploadImage(IFormFile image)
        {
            string rootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(image.FileName);
            string fileExtension = Path.GetExtension(image.FileName);
            string ImageName = fileName + fileExtension;
            string path = Path.Combine(rootPath + "/Images/", ImageName);
            using (var fileStream = new FileStream(path, FileMode.Create))
                await image.CopyToAsync(fileStream);

            return ImageName;
        }

        //private static void ResizeImage(string path)
        //{
        //    using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(path))
        //    {
        //        Resize image and save the resized image
        //        image.Resize(200, 200, ResizeType.LanczosResample);
        //        image.Save(path);
        //    }
        //}

        public bool DeleteImage(string image)
        {
            string rootPath = _hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath + "/Images/", image);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }
            return false;
        }    

    }

}

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace First_10.BusinessLogic
{
    public class ImageService
    {
        private readonly FileDialogService _fileDialogService;

        public ImageService(FileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
        }

        public BitmapImage? PasteImage()
        {
            var imagePath = _fileDialogService.OpenImageDialog();

            return imagePath == null || !File.Exists(imagePath)
                       ? null!
                       : new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }
    }
}
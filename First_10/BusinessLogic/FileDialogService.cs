using Microsoft.Win32;

namespace First_10.BusinessLogic
{
    public class FileDialogService
    {
        private OpenFileDialog _imageOpenFileDialog;

        public FileDialogService() =>
            _imageOpenFileDialog = new OpenFileDialog
                                   {
                                       Filter = "Image files (*.jpg;*.png;*jpeg)|*.jpg;*.png;*jpeg",
                                       Multiselect = false
                                   };

        public string? OpenImageDialog() =>
            _imageOpenFileDialog.ShowDialog() == true
                ? _imageOpenFileDialog.FileName
                : null;
    }
}

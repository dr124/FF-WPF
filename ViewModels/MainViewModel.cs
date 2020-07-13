using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FF_WPF.Commands;
using FF_WPF.Utils;
using Microsoft.Win32;

namespace FF_WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BitmapImage _image;

        private BitmapImage _originalImage;

        public MainViewModel()
        {
            LoadImageCommand = new Command(LoadImage);
        }

        public ICommand LoadImageCommand { get; }

        public BitmapImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private void LoadImage(object obj)
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "jpg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var path = openFileDialog.FileName;

                _originalImage = new BitmapImage(new Uri(path));

                var pixels = _originalImage.ToByteArray();

                Image = pixels.ToBitmapImage();
            }
        }
    }
}
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FF_WPF.Commands;
using Microsoft.Win32;

namespace FF_WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            LoadImageCommand = new Command(LoadImage);
        }

        public ICommand LoadImageCommand { get; }

        private void LoadImage(object obj)
        {
            var openFileDialog = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "jpg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var stream = openFileDialog.OpenFile();
                var path = openFileDialog.FileName;
                var b = new BitmapImage();
                b.BeginInit();
                b.StreamSource = stream;
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.EndInit();
                b.Freeze();
                //var b = new BitmapImage(new Uri(path));
                Image = b;
            }
        }

        private BitmapImage _image;

        public BitmapImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
    }
}

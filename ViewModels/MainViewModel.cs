using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using FF_WPF.Commands;
using FF_WPF.Filters;
using FF_WPF.Filters.Implementations;
using FF_WPF.Utils;
using Microsoft.Win32;

namespace FF_WPF.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
        private ImageFilter _imageFilter;

        //todo: refactor this
        public static bool ThumbnailCallback()
        {
            return false;
        }

        public MainViewModel()
        {
            LoadImageCommand = new Command(LoadImage);
            //todo automate this:
            TestThresholdParams.OnUpdate = ApplyFilter;
            BradleysThresholdParams.OnUpdate = ApplyFilter;
            GaussianFilterParams.OnUpdate = ApplyFilter;
        }

        private void ApplyFilter(FilterParams p)
        {
            Task.Run(async () =>
            {
                var smallImage = await _imageFilter.Filter(_smallImage, p);
                DisplayedImage = smallImage.ToBitmapImage();

                var image = await _imageFilter.Filter(_originalImage, p);
                DisplayedImage = image.ToBitmapImage();
            });
        }

        private void LoadImage(object obj)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files |*.jpeg;*.jpg;*.gif;*.png;*.bmp"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var path = openFileDialog.FileName;
                _originalImage = new Bitmap(path);

                const int maxSize = 500;
                var k = (float) maxSize / Math.Max(_originalImage.Width, _originalImage.Height);
                if (k < 1)
                    _smallImage = (Bitmap) _originalImage.GetThumbnailImage(
                        (int) (_originalImage.Width * k),
                        (int) (_originalImage.Height * k),
                        myCallback, IntPtr.Zero);
                else _smallImage = _originalImage;

                DisplayedImage = _originalImage.ToBitmapImage();
            }
        }

        #region properties

        public ICommand LoadImageCommand { get; }

        //todo: use only one filter params
        public TestThresholdParams TestThresholdParams { get; } = new TestThresholdParams();
        public BradleysThresholdParams BradleysThresholdParams { get; } = new BradleysThresholdParams();
        public GaussianBlurParams GaussianFilterParams { get; } = new GaussianBlurParams();

        private FiltersEnum _selectedFilter;

        public FiltersEnum SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (SetProperty(ref _selectedFilter, value))
                {
                    _imageFilter = FilterFactory.GetFilter(value);
                }
            }
        }

        private Bitmap _smallImage;
        private Bitmap _originalImage;

        private ImageSource _displayedImage;

        public ImageSource DisplayedImage
        {
            get => _displayedImage;
            set => SetProperty(ref _displayedImage, value);
        }

        #endregion
    }
}
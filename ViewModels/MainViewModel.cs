using System;
using System.Drawing;
using System.Threading;
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
        // maximum width or height before image is downscaled
        private const int _maxImageWidthHeight = 1000;
        private const int _downscaleTo = 500;
        private readonly Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
        private CancellationTokenSource _cancellationTokenSource;
        private ImageFilter _imageFilter;

        public MainViewModel()
        {
            LoadImageCommand = new Command(LoadImage);
            //todo automate this:
            TestThresholdParams.OnUpdate = ApplyFilter;
            BradleysThresholdParams.OnUpdate = ApplyFilter;
            GaussianFilterParams.OnUpdate = ApplyFilter;
        }

        //todo: refactor this
        public static bool ThumbnailCallback()
        {
            return false;
        }

        private void ApplyFilter(FilterParams p)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var ct = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                if (_smallImage != null)
                {
                    var smallImage = await _imageFilter.Filter(_smallImage, p, ct);
                    DisplayedImage = smallImage.ToBitmapImage();
                    // small delay so it doesnt give "epilepsy" when displaying
                    // constantly altering smallImage and Image
                    await Task.Delay(100, ct);
                }

                ct.ThrowIfCancellationRequested();

                var image = await _imageFilter.Filter(_originalImage, p, ct);
                DisplayedImage = image.ToBitmapImage();
            }, ct);
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

                var widthHeight = Math.Max(_originalImage.Width, _originalImage.Height);
                if (widthHeight > _maxImageWidthHeight)
                {
                    var k = (float)_downscaleTo / widthHeight;
                    _smallImage = (Bitmap) _originalImage.GetThumbnailImage(
                        (int)(_originalImage.Width * k),
                        (int)(_originalImage.Height * k),
                        myCallback, IntPtr.Zero);
                } else _smallImage = null;

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
                if (SetProperty(ref _selectedFilter, value)) _imageFilter = FilterFactory.GetFilter(value);
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
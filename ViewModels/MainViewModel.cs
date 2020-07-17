using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private const int _maxImageWidthHeight = 1500;
        private const int _downscaleTo = 500;

        private CancellationTokenSource _cancellationTokenSource;
        private ImageFilter _imageFilter = new NoFilter();

        public MainViewModel()
        {
            LoadImageCommand = new Command(LoadImage);
            LoadKernelCommand = new Command(LoadKernel);
            SaveKernelCommand = new Command(SaveKernel);
            ProcessImageCommand = new Command(o => ApplyFilter(FilterParams));
        }

        private void ApplyFilter(FilterParams p)
        {
            IsBusy = true;
            IsPreview = true;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var ct = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                if (_smallImage != null)
                {
                    DisplayedImage = await _imageFilter.Filter(_smallImage, p, ct);
                    // small delay so it doesnt give "epilepsy" when displaying
                    // constantly altering smallImage and Image
                    await Task.Delay(100, ct);
                }

                ct.ThrowIfCancellationRequested(); //no need to catch this or other ct exceptions in this case

                var image = await _imageFilter.Filter(_originalImage, p, ct);

                DisplayedImage = image;
                IsBusy = false;
                IsPreview = false;
            }, ct);
        }

        private void LoadImage(object obj)
        {
            IsBusy = true;

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
                    var scale = (float) _downscaleTo / widthHeight;
                    _smallImage = _originalImage.ResizeImage(scale);
                }
                else
                {
                    _smallImage = null;
                }

                IsImageLoaded = true;
                DisplayedImage = _originalImage;
                ApplyFilter(FilterParams);
            }

            IsBusy = false;
        }

        #region kernel load save

        private void LoadKernel(object obj)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Kernel | *.kernel"
            };
            if (openFileDialog.ShowDialog() == true)
                using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    var bformatter = new BinaryFormatter();
                    var kernel = (float[][]) bformatter.Deserialize(stream);
                    ((GaussianBlurParams) FilterParams).Kernel = kernel;
                    OnPropertyChanged(nameof(FilterParams));
                }
        }

        private void SaveKernel(object obj)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Kernel | *.kernel"
            };

            if (saveFileDialog.ShowDialog() == true)
                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    var bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, ((GaussianBlurParams) FilterParams).Kernel);
                }
        }

        #endregion

        #region properties

        public ICommand LoadKernelCommand { get; }
        public ICommand LoadImageCommand { get; }
        public ICommand SaveKernelCommand { get; }
        public ICommand ProcessImageCommand { get; }

        private FilterParams _filterParams;
        public FilterParams FilterParams
        {
            get => _filterParams;
            set => SetProperty(ref _filterParams, value);
        }

        private FiltersEnum _selectedFilter;
        public FiltersEnum SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (SetProperty(ref _selectedFilter, value))
                {
                    (_imageFilter, FilterParams) = FilterFactory.GetFilter(value);
                    if (FilterParams != null)
                        FilterParams.OnUpdate += ApplyFilter;
                    ApplyFilter(FilterParams);
                }
            }
        }

        private Bitmap _smallImage;
        private Bitmap _originalImage;

        private Bitmap _displayedImage;
        public Bitmap DisplayedImage
        {
            get => _displayedImage;
            set => SetProperty(ref _displayedImage, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _isPreview;
        public bool IsPreview
        {
            get => _isPreview;
            set => SetProperty(ref _isPreview, value);
        }

        private bool _isImageLoaded;
        public bool IsImageLoaded
        {
            get => _isImageLoaded;
            set => SetProperty(ref _isImageLoaded, value);
        }

        #endregion
    }
}
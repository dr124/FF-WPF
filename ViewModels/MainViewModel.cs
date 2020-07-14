using System.Drawing;
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
        private ImageFilter _imageFilter;

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
            DisplayedImage = _imageFilter?.Filter(_originalImage, p).ToBitmapImage();
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
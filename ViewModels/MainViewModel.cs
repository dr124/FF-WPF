using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using FF_WPF.Commands;
using FF_WPF.Filters;
using FF_WPF.Models;
using FF_WPF.Utils;
using Microsoft.Win32;

namespace FF_WPF.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private FilterParamConsumer _filter;

        public MainViewModel()
        {
            LoadImageCommand = new Command(LoadImage);
            TestThresholdParams.OnUpdate = p =>
                DisplayedImage = _filter?.Filter(_originalImage, p).ToBitmapImage();
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

        public TestThresholdParams TestThresholdParams { get; } = new TestThresholdParams();

        private FiltersEnum _selectedFilter;

        public FiltersEnum SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (SetProperty(ref _selectedFilter, value))
                    _filter = FilterFactory.GetFilter(value);
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
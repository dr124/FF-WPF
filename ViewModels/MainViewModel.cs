using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FF.WPF.Filters;
using FF.WPF.Utils;
using Microsoft.Win32;

namespace FF.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    // maximum width or height before image is downscaled
    private const int _maxImageWidthHeight = 1500;
    private const int _downscaleTo = 500;

    private CancellationTokenSource _cancellationTokenSource;
    private ImageFilter _imageFilter = new NoFilter();

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

    [RelayCommand]
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
            DisplayBitmap(new Bitmap(path));
        }

        IsBusy = false;
    }

    private void DisplayBitmap(Bitmap bitmap)
    {
        _originalImage = bitmap;
        var widthHeight = Math.Max(_originalImage.Width, _originalImage.Height);
        if (widthHeight > _maxImageWidthHeight)
        {
            var scale = (float)_downscaleTo / widthHeight;
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

    [RelayCommand]
    private void ProcessImage() => ApplyFilter(FilterParams);

    [RelayCommand]
    private void PasteImage()
    {
        if (Clipboard.ContainsImage())
        {
            var imageSource = Clipboard.GetImage();
            if (imageSource != null)
            {
                DisplayBitmap(BitmapFromSource(imageSource));
            }
        }
    }

    [RelayCommand]
    private void SaveImage()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Image files |*.jpeg;*.jpg;*.gif;*.png;*.bmp"
        };
        if (saveFileDialog.ShowDialog() == true)
        {
            var path = saveFileDialog.FileName;
            DisplayedImage.Save(path);
        }
    }

    [RelayCommand]
    private void CopyImage()
    {
        var imageSource = DisplayedImage.ToImageSource();
        Clipboard.SetImage(imageSource);
    }

    [ObservableProperty]
    private FilterParams? _filterParams;
    
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

    [ObservableProperty]
    private Bitmap _displayedImage;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isPreview;

    [ObservableProperty]
    private bool _isImageLoaded;

    public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
    {
        Bitmap bitmap;
        using (MemoryStream outStream = new MemoryStream())
        {
            // from System.Windows.Media.Imaging BitmapEncoder
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapsource));
            enc.Save(outStream);
            bitmap = new Bitmap(outStream);
        }
        return bitmap;
    }
}
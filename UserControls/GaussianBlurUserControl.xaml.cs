using System.Windows;
using System.Windows.Controls;
using FF_WPF.Filters.Implementations;

namespace FF_WPF.UserControls
{
    public partial class GaussianBlurUserControl : UserControl
    {
        public GaussianBlurUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FilterParamsProperty = DependencyProperty.Register(
            "FilterParams", typeof(GaussianBlurParams), typeof(GaussianBlurUserControl),
            new PropertyMetadata(default(GaussianBlurParams)));

        public GaussianBlurParams FilterParams
        {
            get => (GaussianBlurParams) GetValue(FilterParamsProperty);
            set => SetValue(FilterParamsProperty, value);
        }
    }
}
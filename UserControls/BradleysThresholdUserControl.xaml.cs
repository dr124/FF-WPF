using System.Windows;
using System.Windows.Controls;
using FF_WPF.Filters.Implementations;

namespace FF_WPF.UserControls
{
    public partial class BradleysThresholdUserControl : UserControl
    {
        public BradleysThresholdUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FilterParamsProperty = DependencyProperty.Register(
            "FilterParams", typeof(BradleysThresholdParams), typeof(BradleysThresholdUserControl),
            new PropertyMetadata(default(BradleysThresholdParams)));

        public BradleysThresholdParams FilterParams
        {
            get => (BradleysThresholdParams) GetValue(FilterParamsProperty);
            set => SetValue(FilterParamsProperty, value);
        }
    }
}
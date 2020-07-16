using System.Windows;
using System.Windows.Controls;
using FF_WPF.Filters.Implementations;

namespace FF_WPF.UserControls
{
    public partial class TestThresholdUserControl : UserControl
    {
        public TestThresholdUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FilterParamsProperty = DependencyProperty.Register(
            "FilterParams", typeof(TestThresholdParams), typeof(TestThresholdUserControl),
            new PropertyMetadata(default(TestThresholdParams)));

        public TestThresholdParams FilterParams
        {
            get => (TestThresholdParams) GetValue(FilterParamsProperty);
            set => SetValue(FilterParamsProperty, value);
        }
    }
}
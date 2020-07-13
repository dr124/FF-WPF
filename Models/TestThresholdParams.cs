using FF_WPF.ViewModels;

namespace FF_WPF.Models
{
    public class TestThresholdParams : FilterParams
    {
        private float _ratio;

        public float Ratio
        {
            get => _ratio;
            set => SetPropertyAndNotify(ref _ratio, value);
        }
    }
}
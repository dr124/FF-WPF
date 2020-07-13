namespace FF_WPF.Filters.Implementations
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
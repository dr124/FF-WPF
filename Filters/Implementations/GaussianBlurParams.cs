namespace FF_WPF.Filters.Implementations
{
    public class GaussianBlurParams : FilterParams
    {
        private float[][] _kernel = new float[3][]
        {
            new[] {0, .2f, 0},
            new[] {.2f, .2f, .2f},
            new[] {0, .2f, 0}
        };

        public float[][] Kernel
        {
            get => _kernel;
            set => SetPropertyAndNotify(ref _kernel, value);
        }
    }
}
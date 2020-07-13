using FF_WPF.ViewModels;

namespace FF_WPF.Models
{
    public class BradleysThresholdParams : FilterParams
    {
        private int _t = 50;
        public int T
        {
            get => _t;
            set => SetPropertyAndNotify(ref _t, value);
        }

        private int _s = 2;
        public int S
        {
            get => _s;
            set => SetPropertyAndNotify(ref _s, value);
        }
    }
}
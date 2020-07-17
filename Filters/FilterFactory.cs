using System.ComponentModel;
using FF_WPF.Filters.Implementations;

namespace FF_WPF.Filters
{
    public class FilterFactory
    {
        public static (ImageFilter, FilterParams) GetFilter(FiltersEnum selectedFilter)
        {
            switch (selectedFilter)
            {
                case FiltersEnum.NoFilter:
                    return (new NoFilter(), null);
                case FiltersEnum.TestThreshold:
                    return (new TestThresholdFilter(), new TestThresholdParams());
                case FiltersEnum.BradleyThresholding:
                    return (new BradleysThresholdFilter(), new BradleysThresholdParams());
                case FiltersEnum.GaussianBlur:
                    return (new GaussianBlurFilter(), new GaussianBlurParams());
                default:
                    throw new InvalidEnumArgumentException("This filter does not exist");
            }
        }
    }
}
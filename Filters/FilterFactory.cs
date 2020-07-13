using System.ComponentModel;
using FF_WPF.Filters.Implementations;

namespace FF_WPF.Filters
{
    public class FilterFactory
    {
        public static ImageFilter GetFilter(FiltersEnum selectedFilter)
        {
            switch (selectedFilter)
            {
                case FiltersEnum.TestThreshold:
                    return new TestThresholdFilter();
                case FiltersEnum.BradleyThresholding:
                    return new BradleysThresholdFilter();
                case FiltersEnum.GaussianBlur:
                    return new GaussianBlurFilter();
                default:
                    throw new InvalidEnumArgumentException("This filter does not exist");
            }
        }
    }
}
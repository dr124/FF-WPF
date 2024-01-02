using System.ComponentModel;
using FF.WPF.Filters.Implementations;

namespace FF.WPF.Filters;

public class FilterFactory
{
    public static (ImageFilter, FilterParams) GetFilter(FiltersEnum selectedFilter)
    {
        return selectedFilter switch
        {
            FiltersEnum.NoFilter => (new NoFilter(), null),
            FiltersEnum.TestThreshold => (new TestThresholdFilter(), new TestThresholdParams()),
            FiltersEnum.BradleyThresholding => (new BradleysThresholdFilter(), new BradleysThresholdParams()),
            FiltersEnum.GaussianBlur => (new GaussianBlurFilter(), new GaussianBlurParams()),
            _ => throw new InvalidEnumArgumentException("This filter does not exist")
        };
    }
}
using System.ComponentModel;
using FF_WPF.Models;
using FF_WPF.ViewModels;

namespace FF_WPF.Filters
{
    public class FilterFactory
    {
        public static FilterParamConsumer GetFilter(FiltersEnum selectedFilter)
        {
            switch (selectedFilter)
            {
                case FiltersEnum.TestThreshold:
                    return new TestThresholdFilter();
                default:
                    throw new InvalidEnumArgumentException("This filter does not exist");
            }
        }
    }
}
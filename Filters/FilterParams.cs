using System;
using System.Runtime.CompilerServices;
using FF_WPF.ViewModels;

namespace FF_WPF.Filters
{
    public abstract class FilterParams : ObservableObject
    {
        public Action<FilterParams> OnUpdate { get; set; }

        protected void SetPropertyAndNotify<T>(ref T variable, T value, [CallerMemberName] string propertyName = "")
        {
            if (SetProperty(ref variable, value, propertyName))
                OnUpdate?.Invoke(this);
        }
    }
}
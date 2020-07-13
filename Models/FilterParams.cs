using System;
using System.Runtime.CompilerServices;

namespace FF_WPF.ViewModels
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FF_WPF.ViewModels
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T variable, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(variable, value))
                return false;

            variable = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
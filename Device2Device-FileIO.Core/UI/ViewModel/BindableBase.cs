using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T value,
            // use the name of the calling property if none given:
            [CallerMemberName] string name = null,
            // allow to pass additional property names if needed
            params string[] otherNames)
        {
            if (Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(name);
            foreach (var n in otherNames)
            {
                OnPropertyChanged(n);
            }
            return true;
        }
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

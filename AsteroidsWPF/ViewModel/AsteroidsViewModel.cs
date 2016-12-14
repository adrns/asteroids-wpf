using Asteroids.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AsteroidsWPF.ViewModel
{
    class AsteroidsViewModel : INotifyPropertyChanged
    {
        private AsteroidsGame model;

        public event PropertyChangedEventHandler PropertyChanged;

        public AsteroidsViewModel(AsteroidsGame model)
        {
            this.model = model;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

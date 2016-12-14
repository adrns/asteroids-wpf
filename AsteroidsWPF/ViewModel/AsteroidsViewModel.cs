using Asteroids.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AsteroidsWPF.ViewModel
{
    class AsteroidsViewModel : INotifyPropertyChanged
    {
        private AsteroidsGame model;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Exit;
        public DelegateCommand EnterCommand { get; private set; }
        public DelegateCommand SpaceCommand { get; private set; }
        public DelegateCommand EscCommand { get; private set; }
        public DelegateCommand UpCommand { get; private set; }
        public DelegateCommand LeftCommand { get; private set; }
        public DelegateCommand DownCommand { get; private set; }
        public DelegateCommand RightCommand { get; private set; }

        public AsteroidsViewModel(AsteroidsGame model)
        {
            this.model = model;
            EnterCommand = new DelegateCommand(param => model.start());
            SpaceCommand = new DelegateCommand(param => model.pause());
            EscCommand = new DelegateCommand(param => OnExit());
            UpCommand = new DelegateCommand(param => model.upPressed());
            LeftCommand = new DelegateCommand(param => model.leftPressed());
            DownCommand = new DelegateCommand(param => model.downPressed());
            RightCommand = new DelegateCommand(param => model.rightPressed());
        }

        private void OnExit()
        {
            Exit?.Invoke(this, EventArgs.Empty);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using Asteroids.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace AsteroidsWPF.ViewModel
{
    class AsteroidsViewModel : INotifyPropertyChanged
    {
        class VisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool visible = (bool)value;
                return visible ? true : false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (Visibility)value == Visibility.Visible;
            }
        }

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
        public DelegateCommand UpReleasedCommand { get; private set; }
        public DelegateCommand LeftReleasedCommand { get; private set; }
        public DelegateCommand DownReleasedCommand { get; private set; }
        public DelegateCommand RightReleasedCommand { get; private set; }

        private bool menuVisibility;
        private bool pauseVisiblity;
        private bool gameVisibility;
        private bool gameOverVisibility;

        public bool MenuVisibility { get { return menuVisibility; } private set { menuVisibility = value; NotifyPropertyChanged(); } }
        public bool PauseVisibility { get { return pauseVisiblity; } private set { pauseVisiblity = value; NotifyPropertyChanged(); } }
        public bool GameVisibility { get { return gameVisibility; } private set { gameVisibility = value; NotifyPropertyChanged(); } }
        public bool GameOverVisibility { get { return gameOverVisibility; } private set { gameOverVisibility = value; NotifyPropertyChanged(); } }

        private string elapsedTime;
        public string ElapsedTime { get { return elapsedTime; } private set { elapsedTime = value; NotifyPropertyChanged(); } }

        public ObservableCollection<Asteroid> Asteroids { get; private set; }

        private SpaceShip player;
        public SpaceShip Player { get { return player; } private set { player = value; NotifyPropertyChanged(); } }

        public AsteroidsViewModel(AsteroidsGame model)
        {
            this.model = model;
            model.OnGameStateChange += Model_OnGameStateChange;
            model.OnFrameUpdate += Model_OnFrameUpdate;
            EnterCommand = new DelegateCommand(param =>
            {
                if (GameOverVisibility)
                {
                    GameOverVisibility = PauseVisibility = GameVisibility = false;
                    MenuVisibility = true;
                }
                else
                {
                    model.start();
                }
            });
            MenuVisibility = true;
            GameVisibility = false;
            GameOverVisibility = false;
            PauseVisibility = false;
            SpaceCommand = new DelegateCommand(param => { if (model.GameState == AsteroidsGame.EGameState.Paused) model.resume(); else model.pause(); });
            EscCommand = new DelegateCommand(param => OnExit());
            UpCommand = new DelegateCommand(param => model.upPressed());
            LeftCommand = new DelegateCommand(param => model.leftPressed());
            DownCommand = new DelegateCommand(param => model.downPressed());
            RightCommand = new DelegateCommand(param => model.rightPressed());
            UpReleasedCommand = new DelegateCommand(param => model.upReleased());
            LeftReleasedCommand = new DelegateCommand(param => model.leftReleased());
            DownReleasedCommand = new DelegateCommand(param => model.downReleased());
            RightReleasedCommand = new DelegateCommand(param => model.rightReleased());
            Asteroids = new ObservableCollection<Asteroid>();
        }

        private void Model_OnFrameUpdate(object sender, FrameEventArgs e)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                Asteroids.Clear();
                var sec = e.Seconds % 60;
                var min = e.Seconds / 60;
                ElapsedTime = min.ToString().PadLeft(2, '0') + ":" + sec.ToString().PadLeft(2, '0');
                foreach (Asteroid asteroid in e.Asteroids) Asteroids.Add(asteroid);
                Player = e.Player;
            });
        }

        private void Model_OnGameStateChange(object sender, GameStateEventArgs e)
        {
            switch (e.GameState)
            {
                case (AsteroidsGame.EGameState.Running):
                    {
                        GameVisibility = true;
                        MenuVisibility = GameOverVisibility = PauseVisibility = false;
                        break;
                    }
                case (AsteroidsGame.EGameState.Paused):
                    {
                        GameVisibility = PauseVisibility = true;
                        MenuVisibility = GameOverVisibility = false;
                        break;
                    }
                case (AsteroidsGame.EGameState.GameOver):
                    {
                        GameVisibility = GameOverVisibility = true;
                        MenuVisibility = PauseVisibility = false;
                        break;
                    }
            }
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

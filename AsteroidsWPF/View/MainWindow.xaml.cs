using AsteroidsWPF.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace AsteroidsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (null == DataContext) return;
            var viewModel = DataContext as AsteroidsViewModel;
            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    viewModel.UpReleasedCommand.Execute(null);
                    break;
                case Key.A:
                case Key.Left:
                    viewModel.LeftReleasedCommand.Execute(null);
                    break;
                case Key.S:
                case Key.Down:
                    viewModel.DownReleasedCommand.Execute(null);
                    break;
                case Key.D:
                case Key.Right:
                    viewModel.RightReleasedCommand.Execute(null);
                    break;
            }
        }
    }
}

using Asteroids.Model;
using AsteroidsWPF.ViewModel;
using System.Windows;

namespace AsteroidsWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AsteroidsGame model;
        private AsteroidsViewModel viewModel;
        private MainWindow view;
        private const int FPS = 60;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            view = new MainWindow();
            model = new AsteroidsGame(new GameRules(view.Width, view.Height), FPS);
            viewModel = new AsteroidsViewModel(model);

            view.Show();
        }
    }
}

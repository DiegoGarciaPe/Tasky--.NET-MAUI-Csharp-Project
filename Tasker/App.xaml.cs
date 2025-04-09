using Tasker.MVVM.Views;

namespace Tasker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;

            MainPage = new NavigationPage(new MainView());
        }
    }
}

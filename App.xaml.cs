using System.Globalization;

namespace Journal;

public partial class App : Application
{
	public App()
	{
        InitializeComponent();

        CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
        Application.Current.UserAppTheme = AppTheme.Light;
        MainPage = new AppShell();
	}
}

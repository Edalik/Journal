using Journal.Views;

namespace Journal;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(MainView), typeof(MainView));
    }
}

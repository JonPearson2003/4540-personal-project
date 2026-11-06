using BuildTrack.App.Views;

namespace BuildTrack.App;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(CreateUsersPage), typeof(CreateUsersPage));
		Routing.RegisterRoute(nameof(EditUsersPage), typeof(EditUsersPage));
	}
}
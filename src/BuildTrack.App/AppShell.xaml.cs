using BuildTrack.App.Views;

namespace BuildTrack.App;

public sealed class AppShell : Shell
{
	public AppShell(UsersPage usersPage)
	{
		Title = "BuildTrack";
		FlyoutBehavior = FlyoutBehavior.Disabled;
		Shell.SetBackgroundColor(this, Color.FromArgb("#006D77"));
		Shell.SetForegroundColor(this, Colors.White);

		var tab = new Tab
		{
			Title = "Users",
			Items =
			{
				new ShellContent
				{
					Title = "Users",
					Route = "users",
					Content = usersPage
				}
			}
		};
		Items.Add(new TabBar { Items = { tab } });

		Routing.RegisterRoute(nameof(CreateUsersPage), typeof(CreateUsersPage));
		Routing.RegisterRoute(nameof(EditUsersPage), typeof(EditUsersPage));
	}
}
using BuildTrack.App.Models;
using BuildTrack.App.Services;
using BuildTrack.App.ViewModels;
using BuildTrack.App.Views;

#if MAUI_DEVFLOW
using Microsoft.Maui.DevFlow.Agent;
#endif

namespace BuildTrack.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if MAUI_DEVFLOW
		builder.AddMauiDevFlowAgent();
#endif

		builder.Services.AddHttpClient<UsersApiService>(client =>
		{
			client.BaseAddress = new Uri(ApiSettings.BaseUrl);
			client.Timeout = TimeSpan.FromSeconds(15);
		});

		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<UsersViewModel>();
		builder.Services.AddTransient<CreateUsersViewModel>();
		builder.Services.AddTransient<EditUsersViewModel>();
		builder.Services.AddSingleton<UsersPage>();
		builder.Services.AddTransient<CreateUsersPage>();
		builder.Services.AddTransient<EditUsersPage>();
		return builder.Build();
	}
}
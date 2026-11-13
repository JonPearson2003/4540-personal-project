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
		builder.Services.AddHttpClient<ProjectsApiService>(ConfigureApiClient);
		builder.Services.AddHttpClient<TimeRecordsApiService>(ConfigureApiClient);
		builder.Services.AddHttpClient<ProjectMaterialsApiService>(ConfigureApiClient);
		builder.Services.AddHttpClient<ProjectStatusesApiService>(ConfigureApiClient);
		builder.Services.AddHttpClient<ProjectAssignmentsApiService>(ConfigureApiClient);
		builder.Services.AddHttpClient<MaterialRequestsApiService>(ConfigureApiClient);
		builder.Services.AddHttpClient<MaterialUsagesApiService>(ConfigureApiClient);
		builder.Services.AddHttpClient<ReceiptInvoicesApiService>(ConfigureApiClient);
		builder.Services.AddSingleton<CameraCaptureService>();

		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<UsersViewModel>();
		builder.Services.AddSingleton<ProjectsViewModel>();
		builder.Services.AddSingleton<TimeRecordsViewModel>();
		builder.Services.AddSingleton<ProjectMaterialsViewModel>();
		builder.Services.AddSingleton<ProjectStatusesViewModel>();
		builder.Services.AddSingleton<ProjectAssignmentsViewModel>();
		builder.Services.AddSingleton<MaterialRequestsViewModel>();
		builder.Services.AddSingleton<MaterialUsagesViewModel>();
		builder.Services.AddSingleton<ReceiptInvoicesViewModel>();
		builder.Services.AddTransient<CreateUsersViewModel>();
		builder.Services.AddTransient<EditUsersViewModel>();
		builder.Services.AddTransient<ProjectFormViewModel>();
		builder.Services.AddTransient<TimeRecordFormViewModel>();
		builder.Services.AddTransient<ProjectMaterialFormViewModel>();
		builder.Services.AddTransient<ProjectStatusFormViewModel>();
		builder.Services.AddTransient<ProjectAssignmentFormViewModel>();
		builder.Services.AddTransient<MaterialRequestFormViewModel>();
		builder.Services.AddTransient<MaterialUsageFormViewModel>();
		builder.Services.AddTransient<ReceiptInvoiceFormViewModel>();
		builder.Services.AddSingleton<UsersPage>();
		builder.Services.AddSingleton<ProjectsPage>();
		builder.Services.AddSingleton<TimeRecordsPage>();
		builder.Services.AddSingleton<ProjectMaterialsPage>();
		builder.Services.AddSingleton<ProjectStatusesPage>();
		builder.Services.AddSingleton<ProjectAssignmentsPage>();
		builder.Services.AddSingleton<MaterialRequestsPage>();
		builder.Services.AddSingleton<MaterialUsagesPage>();
		builder.Services.AddSingleton<ReceiptInvoicesPage>();
		builder.Services.AddTransient<CreateUsersPage>();
		builder.Services.AddTransient<EditUsersPage>();
		builder.Services.AddTransient<ProjectFormPage>();
		builder.Services.AddTransient<TimeRecordFormPage>();
		builder.Services.AddTransient<ProjectMaterialFormPage>();
		builder.Services.AddTransient<ProjectStatusFormPage>();
		builder.Services.AddTransient<ProjectAssignmentFormPage>();
		builder.Services.AddTransient<MaterialRequestFormPage>();
		builder.Services.AddTransient<MaterialUsageFormPage>();
		builder.Services.AddTransient<ReceiptInvoiceFormPage>();
		return builder.Build();
	}

	private static void ConfigureApiClient(HttpClient client)
	{
		client.BaseAddress = new Uri(ApiSettings.BaseUrl);
		client.Timeout = TimeSpan.FromSeconds(15);
	}
}
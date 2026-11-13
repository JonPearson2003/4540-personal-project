using BuildTrack.App.Views;

namespace BuildTrack.App;

public sealed class AppShell : Shell
{
	public AppShell(
		UsersPage usersPage,
		ProjectsPage projectsPage,
		TimeRecordsPage timeRecordsPage,
		ProjectMaterialsPage projectMaterialsPage,
		ProjectStatusesPage projectStatusesPage,
		ProjectAssignmentsPage projectAssignmentsPage,
		MaterialRequestsPage materialRequestsPage,
		MaterialUsagesPage materialUsagesPage,
		ReceiptInvoicesPage receiptInvoicesPage)
	{
		Title = "BuildTrack";
		FlyoutBehavior = FlyoutBehavior.Flyout;
		Shell.SetBackgroundColor(this, Color.FromArgb("#006D77"));
		Shell.SetForegroundColor(this, Colors.White);

		var flyout = new FlyoutItem
		{
			Title = "BuildTrack",
			Items =
			{
				CreateContent("Users", "users", usersPage),
				CreateContent("Projects", "projects", projectsPage),
				CreateContent("Time records", "timerecords", timeRecordsPage),
				CreateContent("Project materials", "projectmaterials", projectMaterialsPage),
				CreateContent("Project updates", "projectstatuses", projectStatusesPage),
				CreateContent("Assignments", "projectassignments", projectAssignmentsPage),
				CreateContent("Material requests", "materialrequests", materialRequestsPage),
				CreateContent("Material usage", "materialusages", materialUsagesPage),
				CreateContent("Receipts", "receiptinvoices", receiptInvoicesPage)
			}
		};
		Items.Add(flyout);

		Routing.RegisterRoute(nameof(CreateUsersPage), typeof(CreateUsersPage));
		Routing.RegisterRoute(nameof(EditUsersPage), typeof(EditUsersPage));
		Routing.RegisterRoute(nameof(ProjectFormPage), typeof(ProjectFormPage));
		Routing.RegisterRoute(nameof(TimeRecordFormPage), typeof(TimeRecordFormPage));
		Routing.RegisterRoute(nameof(ProjectMaterialFormPage), typeof(ProjectMaterialFormPage));
		Routing.RegisterRoute(nameof(ProjectStatusFormPage), typeof(ProjectStatusFormPage));
		Routing.RegisterRoute(nameof(ProjectAssignmentFormPage), typeof(ProjectAssignmentFormPage));
		Routing.RegisterRoute(nameof(MaterialRequestFormPage), typeof(MaterialRequestFormPage));
		Routing.RegisterRoute(nameof(MaterialUsageFormPage), typeof(MaterialUsageFormPage));
		Routing.RegisterRoute(nameof(ReceiptInvoiceFormPage), typeof(ReceiptInvoiceFormPage));
	}

	private static ShellContent CreateContent(string title, string route, ContentPage page) =>
		new()
		{
			Title = title,
			Route = route,
			Content = page
		};
}
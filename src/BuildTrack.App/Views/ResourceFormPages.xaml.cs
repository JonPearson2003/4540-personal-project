using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class ProjectFormPage : ContentPage
{
	private readonly ProjectFormViewModel viewModel;

	public ProjectFormPage(ProjectFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

public partial class TimeRecordFormPage : ContentPage
{
	private readonly TimeRecordFormViewModel viewModel;

	public TimeRecordFormPage(TimeRecordFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

public partial class ProjectMaterialFormPage : ContentPage
{
	private readonly ProjectMaterialFormViewModel viewModel;

	public ProjectMaterialFormPage(ProjectMaterialFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

public partial class ProjectStatusFormPage : ContentPage
{
	private readonly ProjectStatusFormViewModel viewModel;

	public ProjectStatusFormPage(ProjectStatusFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

public partial class ProjectAssignmentFormPage : ContentPage
{
	private readonly ProjectAssignmentFormViewModel viewModel;

	public ProjectAssignmentFormPage(ProjectAssignmentFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

public partial class MaterialRequestFormPage : ContentPage
{
	private readonly MaterialRequestFormViewModel viewModel;

	public MaterialRequestFormPage(MaterialRequestFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

public partial class MaterialUsageFormPage : ContentPage
{
	private readonly MaterialUsageFormViewModel viewModel;

	public MaterialUsageFormPage(MaterialUsageFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

public partial class ReceiptInvoiceFormPage : ContentPage
{
	private readonly ReceiptInvoiceFormViewModel viewModel;

	public ReceiptInvoiceFormPage(ReceiptInvoiceFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

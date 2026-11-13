using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class TimeRecordsPage : ContentPage
{
	private readonly TimeRecordsViewModel viewModel;

	public TimeRecordsPage(TimeRecordsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.LoadAsync();
	}
}

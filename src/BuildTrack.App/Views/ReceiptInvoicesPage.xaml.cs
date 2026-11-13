using BuildTrack.App.ViewModels;

namespace BuildTrack.App.Views;

public partial class ReceiptInvoicesPage : ContentPage
{
	private readonly ReceiptInvoicesViewModel viewModel;

	public ReceiptInvoicesPage(ReceiptInvoicesViewModel viewModel)
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

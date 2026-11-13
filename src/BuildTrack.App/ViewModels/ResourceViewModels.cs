using System.Globalization;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BuildTrack.App.Services;
using BuildTrack.App.Views;
using BuildTrack.Contracts.Models;

namespace BuildTrack.App.ViewModels;

public sealed class ProjectsViewModel(ProjectsApiService api)
	: CrudListViewModel<ProjectDto, CreateProjectRequest, UpdateProjectRequest>(
		api, nameof(ProjectFormPage), item => item.ProjectId.ToString(CultureInfo.InvariantCulture));

public partial class ProjectFormViewModel(ProjectsApiService api)
	: CrudFormViewModel<ProjectDto, CreateProjectRequest, UpdateProjectRequest>(api)
{
	[ObservableProperty] private string name = string.Empty;
	[ObservableProperty] private string description = string.Empty;
	[ObservableProperty] private string location = string.Empty;
	[ObservableProperty] private DateTime startDate = DateTime.Today;
	[ObservableProperty] private DateTime endDate = DateTime.Today.AddMonths(1);

	protected override void Apply(ProjectDto item)
	{
		Name = item.Name;
		Description = item.Description;
		Location = item.Location;
		StartDate = item.StartDate;
		EndDate = item.EndDate;
	}

	protected override CreateProjectRequest CreateCreateRequest() => new()
	{
		Name = Name.Trim(),
		Description = Description.Trim(),
		Location = Location.Trim(),
		StartDate = StartDate,
		EndDate = EndDate
	};

	protected override UpdateProjectRequest CreateUpdateRequest() => new()
	{
		Name = Name.Trim(),
		Description = Description.Trim(),
		Location = Location.Trim(),
		StartDate = StartDate,
		EndDate = EndDate
	};

	protected override bool Validate()
	{
		if (string.IsNullOrWhiteSpace(Name) || Name.Trim().Length is < 2 or > 100)
			ErrorMessage = "Enter a project name between 2 and 100 characters.";
		else if (string.IsNullOrWhiteSpace(Description) || Description.Trim().Length is < 10 or > 250)
			ErrorMessage = "Enter a description between 10 and 250 characters.";
		else if (string.IsNullOrWhiteSpace(Location) || Location.Trim().Length is < 6 or > 100)
			ErrorMessage = "Enter a location between 6 and 100 characters.";
		else if (EndDate < StartDate)
			ErrorMessage = "The end date must be on or after the start date.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}
}

public sealed class TimeRecordsViewModel(TimeRecordsApiService api)
	: CrudListViewModel<TimeRecordDto, CreateTimeRecordRequest, UpdateTimeRecordRequest>(
		api, nameof(TimeRecordFormPage), item => item.TimeRecordId.ToString(CultureInfo.InvariantCulture));

public partial class TimeRecordFormViewModel(TimeRecordsApiService api)
	: CrudFormViewModel<TimeRecordDto, CreateTimeRecordRequest, UpdateTimeRecordRequest>(api)
{
	[ObservableProperty] private string userId = string.Empty;
	[ObservableProperty] private string projectId = string.Empty;
	[ObservableProperty] private DateTime startTime = DateTime.Now.AddHours(-1);
	[ObservableProperty] private TimeSpan startTimeOfDay = DateTime.Now.AddHours(-1).TimeOfDay;
	[ObservableProperty] private DateTime endTime = DateTime.Now;
	[ObservableProperty] private TimeSpan endTimeOfDay = DateTime.Now.TimeOfDay;
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsCompleted))]
	[NotifyPropertyChangedFor(nameof(DurationText))]
	private bool isInProgress;

	public bool IsCompleted => !IsInProgress;
	public string DurationText => IsInProgress
		? "In progress"
		: EndTime > StartTime
			? (EndTime - StartTime).ToString("c", CultureInfo.InvariantCulture)
			: "Enter an end time after the start time";

	partial void OnStartTimeChanged(DateTime value) => OnPropertyChanged(nameof(DurationText));
	partial void OnEndTimeChanged(DateTime value) => OnPropertyChanged(nameof(DurationText));

	partial void OnStartTimeOfDayChanged(TimeSpan value) => StartTime = StartTime.Date + value;
	partial void OnEndTimeOfDayChanged(TimeSpan value) => EndTime = EndTime.Date + value;

	protected override void Apply(TimeRecordDto item)
	{
		UserId = item.UserId.ToString(CultureInfo.InvariantCulture);
		ProjectId = item.ProjectId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
		StartTime = item.StartTime;
		StartTimeOfDay = item.StartTime.TimeOfDay;
		IsInProgress = item.EndTime is null || item.Duration is null;
		EndTime = item.EndTime ?? DateTime.Now;
		EndTimeOfDay = EndTime.TimeOfDay;
	}

	protected override CreateTimeRecordRequest CreateCreateRequest() => new()
	{
		UserId = int.Parse(UserId, CultureInfo.InvariantCulture),
		ProjectId = ParseNullableInt(ProjectId),
		StartTime = StartTime,
		EndTime = IsInProgress ? null : EndTime
	};

	protected override UpdateTimeRecordRequest CreateUpdateRequest() => new()
	{
		ProjectId = ParseNullableInt(ProjectId),
		StartTime = StartTime,
		EndTime = IsInProgress ? null : EndTime
	};

	protected override bool Validate()
	{
		if (!int.TryParse(UserId, out var parsedUserId) || parsedUserId <= 0)
			ErrorMessage = "Enter a valid user ID.";
		else if (!string.IsNullOrWhiteSpace(ProjectId)
			&& (!int.TryParse(ProjectId, out var parsedProjectId) || parsedProjectId <= 0))
			ErrorMessage = "Enter a valid project ID or leave it blank.";
		else if (!IsInProgress && EndTime <= StartTime)
			ErrorMessage = "Completed records need an end time after the start time.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}

	private static int? ParseNullableInt(string value) =>
		string.IsNullOrWhiteSpace(value) ? null : int.Parse(value, CultureInfo.InvariantCulture);
}

public sealed class ProjectMaterialsViewModel(ProjectMaterialsApiService api)
	: CrudListViewModel<ProjectMaterialDto, CreateProjectMaterialRequest, UpdateProjectMaterialRequest>(
		api, nameof(ProjectMaterialFormPage), item => item.ProjectMaterialId.ToString(CultureInfo.InvariantCulture));

public partial class ProjectMaterialFormViewModel(ProjectMaterialsApiService api)
	: CrudFormViewModel<ProjectMaterialDto, CreateProjectMaterialRequest, UpdateProjectMaterialRequest>(api)
{
	[ObservableProperty] private string projectId = string.Empty;
	[ObservableProperty] private string name = string.Empty;
	[ObservableProperty] private string description = string.Empty;
	[ObservableProperty] private string price = string.Empty;
	[ObservableProperty] private string quantity = string.Empty;

	protected override void Apply(ProjectMaterialDto item)
	{
		ProjectId = item.ProjectId.ToString(CultureInfo.InvariantCulture);
		Name = item.Name;
		Description = item.Description;
		Price = item.Price.ToString(CultureInfo.CurrentCulture);
		Quantity = item.Quantity.ToString(CultureInfo.InvariantCulture);
	}

	protected override CreateProjectMaterialRequest CreateCreateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		Name = Name.Trim(),
		Description = Description.Trim(),
		Price = decimal.Parse(Price, CultureInfo.CurrentCulture),
		Quantity = int.Parse(Quantity, CultureInfo.InvariantCulture)
	};

	protected override UpdateProjectMaterialRequest CreateUpdateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		Name = Name.Trim(),
		Description = Description.Trim(),
		Price = decimal.Parse(Price, CultureInfo.CurrentCulture),
		Quantity = int.Parse(Quantity, CultureInfo.InvariantCulture)
	};

	protected override bool Validate()
	{
		if (!int.TryParse(ProjectId, out var project) || project <= 0)
			ErrorMessage = "Enter a valid project ID.";
		else if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description))
			ErrorMessage = "Enter a material name and description.";
		else if (!decimal.TryParse(Price, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsedPrice)
			|| parsedPrice < 0)
			ErrorMessage = "Enter a non-negative price.";
		else if (!int.TryParse(Quantity, out var parsedQuantity) || parsedQuantity < 0)
			ErrorMessage = "Enter a non-negative quantity.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}
}

public sealed class ProjectStatusesViewModel(ProjectStatusesApiService api)
	: CrudListViewModel<ProjectStatusDto, CreateProjectStatusRequest, UpdateProjectStatusRequest>(
		api, nameof(ProjectStatusFormPage), item => item.ProjectStatusId.ToString(CultureInfo.InvariantCulture));

public partial class ProjectStatusFormViewModel(
	ProjectStatusesApiService api,
	CameraCaptureService cameraCapture)
	: CrudFormViewModel<ProjectStatusDto, CreateProjectStatusRequest, UpdateProjectStatusRequest>(api)
{
	[ObservableProperty] private string projectId = string.Empty;
	[ObservableProperty] private string status = string.Empty;
	[ObservableProperty] private DateTime date = DateTime.Today;
	[ObservableProperty] private string photoFileName = "No photo selected";
	private byte[] progressPhoto = [];

	protected override void Apply(ProjectStatusDto item)
	{
		ProjectId = item.ProjectId.ToString(CultureInfo.InvariantCulture);
		Status = item.status;
		Date = item.date;
		progressPhoto = item.ProgressPhoto;
		PhotoFileName = progressPhoto.Length == 0 ? "No photo selected" : "Existing photo saved";
	}

	protected override CreateProjectStatusRequest CreateCreateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		Status = Status.Trim(),
		Date = Date,
		ProgressPhoto = progressPhoto
	};

	protected override UpdateProjectStatusRequest CreateUpdateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		Status = Status.Trim(),
		Date = Date,
		ProgressPhoto = progressPhoto
	};

	protected override bool Validate()
	{
		if (!int.TryParse(ProjectId, out var projectId) || projectId <= 0)
			ErrorMessage = "Enter a valid project ID.";
		else if (string.IsNullOrWhiteSpace(Status))
			ErrorMessage = "Enter a status update.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}

	[RelayCommand]
	private async Task SelectPhotoAsync()
	{
		try
		{
			var file = await FilePicker.Default.PickAsync(new PickOptions
			{
				PickerTitle = "Choose a progress photo",
				FileTypes = FilePickerFileType.Images
			});
			if (file is null)
				return;

			await using var stream = await file.OpenReadAsync();
			using var memory = new MemoryStream();
			await stream.CopyToAsync(memory);
			progressPhoto = memory.ToArray();
			PhotoFileName = file.FileName;
			ErrorMessage = string.Empty;
		}
		catch (Exception exception) when (exception is not OperationCanceledException)
		{
			ErrorMessage = "The progress photo could not be selected.";
		}
	}

	[RelayCommand]
	private async Task CapturePhotoAsync()
	{
		try
		{
			var capture = await cameraCapture.CaptureImageAsync("Capture a progress photo");
			if (capture is null)
				return;

			progressPhoto = capture.Value.Image;
			PhotoFileName = capture.Value.FileName;
			ErrorMessage = string.Empty;
		}
		catch (Exception exception) when (exception is not OperationCanceledException)
		{
			ErrorMessage = exception is NotSupportedException
				? "Camera capture is not supported on this device."
				: "The progress photo could not be captured.";
		}
	}
}

public sealed class ProjectAssignmentsViewModel(ProjectAssignmentsApiService api)
	: CrudListViewModel<ProjectAssignmentDto, CreateProjectAssignmentRequest, UpdateProjectAssignmentRequest>(
		api, nameof(ProjectAssignmentFormPage),
		item => $"{item.ProjectId.ToString(CultureInfo.InvariantCulture)}/{item.UserId.ToString(CultureInfo.InvariantCulture)}");

public partial class ProjectAssignmentFormViewModel(ProjectAssignmentsApiService api)
	: CrudFormViewModel<ProjectAssignmentDto, CreateProjectAssignmentRequest, UpdateProjectAssignmentRequest>(api)
{
	[ObservableProperty] private string projectId = string.Empty;
	[ObservableProperty] private string userId = string.Empty;
	[ObservableProperty] private DateTime assignedAt = DateTime.Now;
	[ObservableProperty] private TimeSpan assignedAtTime = DateTime.Now.TimeOfDay;

	partial void OnAssignedAtTimeChanged(TimeSpan value) => AssignedAt = AssignedAt.Date + value;

	protected override void Apply(ProjectAssignmentDto item)
	{
		ProjectId = item.ProjectId.ToString(CultureInfo.InvariantCulture);
		UserId = item.UserId.ToString(CultureInfo.InvariantCulture);
		AssignedAt = item.AssignedAt;
		AssignedAtTime = item.AssignedAt.TimeOfDay;
	}

	protected override CreateProjectAssignmentRequest CreateCreateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		UserId = int.Parse(UserId, CultureInfo.InvariantCulture),
		AssignedAt = AssignedAt
	};

	protected override UpdateProjectAssignmentRequest CreateUpdateRequest() => new()
	{
		AssignedAt = AssignedAt
	};

	protected override bool Validate()
	{
		if (!int.TryParse(ProjectId, out var project) || project <= 0
			|| !int.TryParse(UserId, out var user) || user <= 0)
			ErrorMessage = "Enter valid project and user IDs.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}
}

public sealed class MaterialRequestsViewModel(MaterialRequestsApiService api)
	: CrudListViewModel<MaterialRequestDto, CreateMaterialRequestRequest, UpdateMaterialRequestRequest>(
		api, nameof(MaterialRequestFormPage), item => item.MaterialRequestId.ToString(CultureInfo.InvariantCulture));

public partial class MaterialRequestFormViewModel(MaterialRequestsApiService api)
	: CrudFormViewModel<MaterialRequestDto, CreateMaterialRequestRequest, UpdateMaterialRequestRequest>(api)
{
	[ObservableProperty] private string projectId = string.Empty;
	[ObservableProperty] private string requestedByUserId = string.Empty;
	[ObservableProperty] private string materialId = string.Empty;
	[ObservableProperty] private string quantityRequested = string.Empty;
	[ObservableProperty] private string unit = string.Empty;
	[ObservableProperty] private string priority = "Normal";
	[ObservableProperty] private string status = "Pending";
	[ObservableProperty] private string notes = string.Empty;
	[ObservableProperty] private DateTime requestedAt = DateTime.Now;
	[ObservableProperty] private string reviewedByUserId = string.Empty;
	[ObservableProperty] private DateTime reviewedAt = DateTime.Now;
	[ObservableProperty] private bool isReviewed;

	public IReadOnlyList<string> PriorityOptions { get; } = ["Low", "Normal", "High", "Urgent"];
	public IReadOnlyList<string> StatusOptions { get; } = ["Pending", "Approved", "Rejected", "Fulfilled"];

	protected override void Apply(MaterialRequestDto item)
	{
		ProjectId = item.ProjectId.ToString(CultureInfo.InvariantCulture);
		RequestedByUserId = item.RequestedByUserId.ToString(CultureInfo.InvariantCulture);
		MaterialId = item.MaterialId.ToString(CultureInfo.InvariantCulture);
		QuantityRequested = item.QuantityRequested.ToString(CultureInfo.InvariantCulture);
		Unit = item.Unit;
		Priority = item.Priority;
		Status = item.Status;
		Notes = item.Notes ?? string.Empty;
		RequestedAt = item.RequestedAt;
		ReviewedByUserId = item.ReviewedByUserId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
		IsReviewed = item.ReviewedByUserId.HasValue;
		ReviewedAt = item.ReviewedAt ?? DateTime.Now;
	}

	protected override CreateMaterialRequestRequest CreateCreateRequest()
	{
		var reviewed = IsReviewed ? int.Parse(ReviewedByUserId, CultureInfo.InvariantCulture) : (int?)null;
		return new()
		{
			ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
			RequestedByUserId = int.Parse(RequestedByUserId, CultureInfo.InvariantCulture),
			MaterialId = int.Parse(MaterialId, CultureInfo.InvariantCulture),
			QuantityRequested = int.Parse(QuantityRequested, CultureInfo.InvariantCulture),
			Unit = Unit.Trim(),
			Priority = Priority.Trim(),
			Status = Status.Trim(),
			Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
			RequestedAt = RequestedAt,
			ReviewedByUserId = reviewed,
			ReviewedAt = reviewed.HasValue ? ReviewedAt : null
		};
	}

	protected override UpdateMaterialRequestRequest CreateUpdateRequest()
	{
		var reviewed = IsReviewed ? int.Parse(ReviewedByUserId, CultureInfo.InvariantCulture) : (int?)null;
		return new()
		{
			ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
			RequestedByUserId = int.Parse(RequestedByUserId, CultureInfo.InvariantCulture),
			MaterialId = int.Parse(MaterialId, CultureInfo.InvariantCulture),
			QuantityRequested = int.Parse(QuantityRequested, CultureInfo.InvariantCulture),
			Unit = Unit.Trim(),
			Priority = Priority.Trim(),
			Status = Status.Trim(),
			Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
			RequestedAt = RequestedAt,
			ReviewedByUserId = reviewed,
			ReviewedAt = reviewed.HasValue ? ReviewedAt : null
		};
	}

	protected override bool Validate()
	{
		if (!PositiveInt(ProjectId) || !PositiveInt(RequestedByUserId)
			|| !PositiveInt(MaterialId) || !PositiveInt(QuantityRequested))
			ErrorMessage = "Enter positive project, user, material, and quantity values.";
		else if (string.IsNullOrWhiteSpace(Unit) || string.IsNullOrWhiteSpace(Priority)
			|| string.IsNullOrWhiteSpace(Status))
			ErrorMessage = "Unit, priority, and status are required.";
		else if (IsReviewed && !PositiveInt(ReviewedByUserId))
			ErrorMessage = "Enter the reviewing user's ID.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}

	private static bool PositiveInt(string value) => int.TryParse(value, out var number) && number > 0;
}

public sealed class MaterialUsagesViewModel(MaterialUsagesApiService api)
	: CrudListViewModel<MaterialUsageDto, CreateMaterialUsageRequest, UpdateMaterialUsageRequest>(
		api, nameof(MaterialUsageFormPage), item => item.MaterialUsageId.ToString(CultureInfo.InvariantCulture));

public partial class MaterialUsageFormViewModel(MaterialUsagesApiService api)
	: CrudFormViewModel<MaterialUsageDto, CreateMaterialUsageRequest, UpdateMaterialUsageRequest>(api)
{
	[ObservableProperty] private string projectId = string.Empty;
	[ObservableProperty] private string userId = string.Empty;
	[ObservableProperty] private string materialId = string.Empty;
	[ObservableProperty] private string quantityUsed = string.Empty;
	[ObservableProperty] private DateTime usedAt = DateTime.Now;
	[ObservableProperty] private TimeSpan usedAtTime = DateTime.Now.TimeOfDay;

	partial void OnUsedAtTimeChanged(TimeSpan value) => UsedAt = UsedAt.Date + value;

	protected override void Apply(MaterialUsageDto item)
	{
		ProjectId = item.ProjectId.ToString(CultureInfo.InvariantCulture);
		UserId = item.UserId.ToString(CultureInfo.InvariantCulture);
		MaterialId = item.MaterialId.ToString(CultureInfo.InvariantCulture);
		QuantityUsed = item.QuantityUsed.ToString(CultureInfo.InvariantCulture);
		UsedAt = item.UsedAt;
		UsedAtTime = item.UsedAt.TimeOfDay;
	}

	protected override CreateMaterialUsageRequest CreateCreateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		UserId = int.Parse(UserId, CultureInfo.InvariantCulture),
		MaterialId = int.Parse(MaterialId, CultureInfo.InvariantCulture),
		QuantityUsed = int.Parse(QuantityUsed, CultureInfo.InvariantCulture),
		UsedAt = UsedAt
	};

	protected override UpdateMaterialUsageRequest CreateUpdateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		UserId = int.Parse(UserId, CultureInfo.InvariantCulture),
		MaterialId = int.Parse(MaterialId, CultureInfo.InvariantCulture),
		QuantityUsed = int.Parse(QuantityUsed, CultureInfo.InvariantCulture),
		UsedAt = UsedAt
	};

	protected override bool Validate()
	{
		if (!PositiveInt(ProjectId) || !PositiveInt(UserId)
			|| !PositiveInt(MaterialId) || !PositiveInt(QuantityUsed))
			ErrorMessage = "Enter positive project, user, material, and quantity values.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}

	private static bool PositiveInt(string value) => int.TryParse(value, out var number) && number > 0;
}

public sealed class ReceiptInvoicesViewModel(ReceiptInvoicesApiService api)
	: CrudListViewModel<ReceiptInvoiceDto, CreateReceiptInvoiceRequest, UpdateReceiptInvoiceRequest>(
		api, nameof(ReceiptInvoiceFormPage), item => item.ReceiptInvoiceId.ToString(CultureInfo.InvariantCulture));

public partial class ReceiptInvoiceFormViewModel(
	ReceiptInvoicesApiService api,
	CameraCaptureService cameraCapture)
	: CrudFormViewModel<ReceiptInvoiceDto, CreateReceiptInvoiceRequest, UpdateReceiptInvoiceRequest>(api)
{
	[ObservableProperty] private string projectId = string.Empty;
	[ObservableProperty] private string userId = string.Empty;
	[ObservableProperty] private string vendor = string.Empty;
	[ObservableProperty] private string amount = string.Empty;
	[ObservableProperty] private DateTime purchaseDate = DateTime.Today;
	[ObservableProperty] private string invoiceFileName = "No invoice image selected";
	private byte[] invoiceImage = [];

	protected override void Apply(ReceiptInvoiceDto item)
	{
		ProjectId = item.ProjectId.ToString(CultureInfo.InvariantCulture);
		UserId = item.UserId.ToString(CultureInfo.InvariantCulture);
		Vendor = item.Vendor;
		Amount = item.Amount.ToString(CultureInfo.CurrentCulture);
		PurchaseDate = item.PurchaseDate;
		invoiceImage = item.InvoiceImage;
		InvoiceFileName = invoiceImage.Length == 0 ? "No invoice image selected" : "Existing image saved";
	}

	protected override CreateReceiptInvoiceRequest CreateCreateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		UserId = int.Parse(UserId, CultureInfo.InvariantCulture),
		Vendor = Vendor.Trim(),
		Amount = decimal.Parse(Amount, CultureInfo.CurrentCulture),
		PurchaseDate = PurchaseDate,
		InvoiceImage = invoiceImage
	};

	protected override UpdateReceiptInvoiceRequest CreateUpdateRequest() => new()
	{
		ProjectId = int.Parse(ProjectId, CultureInfo.InvariantCulture),
		UserId = int.Parse(UserId, CultureInfo.InvariantCulture),
		Vendor = Vendor.Trim(),
		Amount = decimal.Parse(Amount, CultureInfo.CurrentCulture),
		PurchaseDate = PurchaseDate,
		InvoiceImage = invoiceImage
	};

	protected override bool Validate()
	{
		if (!int.TryParse(ProjectId, out var project) || project <= 0
			|| !int.TryParse(UserId, out var user) || user <= 0)
			ErrorMessage = "Enter valid project and user IDs.";
		else if (string.IsNullOrWhiteSpace(Vendor) || Vendor.Trim().Length > 200)
			ErrorMessage = "Enter a vendor name of 1 to 200 characters.";
		else if (!decimal.TryParse(Amount, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsedAmount)
			|| parsedAmount <= 0)
			ErrorMessage = "Enter an amount greater than zero.";
		else
		{
			ErrorMessage = string.Empty;
			return true;
		}

		return false;
	}

	[RelayCommand]
	private async Task SelectInvoiceImageAsync()
	{
		try
		{
			var file = await FilePicker.Default.PickAsync(new PickOptions
			{
				PickerTitle = "Choose an invoice image",
				FileTypes = FilePickerFileType.Images
			});
			if (file is null)
				return;

			await using var stream = await file.OpenReadAsync();
			using var memory = new MemoryStream();
			await stream.CopyToAsync(memory);
			invoiceImage = memory.ToArray();
			InvoiceFileName = file.FileName;
			ErrorMessage = string.Empty;
		}
		catch (Exception exception) when (exception is not OperationCanceledException)
		{
			ErrorMessage = "The invoice image could not be selected.";
		}
	}

	[RelayCommand]
	private async Task CaptureInvoiceImageAsync()
	{
		try
		{
			var capture = await cameraCapture.CaptureImageAsync("Capture an invoice");
			if (capture is null)
				return;

			invoiceImage = capture.Value.Image;
			InvoiceFileName = capture.Value.FileName;
			ErrorMessage = string.Empty;
		}
		catch (Exception exception) when (exception is not OperationCanceledException)
		{
			ErrorMessage = exception is NotSupportedException
				? "Camera capture is not supported on this device."
				: "The invoice image could not be captured.";
		}
	}
}

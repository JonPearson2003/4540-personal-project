#if ANDROID
using Android.App;
using Android.Content;
using Android.Provider;
#endif

namespace BuildTrack.App.Services;

public sealed class CameraCaptureService
{
#if ANDROID
	private const int CaptureRequestCode = 941;
	private static TaskCompletionSource<bool>? pendingCapture;
#endif

	public async Task<(byte[] Image, string FileName)?> CaptureImageAsync(string title)
	{
#if ANDROID
		return await CaptureAndroidImageAsync();
#else
		if (!MediaPicker.Default.IsCaptureSupported)
			throw new NotSupportedException("Camera capture is not supported on this device.");

		var photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
		{
			Title = title
		});
		if (photo is null)
			return null;

		await using var stream = await photo.OpenReadAsync();
		using var memory = new MemoryStream();
		await stream.CopyToAsync(memory);
		return (memory.ToArray(), photo.FileName);
#endif
	}

#if ANDROID
	private static async Task<(byte[] Image, string FileName)?> CaptureAndroidImageAsync()
	{
		var permission = await Permissions.RequestAsync<Permissions.Camera>();
		if (permission != PermissionStatus.Granted)
			throw new UnauthorizedAccessException("Camera permission was denied.");

		var activity = Platform.CurrentActivity
			?? throw new InvalidOperationException("No Android activity is available for camera capture.");
		var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		if (Interlocked.CompareExchange(ref pendingCapture, completion, null) is not null)
			throw new InvalidOperationException("A camera capture is already in progress.");

		var fileName = $"buildtrack_{Guid.NewGuid():N}.jpg";
		var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
		try
		{
			var intent = new Intent(MediaStore.ActionImageCapture);
			if (intent.ResolveActivity(activity.PackageManager) is null)
				throw new NotSupportedException("No camera app is available on this device.");

			var outputFile = new Java.IO.File(filePath);
			var outputUri = AndroidX.Core.Content.FileProvider.GetUriForFile(
				activity,
				$"{activity.PackageName}.fileProvider",
				outputFile);
			intent.PutExtra(MediaStore.ExtraOutput, outputUri);
			intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);

			activity.StartActivityForResult(intent, CaptureRequestCode);
			if (!await completion.Task)
				return null;

			var image = await System.IO.File.ReadAllBytesAsync(filePath);
			return (image, fileName);
		}
		finally
		{
			Interlocked.CompareExchange(ref pendingCapture, null, completion);
			if (System.IO.File.Exists(filePath))
				System.IO.File.Delete(filePath);
		}
	}

	public static void CompleteAndroidCapture(int requestCode, bool succeeded)
	{
		if (requestCode == CaptureRequestCode)
			pendingCapture?.TrySetResult(succeeded);
	}
#endif
}

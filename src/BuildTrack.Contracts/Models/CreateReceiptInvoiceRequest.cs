using System.ComponentModel.DataAnnotations;

namespace BuildTrack.Contracts.Models;

/// <summary>Payload for recording a project purchase and invoice image.</summary>
public sealed record CreateReceiptInvoiceRequest
{
	[Range(1, int.MaxValue)]
	public int ProjectId { get; init; }

	[Range(1, int.MaxValue)]
	public int UserId { get; init; }

	[Required]
	[StringLength(200, MinimumLength = 1)]
	public string Vendor { get; init; } = string.Empty;

	[Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
	public decimal Amount { get; init; }

	public DateTime PurchaseDate { get; init; }

	public byte[] InvoiceImage { get; init; } = [];
}

namespace BuildTrack.Contracts.Models;

public sealed record ReceiptInvoiceDto(
	int ReceiptInvoiceId,
	int ProjectId,
	int UserId,
	string Vendor,
	decimal Amount,
	DateTime PurchaseDate,
	byte[] InvoiceImage
);

namespace BuildTrack.Contracts.Models;

public sealed record RecieptInvoiceDto(
	int ProjectId,
	int UserId,
	string Vendor,
	double Amount,
	DateTime PurchaseDate,
	byte[] InvoiceImage
);

namespace BuildTrack.Api.Entities;

public sealed class ReceiptInvoice
{
	public int ReceiptInvoiceId { get; set; }
	public int ProjectId { get; set; }
	public int UserId { get; set; }
	public required string Vendor { get; set; }
	public decimal Amount { get; set; }
	public DateTime PurchaseDate { get; set; }
	public byte[] InvoiceImage { get; set; } = Array.Empty<byte>();
}
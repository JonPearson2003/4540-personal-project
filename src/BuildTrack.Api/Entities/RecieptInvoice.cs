namespace BuildTrack.Api.Entities;

public sealed class RecieptInvoice
{
	public int ProjectId { get; set; }
	public int UserId { get; set; }
    public string Vendor { get; set; } = string.Empty;
	public double Amount { get; set; }
	public DateTime purchaseDate { get; set; }
	public byte[] InvoiceImage { get; set; } = Array.Empty<byte>();
}
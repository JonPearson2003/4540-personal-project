namespace BuildTrack.Api.Entities;

public sealed class TimeRecords
{
	public int TimeRecordId { get; set; }
	public int UserId { get; set; }
    public int ProjectId { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public TimeSpan Duration { get; set; }
}
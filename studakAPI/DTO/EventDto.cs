namespace studakAPI.DTO;

public class EventDto
{
    public int Id { get; set; }
    public string? Organization { get; set; }
    public string? Responsible { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Rate { get; set; }
}
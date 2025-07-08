
public class SegDocumentJunctionWithTriggerDTO
{
    public int? SegId { get; set; }  // Nullable for unmatched EventTriggers
    public int? Position { get; set; }
    public List<Observation>? Observations { get; set; } = new();
    public required EventTrigger EventTrigger { get; set; }
}


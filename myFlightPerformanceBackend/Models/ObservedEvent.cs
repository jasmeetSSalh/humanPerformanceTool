using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace myFlightPerformance.Models;

public class ObservedEvent
{
    [Key]
    public int EventId { get; set; }
    
    [ForeignKey("SegDocument")]
    public int SegId { get; set; }

    public int FlyingPilot { get; set; }
    public int NonFlyingPilot { get; set; }
    public DateTime EventCreationDate { get; set; }

    public required string FocusArea {get; set;}
}

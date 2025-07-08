using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace myFlightPerformance.Models;

public class ObservedGradesInstructorCritique
{
    [Key]
    public int Id {get; set;}

    [ForeignKey("SegDocuments")]
    public int SegId { get; set; }

    [ForeignKey("ObservedEvent")]
    public int EventId {get; set;}

    public required string Category {get; set;}
    public required int PilotEmpId {get; set;}
    public required int Score {get; set;}

}
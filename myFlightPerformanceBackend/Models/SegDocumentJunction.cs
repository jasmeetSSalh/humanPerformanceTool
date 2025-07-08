using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class SegDocumentJunction {
    [ForeignKey("SegDocument")]
    public int SegId {get; set;}

    [ForeignKey("EventTrigger")]
    public int TrigId {get; set;}

    public int Position {get; set;} // Form Position

   [Column(TypeName = "TEXT")]  // Store as JSON string
    public required List<int> ObsId { get; set; } = [];
}
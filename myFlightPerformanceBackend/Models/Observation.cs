using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Observation
{
    [Key]
    public int ObsId { get; set; }

    public required string Category { get; set; }
    public required string SubCategory { get; set; }
    public required string Behaviour { get; set; }
    

}

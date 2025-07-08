using System.ComponentModel.DataAnnotations;

public class EventTrigger
{
    [Key]
    public int TrigId {get; set;}
    public required string TrigCat {get; set;}
    public required string TrigDes {get; set;}
    public required TrigTypeEnum TrigType {get; set;}

    public enum TrigTypeEnum
    {
        Malfunction,
        Procedure,
        Other
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace myFlightPerformance.Models;


public class SegDocument
{
    [Key]
    public int SegId { get; set; }

    [Required]
    public required string SegName { get; set; }

    public DateTime CreationDate { get; set; }

    public required string CreatedBy { get; set; }

    public required string DocName { get; set; }

    public required string TrainingProgram {get; set;}

}

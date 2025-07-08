using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using myFlightPerformance.Models;

namespace myFlightPerformance.Models
{
    public class ObservedGrade
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ObservedEvent")]
        public int EventId { get; set; }

        [ForeignKey("Observation")]
        public int ObsId { get; set; }

        public int PilotEmpId { get; set; }

        public Grades Grade { get; set; }

        public enum Grades
        {
            NegativeOne = -1,
            Zero = 0,
            One = 1
        }
    }
}

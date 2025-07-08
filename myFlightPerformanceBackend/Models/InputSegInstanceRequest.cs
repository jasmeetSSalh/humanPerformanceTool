using myFlightPerformance.Models;

public class InputSegInstanceRequest
{
    public required ObservedEvent observedEventInput { get; set; }
    public required List<ObservedGrade> observeredGradesInput { get; set; } // Fixed name
    public required List<ObservedGradesInstructorCritique> observedGradesSummaryInput {get; set;}
}

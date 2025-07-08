using myFlightPerformance.Models;

namespace myFlightPerformance
{
    public interface IFormInputService
    {
        Task<SegDocument> GetSpecificSegDocument(int segId);
        
        // Task<SegDocumentJunction[]> GetSpecificDocumentJunction(int segId);

        Task<IEnumerable<SegDocumentJunctionWithTriggerDTO>> GetSpecificDocumentJunctionWithEventTrigger(int segId);

        Task<string> InputSegInstance(ObservedEvent observedEventRequest , List<ObservedGrade> ObservedGradesRequest, List<ObservedGradesInstructorCritique> ObservedGradesInstructorCritiqueRequest );    
    }
}
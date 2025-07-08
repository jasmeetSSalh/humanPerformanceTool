using myFlightPerformance.Controllers;
using myFlightPerformance.Models;
using myFlightPerformance.Services;

namespace myFlightPerformance
{
    public interface ISegInputService
    {
        Task<List<EventTrigger>> GetMalfunction();
        Task<List<EventTrigger>> GetProcedures();
        Task<List<EventTrigger>> GetOthers();
        Task<List<Observation>> GetObservations();

        //Creating a new SEG
       Task<int> CreateSegDocumentAsync(SegDocument segDocumentInput, List<SegDocumentJunction> segDocumentJunctionsInput);
    }
}
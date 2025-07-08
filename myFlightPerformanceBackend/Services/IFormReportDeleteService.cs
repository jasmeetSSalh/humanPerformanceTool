using myFlightPerformance.Models;

namespace myFlightPerformance
{
    public interface IFormReportDeleteService
    {
        Task<List<ObservedEvent>> GetAllFormsAsync();
        Task<ObservedEvent> DeleteInputFormAsync(int eventId);
    }
}
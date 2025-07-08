using myFlightPerformance.Models;
using Microsoft.EntityFrameworkCore;

namespace myFlightPerformance.Services
{
    public class FormReportDeleteService : IFormReportDeleteService
    {
        private readonly ApplicationDbContext _context;

        public FormReportDeleteService(ApplicationDbContext context)
        {
            _context = context;
        }


        // Get Request Service
       public async Task<List<ObservedEvent>> GetAllFormsAsync()
        {
            try
            {
                return await _context.ObservedEvents
                    .AsNoTracking() // to optimize read performance
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving observed events: {ex.Message}");
                return new List<ObservedEvent>(); // Return an empty list to avoid crashes
            }
        }

        public async Task<ObservedEvent> DeleteInputFormAsync(int eventId)
        {
            try
            {
                var deletedEvent = _context.ObservedEvents.FirstOrDefault(p => p.EventId == eventId);

                if (deletedEvent != null)
                {
                    _context.ObservedEvents.Remove(deletedEvent);
                    await _context.SaveChangesAsync();
                }

                return deletedEvent;
            } 
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting observed event: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }

    }
}
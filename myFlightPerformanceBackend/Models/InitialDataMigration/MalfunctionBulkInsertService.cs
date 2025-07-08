using myFlightPerformance.Models;

namespace myFlightPerformance.Services
{
    public class MalfunctionBulkInsertService
    {
        private readonly ApplicationDbContext _context;

        public MalfunctionBulkInsertService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void BulkInsertMalfunctions(List<EventTrigger> malfunctions)
        {
            if (malfunctions == null || !malfunctions.Any())
            {
                throw new ArgumentNullException("This list of malfunctions is empty.");
            }
            try 
            {
                // Use AddRange for bulk insert
               _context.EventTriggers.AddRange(malfunctions);
                _context.SaveChanges();
                Console.WriteLine("Bulk Insert Completed Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during bulk insert: {ex.Message}");
            }
        }
    }
}
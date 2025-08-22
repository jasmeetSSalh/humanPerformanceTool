using Microsoft.EntityFrameworkCore;
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

         public async Task<int> InsertMalfunction(EventTrigger malfunction)
        {
             if (malfunction == null)
            {
                throw new ArgumentNullException("This list of malfunctions is empty.");
            }


            try 
            {
                // Use AddRange for bulk insert
                await _context.EventTriggers.AddAsync(malfunction);

                // get the id for the inserted record

                await _context.SaveChangesAsync();

                var id = await _context.EventTriggers
                    .Where(e =>  e.TrigDes == malfunction.TrigDes)
                    .FirstOrDefaultAsync();

                Console.WriteLine("Bulk Insert Completed Successfully!");
                return id.TrigId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during bulk insert: {ex.Message}");
                throw new Exception("An error occurred while inserting the malfunction event trigger.", ex);
            }
        }
    }
}
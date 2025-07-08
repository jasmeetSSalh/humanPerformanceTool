using myFlightPerformance.Models;

namespace myFlightPerformance.Services
{
    public class ObservationBulkInsertService
    {
        private readonly ApplicationDbContext _context;

        public ObservationBulkInsertService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to perform bulk insert using AddRange
        public void BulkInsertObservations(List<Observation> observations)
        {
            if (observations == null || !observations.Any())
            {
                throw new ArgumentException("The list of observations is empty.");
            }

            try
            {
                // Use AddRange for bulk insert
                _context.Observations.AddRange(observations);
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

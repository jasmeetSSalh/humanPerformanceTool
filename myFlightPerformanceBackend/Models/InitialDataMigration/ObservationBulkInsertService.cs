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

        public int InsertObservation(Observation newObservation)
        {
            if (newObservation == null)
            {
                throw new ArgumentNullException("The observation is null.");
            }
            try 
            {
                // Use AddRange for bulk insert
                _context.Observations.Add(newObservation);
                // get the id for the inserted record
               

                _context.SaveChanges();
                var id = _context.Observations
                    .Where(e => e.Behaviour == newObservation.Behaviour && e.Behaviour == newObservation.Behaviour)
                    .Select(e => e.ObsId)
                    .FirstOrDefault();
                    
                Console.WriteLine("Bulk Insert Completed Successfully!");
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during bulk insert: {ex.Message}");
                throw new Exception("An error occurred while inserting the malfunction event trigger.", ex);
            }
        }


    }
}

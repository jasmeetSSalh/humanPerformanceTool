using myFlightPerformance.Models;

namespace myFlightPerformance.Services
{
    public class OtherBulkInsertService
    {
        private ApplicationDbContext _context;

        public OtherBulkInsertService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int InsertOther(EventTrigger other)
        {
             if (other == null)
            {
                throw new ArgumentNullException("This list of others is empty.");
            }
            try 
            {
                // Use AddRange for bulk insert
                _context.EventTriggers.Add(other);
                // get the id for the inserted record
               

                _context.SaveChanges();
                 var id = _context.EventTriggers
                    .Where(e => e.TrigCat == "Other" && e.TrigDes == other.TrigDes)
                    .FirstOrDefault().TrigId;
                Console.WriteLine("Bulk Insert Completed Successfully!");
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during bulk insert: {ex.Message}");
                throw new Exception("An error occurred while inserting the other event trigger.", ex);
            }
        }
    }
}
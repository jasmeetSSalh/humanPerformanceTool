using myFlightPerformance.Models;

namespace myFlightPerformance.Services
{
    public class ProcedureBulkInserService
    {
        private readonly ApplicationDbContext _context;

        public ProcedureBulkInserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int InsertProcedure(EventTrigger procedure)
        {
            if (procedure == null )
            {
                throw new ArgumentNullException("This list of procedures is empty.");
            }
            try 
            {
                // Use AddRange for bulk insert
                _context.EventTriggers.AddRange(procedure);
                // get the id for the inserted record
               


                _context.SaveChanges();
                
                 var id = _context.EventTriggers
                    .Where(e => e.TrigCat == "Procedure" && e.TrigDes == procedure.TrigDes)
                    .FirstOrDefault().TrigId;
                //Console.WriteLine("Here's the ID of the inserted procedure: " + id);

                return id;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during bulk insert: {ex.Message}");
                throw new Exception("An error occurred while inserting the procedure event trigger.", ex);
            }
        }
    }
}
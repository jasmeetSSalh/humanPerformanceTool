using Microsoft.AspNetCore.Mvc;
using myFlightPerformance.Services;
using myFlightPerformance.Models;

namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]
    public class ProcedureController : Controller
    {
        private readonly ProcedureBulkInserService _bulkInsertService;
        public ProcedureController(ProcedureBulkInserService bulkInsertService)
        {
            _bulkInsertService = bulkInsertService;
        }

        
        [HttpPost("api/procedures/insert")]
        public IActionResult Insert([FromQuery] string procedureDescription)
        {
            try
            {
                var newEventTrigger = new EventTrigger 
                {
                    TrigCat = "Procedure", 
                    TrigDes = procedureDescription, 
                    TrigType = EventTrigger.TrigTypeEnum.Procedure
                };

                var id = _bulkInsertService.InsertProcedure(newEventTrigger);
                Console.WriteLine("Here's the ID of the inserted procedure: " + id);
                
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred during bulk insert: {ex.Message}");
            }
        }

}
}

using Microsoft.AspNetCore.Mvc;
using myFlightPerformance.Services;
using myFlightPerformance.Models;

namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]
    public class OtherController : Controller
    {
        private readonly OtherBulkInsertService _bulkInsertService;
        public OtherController(OtherBulkInsertService bulkInsertService)
        {
            _bulkInsertService = bulkInsertService;
        }

        
        [HttpPost("api/others/insert")]
        public IActionResult Insert([FromQuery] string otherDescription)
        {
            try
            {
                // Define the malfunctions to be inserted
                var newEventTrigger = new EventTrigger 
                                        {TrigCat = "Other", 
                                        TrigDes = otherDescription, 
                                        TrigType = EventTrigger.TrigTypeEnum.Other};
       

                var id = _bulkInsertService.InsertOther(newEventTrigger);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred during bulk insert: {ex.Message}");
            }
        }
    }

}

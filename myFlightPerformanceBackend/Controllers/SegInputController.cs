using Microsoft.AspNetCore.Mvc;
using myFlightPerformance.Models;


namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]

    public class SegInputController : Controller
    {
        private readonly ISegInputService _segInputService;

        public SegInputController(ISegInputService segInputService)
        {
            _segInputService = segInputService;
        }
        
        [HttpGet("api/get-malfunctions")]
        public async Task<ActionResult> GetMalfunctions()
        {
            var malfunctions = await _segInputService.GetMalfunction();
            if (malfunctions == null) {
                Console.WriteLine("Procedures's Array is NULL");
            } else {
                Console.WriteLine("Here's the Data", malfunctions);
            }
            return Ok(malfunctions);
        }

        [HttpGet("api/get-procedures")]
        public async Task<ActionResult> GetProcedures()
        {
            var procedures = await _segInputService.GetProcedures();
            if (procedures == null) {
                Console.WriteLine("Procedures's Array is NULL");
            } else {
                Console.WriteLine("Here's the Data", procedures);
            }
            return Ok(procedures);
        }

        [HttpGet("api/get-others")]
        public async Task<ActionResult> GetOthers()
        {
            var others = await _segInputService.GetOthers();
            if (others == null) {
                Console.WriteLine("Others's Array is NULL");
            } else {
                Console.WriteLine("Here's the Data", others);
            }
            return Ok(others);
        }

        [HttpGet("api/get-observations")]
        public async Task<ActionResult> GetObservations()
        {
            var observations = await _segInputService.GetObservations();
            
            return Ok(observations);
        }




       [HttpPost("api/create-seg")]
        public async Task<IActionResult> CreateSegDocument([FromBody] CreateSegDocumentRequest request)
        {
            var segDocumentInput = request.SegDocumentInput;
            var segDocumentJunctionsInput = request.SegDocumentJunctionsInput;

            try
            {
                var segDocument = await _segInputService.CreateSegDocumentAsync(segDocumentInput, segDocumentJunctionsInput);
                Console.WriteLine("tryed inputting a document this is what happened", segDocument);
                return Ok(segDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        



    }

}
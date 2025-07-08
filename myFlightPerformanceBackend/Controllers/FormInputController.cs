using Microsoft.AspNetCore.Mvc;
using myFlightPerformance.Services;
        using System.Text.Json; // Import for JSON serialization


namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]
    public class FormInputController : Controller
    {

        private readonly IFormInputService _formInputService;

        public FormInputController(IFormInputService formInputService)
        {
            _formInputService = formInputService;
        }
        

        // to recieve a seg_id as a JSON object
        public class segIdRequest
        {
            public int SegId {get;set;}
        }

       [HttpGet("api/retrieve-seg")]
        public IActionResult RetrieveSegDocument([FromQuery] int SegId)
        {
            try
            {
                
                var segDocument = _formInputService.GetSpecificSegDocument(SegId);


                // Proper logging
                Console.WriteLine($"Current Edit Seg: {SegId}");
                Console.WriteLine($"Current Seg Retreved: {segDocument}");

                return Ok(segDocument);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving segment document: {ex.Message}");

                return StatusCode(500, new 
                { 
                    message = "Internal server error", 
                    error = ex.Message 
                });
            }
        }

        [HttpGet("api/retrieve-seg-junctions")]
        public IActionResult RetrieveSegDocumentJunctions([FromQuery] int SegId)
        {
            try
            {
                
                var segJunctions = _formInputService.GetSpecificDocumentJunctionWithEventTrigger(SegId);
                

                // Proper logging
                Console.WriteLine($"Current Edit Seg: {SegId}");
                Console.WriteLine($"Current Seg Retreved: {segJunctions}");


                return Ok(segJunctions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving segment document: {ex.Message}");

                return StatusCode(500, new 
                { 
                    message = "Internal server error", 
                    error = ex.Message 
                });
            }
        }



        [HttpPost("api/input-seg-form")]
        public async Task<IActionResult> InputSegInstance([FromBody] InputSegInstanceRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body cannot be null" });
            }

            var observedEventRequest = request.observedEventInput;
            var observedGradesRequest = request.observeredGradesInput;
            var observedGradesInstructorCritiqueRequest = request.observedGradesSummaryInput;

            // Convert objects to JSON format for better logging
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

            Console.WriteLine($"Received Observed Event: {JsonSerializer.Serialize(observedEventRequest, jsonOptions)}");
            Console.WriteLine($"Received Observed Grades: {JsonSerializer.Serialize(observedGradesRequest, jsonOptions)}");
            Console.WriteLine($"Received Observed Grades Instructor Critique: {JsonSerializer.Serialize(observedGradesInstructorCritiqueRequest, jsonOptions)}");

            try
            {
                var inputInstance = await _formInputService.InputSegInstance(
                    observedEventRequest, 
                    observedGradesRequest, 
                    observedGradesInstructorCritiqueRequest
                );

                return Ok(new { message = "Input processed successfully", data = request });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing input segment: {ex.Message}\n{ex.StackTrace}");

                return StatusCode(500, new
                {
                    message = "Internal Server Error. Please try again later.",
                    error = ex.Message
                });
            }
        }



    }


}
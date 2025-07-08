using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;
using myFlightPerformance.Models;
using myFlightPerformance.Services;


namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]

    public class SegEditDeleteController : Controller
    {
        private readonly ISegEditDeleteService _segEditDeleteService;

        public SegEditDeleteController(ISegEditDeleteService segEditDeleteService)
        {
            _segEditDeleteService = segEditDeleteService;
        }
        

        // to recieve a seg_id as a JSON object
        public class segIdRequest
        {
            public int SegId {get;set;}
        }

       [HttpGet("api/retrieve-seg-and-junction")]
        public IActionResult RetrieveSegDocument([FromQuery] int SegId)
        {
            try
            {
                
                var segDocumentWithSegJunctions = _segEditDeleteService.RetrieveEditSeg(SegId);

                // Proper logging
                Console.WriteLine($"Current Edit Seg: {SegId}");

                return Ok(segDocumentWithSegJunctions);
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

        [HttpGet("api/get-all-seg-documents")]
        public async Task<IActionResult> GetAllSegDocuments()
        {
            var segDocuments = await _segEditDeleteService.GetAllSegDocuments();
            if(segDocuments == null){
                Console.WriteLine("No SEGs Found");
                return Ok(null);
            } else {
                Console.WriteLine("Here's All the SEGs: ", segDocuments);
                return Ok(segDocuments);
            }
        }

       [HttpDelete("api/delete-seg-document")]
        public async Task<IActionResult> DeleteSegDocument([FromQuery] int SegId)  // Use [FromQuery] to bind to query string
        {
            try
            {
                var segDocument = await _segEditDeleteService.DeleteSegDocumentAsync(SegId);
                Console.WriteLine("Deleted the following Seg ID: ", segDocument);
                return Ok(segDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost("api/edit-seg-documents")]

        public async Task<IActionResult> EditSegDocument([FromBody] CreateSegDocumentRequest request)
        {
            
            var segDocumentInput = request.SegDocumentInput;
            var segDocumentJunctionsInput = request.SegDocumentJunctionsInput;

            try
            {
                var segDocument = await _segEditDeleteService.EditSeg(segDocumentInput, segDocumentJunctionsInput);
                Console.WriteLine("Editting the following document: ", segDocument);
                return Ok(segDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        


    }

}
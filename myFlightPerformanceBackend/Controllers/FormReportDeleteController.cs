using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using myFlightPerformance.Models;
using myFlightPerformance.Services;


namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]

    public class FormReportDeleteController : Controller
    {
        private readonly IFormReportDeleteService _formReportDeleteService;

        public FormReportDeleteController(IFormReportDeleteService formReportDeleteService)
        {
            _formReportDeleteService = formReportDeleteService;
        }

        [HttpGet("api/retrieve-all-inputs")]
        public async Task<ActionResult> RetrieveAllInputtedForms()
        {
            var forms = await _formReportDeleteService.GetAllFormsAsync();
            if (forms == null)
            {
                Console.WriteLine("Forms's Array is Null");
            } else  {
                Console.WriteLine("Here's the form data: ", forms);
            }
            return Ok(forms);
        }
        

        
       [HttpDelete("api/delete-input-form")]
        public async Task<IActionResult> DeleteInputForm([FromQuery] int eventId)  // Use [FromQuery] to bind to query string
        {
            try
            {
                var deletedForm = await _formReportDeleteService.DeleteInputFormAsync(eventId);
                Console.WriteLine("Deleted the following Seg ID: ", deletedForm);
                return Ok(deletedForm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }


}
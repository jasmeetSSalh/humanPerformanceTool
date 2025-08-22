using Microsoft.AspNetCore.Mvc;
using myFlightPerformance.Services;


namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]
    public class ObservationController : Controller
    {
        private readonly ObservationBulkInsertService _bulkInsertService;

        // Constructor to inject the ObservationBulkInsertService
        public ObservationController(ObservationBulkInsertService bulkInsertService)
        {
            _bulkInsertService = bulkInsertService;
        }

        // Action to trigger bulk insert
        [HttpPost("api/observations/bulkInsert")]
        public IActionResult BulkInsert()
        {
            try
            {
                // Sample observations to insert
                var observations = new List<Observation>
                {
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "DETERMINES TYPE of briefing needed"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "EVALUATES RISK using T.W.I.N."},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "DETERMINES DEFENCES required"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "CONSIDERS FUNDAMENTALS required"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "CONSIDERS CONVENTIONAL SAFETY implications"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "REVIEWS and incorporates OPEX"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "REVIEWS Event Prevention TOOLS"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "Plans for a FAIL SAFE approach"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "ENGAGES the TEAM"},
                    new Observation{Category = "Briefings", SubCategory = "Preparation", Behaviour = "DETERMINES CRITICAL STEPS"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "Holds INTERACTIVE brief with the team"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "Discusses ITEMS IDENTIFIED in the preparation"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "Discusses CRITICAL STEPS"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "Discusses MITIGATING ACTIONS"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "SUMMARIZES the task and critical steps"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "ANTICIPATES hazards"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "FORESEEs potential CONSEQUENCES"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "EVALUATES DEFENSES"},
                    new Observation{Category = "Briefings", SubCategory = "Delivery", Behaviour = "REVIEWS OPEX"},
                    new Observation{Category = "Briefings", SubCategory = "De-Briefing", Behaviour = "Identifies POSITIVE BEHAVIOURS"},
                    new Observation{Category = "Briefings", SubCategory = "De-Briefing", Behaviour = "Identifies LEARNING opportunities"},
                    new Observation{Category = "Briefings", SubCategory = "De-Briefing", Behaviour = "Identifies OBSTACLES"},
                    new Observation{Category = "Briefings", SubCategory = "Documentation", Behaviour = "COMPLETES DOCUMENTATION"},
                    new Observation{Category = "Procedure Use", SubCategory = "General", Behaviour = "Verifies CORRECT PROCEDURE"},
                    new Observation{Category = "Procedure Use", SubCategory = "General", Behaviour = "Verifies CORRECT SECTION"},
                    new Observation{Category = "Procedure Use", SubCategory = "General", Behaviour = "Verifies CURRENT REVISION"},
                    new Observation{Category = "Procedure Use", SubCategory = "General", Behaviour = "INFORMS SUPERVISION of completion"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "READS NOTE box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "READS CAUTION box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "READ WARNING box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "READS ALARA box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "PLACE-KEEPS NOTE box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "PLACE-KEEPS CAUTION box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "PLACE-KEEPS WARNING box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Informatin Box", Behaviour = "PLACE-KEEPS ALARA box"},
                    new Observation{Category = "Procedure Use", SubCategory = "Place-Keeping", Behaviour = "CIRCLES STEP to be completed"},
                    new Observation{Category = "Procedure Use", SubCategory = "Place-Keeping", Behaviour = "READS STEP to be completed"},
                    new Observation{Category = "Procedure Use", SubCategory = "Place-Keeping", Behaviour = "Performs CORRECT ACTION"},
                    new Observation{Category = "Procedure Use", SubCategory = "Place-Keeping", Behaviour = "Reviews action and SLASHES STEP completed"},
                    new Observation{Category = "Procedure Use", SubCategory = "Place-Keeping", Behaviour = "HIGHLIGHTS PATH taken in procedure"},
                    new Observation{Category = "Procedure Use", SubCategory = "Flowchart Notes", Behaviour = "READS FLOWCHART NOTE"},
                    new Observation{Category = "Procedure Use", SubCategory = "Procedure Hold", Behaviour = "DRAWS LINE to indicate last completed step"},
                    new Observation{Category = "Procedure Use", SubCategory = "Procedure Hold", Behaviour = "Records DATE & TIME"},
                    new Observation{Category = "Procedure Use", SubCategory = "Procedure Hold", Behaviour = "Records REASON why"},
                    new Observation{Category = "Procedure Use", SubCategory = "Procedure Hold", Behaviour = "INITIALS procedure hold"},
                    new Observation{Category = "Procedure Use", SubCategory = "Procedure Hold", Behaviour = "INFORMS SUPERVISION of hold"},
                    new Observation{Category = "Communications", SubCategory = "Verbal Communications", Behaviour = "OBTAINS the ATTENTION of the receiver"},
                    new Observation{Category = "Communications", SubCategory = "Verbal Communications", Behaviour = "ACKNOWLEDGES START of communication"},
                    new Observation{Category = "Communications", SubCategory = "Verbal Communications", Behaviour = "Message delivered is CLEAR and CONCISE"},
                    new Observation{Category = "Communications", SubCategory = "Verbal Communications", Behaviour = "Message has LESS THAN TWO INSTRUCTIONS"},
                    new Observation{Category = "Communications", SubCategory = "Verbal Communications", Behaviour = "Utilized the PHONETIC ALPHABET"},
                    new Observation{Category = "Communications", SubCategory = "Verbal Communications", Behaviour = "Message REPEATED BACK to sender"},
                    new Observation{Category = "Communications", SubCategory = "Verbal Communications", Behaviour = "Message is ACKNOWLEDGED as CORRECT or not correct"},
                    new Observation{Category = "Communications", SubCategory = "Written Communications", Behaviour = "The message has a CLEAR OBJECTIVE"},
                    new Observation{Category = "Communications", SubCategory = "Written Communications", Behaviour = "The message is LEGIBLE"},
                    new Observation{Category = "Communications", SubCategory = "Written Communications", Behaviour = "The message CLEARLY STATES requirements"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Identification", Behaviour = "IDENTIFIES they are UNSURE"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Identification", Behaviour = "Identifies DIFFERENT FROM START"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Identification", Behaviour = "Identifies DIFFERENT THAN EXPECTED"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Identification", Behaviour = "Identifies NOT CLEAR instruction"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Identification", Behaviour = "Identifies information NOT PROVIDED"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (worker)", Behaviour = "STOPS when safe to do so"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (worker)", Behaviour = "SAFE STATES the work"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (worker)", Behaviour = "STEPS AWAY from the worksite"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (worker)", Behaviour = "CONTACT SUPERVISION"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (supervisor)", Behaviour = "THANKS worker for stopping"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (supervisor)", Behaviour = "Determines SUPPORT required"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (supervisor)", Behaviour = "Determines RISK with restarting"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (supervisor)", Behaviour = "Determines LEVEL OF AUTHORITY required"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (supervisor)", Behaviour = "Reviews actions required and re-BRIEFS TEAM"},
                    new Observation{Category = "Stop When Unsure", SubCategory = "Actions (supervisor)", Behaviour = "Inputs DOCUMENTATION if required"},
                    new Observation{Category = "Self Check", SubCategory = "S.T.A.R.", Behaviour = "VISIBLY PAUSED before performing task"},
                    new Observation{Category = "Self Check", SubCategory = "S.T.A.R.", Behaviour = "REVIEWED the intended ACTION and EXPECTED RESPONSE"},
                    new Observation{Category = "Self Check", SubCategory = "S.T.A.R.", Behaviour = "Performs CORRECT ACTION"},
                    new Observation{Category = "Self Check", SubCategory = "S.T.A.R.", Behaviour = "VERIFIES correct RESULT is obtained"},
                    new Observation{Category = "Self Check", SubCategory = "Two Minute Drill", Behaviour = "STOPS before performing job"},
                    new Observation{Category = "Self Check", SubCategory = "Two Minute Drill", Behaviour = "ASSESSES RISK with the job"},
                    new Observation{Category = "Conservatism", SubCategory = "Conservative Decision Making", Behaviour = "Does NOT JUSTIFY unacceptable conditions"},
                    new Observation{Category = "Conservatism", SubCategory = "Conservative Decision Making", Behaviour = "SLOWS DOWN to allow time for understanding"},
                    new Observation{Category = "Conservatism", SubCategory = "Conservative Decision Making", Behaviour = "STOPS IF UNSURE about the situation"},
                    new Observation{Category = "Conservatism", SubCategory = "Conservative Decision Making", Behaviour = "ASKS QUESTIONS to understand the situation"},
                    new Observation{Category = "Conservatism", SubCategory = "Conservative Decision Making", Behaviour = "CHALLENGES assumptions"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Peer Check", Behaviour = "PERFORMER SELF-CHECKS the correct component or task"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Peer Check", Behaviour = "PEER SELF-CHECKS the correct component or task"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Peer Check", Behaviour = "Performer and Peer AGREE on action and component"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Peer Check", Behaviour = "PEER OBSERVES the action before and during completion"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Peer Check", Behaviour = "PEER INFORMS performer of correct or incorrect action"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Independent Verification", Behaviour = "PERFORMER UTILIZES S.T.A.R. technique"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Independent Verification", Behaviour = "PERFORMER PLACE-KEEPS guiding document"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Independent Verification", Behaviour = "PERFORMER INFORMS SUPERVISION"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Independent Verification", Behaviour = "VERIFIER UTILIZES S.T.A.R. technique"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Independent Verification", Behaviour = "VERIFIER CONFIRMS as found condition"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Independent Verification", Behaviour = "VERIFIER PLACE-KEEPS guiding document"},
                    new Observation{Category = "Second Party Verification", SubCategory = "Independent Verification", Behaviour = "VERIFIER INFORMS SUPERVISION"}

                };

                // Ensure observations list is not empty
                if (observations == null || observations.Count == 0)
                {
                    return BadRequest("No observations provided for bulk insert.");
                }

                // Call the bulk insert service to insert observations
                _bulkInsertService.BulkInsertObservations(observations);

                // Return success message after insert
                return Ok("Bulk insert completed successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors during bulk insert
                return StatusCode(500, $"Error occurred during bulk insert: {ex.Message}");
            }
        }



        [HttpPost("api/observation/insert")]
        public IActionResult Insert([FromQuery] string observationDescription, string observationCategory, string observationSubCategory)
        {
            try
            {
                // Define the malfunctions to be inserted
                var newObservation = new Observation 
                {
                    Behaviour = observationDescription,
                    Category = observationCategory,
                    SubCategory = observationSubCategory
                };

                var id = _bulkInsertService.InsertObservation(newObservation);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred during bulk insert: {ex.Message}");
            }
        }


    }
}

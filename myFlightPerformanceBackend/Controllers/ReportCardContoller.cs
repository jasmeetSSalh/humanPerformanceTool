using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFlightPerformance.Services;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks; // Import for JSON serialization
using Xceed.Words.NET;



namespace myFlightPerformance.Controllers
{

    [Route("/")]
    [ApiController]
    public class ReportCardController : Controller
    {

        private readonly IFormInputService _formInputService;
        private readonly IWebHostEnvironment _env;

         private readonly Models.ApplicationDbContext _context;

        public ReportCardController(IFormInputService formInputService, IWebHostEnvironment env, Models.ApplicationDbContext context)
        {
            _formInputService = formInputService;
            _env = env;
            _context = context;
        }
    

        [HttpPost("api/download-report-card")]
        public IActionResult GenerateMarkingSheet([FromQuery] int eventId, int segId, int pilotEmpId)
        {
            if (segId == 0 || pilotEmpId == 0)
                return BadRequest("Invalid segment ID or pilot employee ID");

            var fields = getPilotGradeFields(eventId, segId, pilotEmpId).GetAwaiter().GetResult();
            string templatePath = Path.Combine(_env.ContentRootPath, "Forms", "report_card_template.docx");
            string outputPath = Path.Combine(_env.ContentRootPath, "bin", "FilledMarkingSheet.docx");

            using (var document = DocX.Load(templatePath))
            {
                foreach (var kvp in fields)
                {
                    document.ReplaceText(kvp.Key, kvp.Value ?? "");
                }
                document.SaveAs(outputPath);
            }

            byte[] bytes = System.IO.File.ReadAllBytes(outputPath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "FilledMarkingSheet.docx");
        }



        // typically would in a seperate service file but this controller only has one end point

        // private async Task<Dictionary<string, string>> getPilotGradeFields(int eventId, int segId, int pilotEmpId)
        // {
        //     var fields = new Dictionary<string, string>();

        //     Console.WriteLine($"Event ID: {eventId}, Segment ID: {segId}, Pilot Employee ID: {pilotEmpId}");

        //     fields["[emp][/emp]"] = pilotEmpId.ToString();
        //     fields["[ed][/ed]"] = await _context.ObservedEvents
        //                                             .Where(e => e.EventId == eventId)
        //                                             .Select(e => e.EventCreationDate.ToString("MM/dd/yyyy"))
        //                                             .FirstOrDefaultAsync() ?? "N/A";

        //     fields["[ogradep][/ogradep]"] = "N/A";
        //     fields["[/focusarea]"] = await _context.ObservedEvents
        //                                             .Where(e => e.EventId == eventId)
        //                                             .Select(e => e.FocusArea)
        //                                             .FirstOrDefaultAsync() ?? "N/A";

        //     var listOfAllCandidateGrades = await _context.ObservedGradesInstructorCritiques
        //                                             .Where(e => e.EventId == eventId && e.SegId == segId && e.PilotEmpId == pilotEmpId)
        //                                             .ToListAsync();

        //     Console.WriteLine($"Found {listOfAllCandidateGrades.Count} candidate grades for event ID {eventId}, segment ID {segId}, pilot employee ID {pilotEmpId}");

        //     foreach (var currentCandidateGrade in listOfAllCandidateGrades)
        //     {
        //         string keyPrefix = currentCandidateGrade.Category switch
        //         {
        //             "Procedural Use & Adherence" => "pua",
        //             "Briefing" => "b",
        //             "Communication" => "c",
        //             "Stop When Unsure" => "swu",
        //             "Self Check" => "sc",
        //             "Conservatism" => "con",
        //             "Situational Awareness" => "sa",
        //             "Verification Practices" => "vp",
        //             _ => null
        //         };

        //         if (keyPrefix == null) {
        //             // field should be filled out with N/A

        //             fields[$"[/{keyPrefix}s]"] = "N/A";
        //             fields[$"[/{keyPrefix}t]"] = "N/A";
        //             fields[$"[/{keyPrefix}c]"] = "N/A";
        //             fields[$"[/{keyPrefix}p]"] = "N/A";
        //         }

        //         Console.WriteLine($"Processing category: {currentCandidateGrade.Category}");
        //         Console.WriteLine($"Score: {currentCandidateGrade.Score}, Comment: {currentCandidateGrade.Comment}");

        //         // total points
        //         int total = 10;


        //         // Build field values in your [xxx][/xxx] format
        //         fields[$"[/{keyPrefix}s]"] = currentCandidateGrade.Score.ToString();
        //         fields[$"[/{keyPrefix}t]"] = total.ToString();
        //         fields[$"[/{keyPrefix}c]"] = currentCandidateGrade.Comment ?? "";
        //         fields[$"[/{keyPrefix}p]"] = (currentCandidateGrade.Score / (double)total * 100).ToString("F2") ;
        //     }

        //     // Overall Score
        //     fields["[/oss]"] = listOfAllCandidateGrades
        //         .Where(g => g.Category != "Overall Score")
        //         .Select(g => g.Score)
        //         .Sum().ToString();

        //     fields["[/ost]"] = ((listOfAllCandidateGrades.Count - 1) * 10).ToString();

        //     fields["[/osc]"] = string.Join(" ", listOfAllCandidateGrades
        //         .Where(g => g.Category != "Overall Score")
        //         .Select(g => g.Comment ?? ""));

        //     return fields;
        // }


        private async Task<Dictionary<string, string>> getPilotGradeFields(int eventId, int segId, int pilotEmpId)
        {
            var fields = new Dictionary<string, string>();

            var categories = new Dictionary<string, string>
            {
                { "Procedural Use & Adherence", "pua" },
                { "Briefing", "b" },
                { "Communication", "c" },
                { "Stop When Unsure", "swu" },
                { "Self Check", "sc" },
                { "Conservatism", "con" },
                { "Situational Awareness", "sa" },
                { "Verification Practices", "vp" }
            };

            fields["[emp][/emp]"] = pilotEmpId.ToString();
            fields["[ed][/ed]"] = await _context.ObservedEvents
                                                    .Where(e => e.EventId == eventId)
                                                    .Select(e => e.EventCreationDate.ToString("MM/dd/yyyy"))
                                                    .FirstOrDefaultAsync() ?? "N/A";

            fields["[/focusarea]"] = await _context.ObservedEvents
                                                    .Where(e => e.EventId == eventId)
                                                    .Select(e => e.FocusArea)
                                                    .FirstOrDefaultAsync() ?? "N/A";

            var listOfAllCandidateGrades = await _context.ObservedGradesInstructorCritiques
                                                        .Where(e => e.EventId == eventId && e.SegId == segId && e.PilotEmpId == pilotEmpId)
                                                        .ToListAsync();

            // Track which categories we have entries for
            var foundCategories = new HashSet<string>();

            foreach (var currentCandidateGrade in listOfAllCandidateGrades)
            {
                if (categories.TryGetValue(currentCandidateGrade.Category, out var keyPrefix))
                {
                    foundCategories.Add(currentCandidateGrade.Category);

                    int total = 10;
                    fields[$"[/{keyPrefix}s]"] = currentCandidateGrade.Score.ToString();
                    fields[$"[/{keyPrefix}t]"] = total.ToString();
                    fields[$"[/{keyPrefix}c]"] = currentCandidateGrade.Comment ?? "";
                }
            }

            // Fill in "N/A" for missing categories
            foreach (var kvp in categories)
            {
                var keyPrefix = kvp.Value;
                if (!foundCategories.Contains(kvp.Key))
                {
                    fields[$"[/{keyPrefix}s]"] = "N/A";
                    fields[$"[/{keyPrefix}t]"] = "N/A";
                    fields[$"[/{keyPrefix}c]"] = "N/A";
                    fields[$"[/{keyPrefix}p]"] = "N/A";
                }
            }

            // Overall Score (excluding "Overall Score" category if present)
            var gradesExcludingOverall = listOfAllCandidateGrades.Where(g => g.Category != "Overall Score").ToList();
            
            // average percentage rounded
            var overallScore = (int)gradesExcludingOverall.Sum(g => g.Score);
            Console.WriteLine($"Overall Score: {overallScore}");
            var overallTotalScore = (int)gradesExcludingOverall.Count * 10;
            Console.WriteLine($"Overall Count: {overallTotalScore}");
            var overallFocusAreaAverage = overallTotalScore > 0 ? Math.Round((double)overallScore/overallTotalScore * 100, 0).ToString() : "N/A";
            Console.WriteLine($"Overall Focus Area Average: {overallFocusAreaAverage}");
            fields["[/osfa]"] = overallFocusAreaAverage;

            // average focus area percentage rounded
            var observationalGrades = await _context.ObservedGrades
                        .Where(g => g.EventId == eventId  && g.PilotEmpId == pilotEmpId && g.Grade != 0)
                        .ToListAsync();

            var countOfObservation = observationalGrades.Count;
            var sumOfObservation = observationalGrades.Sum(g => (decimal)g.Grade);

            // average observational grade rounded
            fields["[/osp]"] = (countOfObservation > 0 ? Math.Round((double)(sumOfObservation / countOfObservation) * 100, 0).ToString() : "N/A");

            fields["[/osc]"] = string.Join(" ", gradesExcludingOverall.Select(g => g.Comment ?? ""));

            //average of the observation grade and average of focus area percentage averaged 
            double osfaValue;
            double.TryParse(fields["[/osfa]"], out osfaValue);
            fields["[ogradep][/ogradep] "] = countOfObservation > 0 ? 
                Math.Round(((double)(sumOfObservation / countOfObservation) * 100 + osfaValue) / 2, 0)
                .ToString() : "N/A";

            return fields;
        }

    }


}
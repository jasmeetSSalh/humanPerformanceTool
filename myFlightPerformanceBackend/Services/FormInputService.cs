using myFlightPerformance.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace myFlightPerformance.Services
{
    public class FormInputService : IFormInputService
    {
        private readonly ApplicationDbContext _context;


        // GET Request Services
        public FormInputService(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public  async Task<SegDocument> GetSpecificSegDocument(int segId)
        {
            var currentSeg = _context.SegDocuments.FirstOrDefault(p => p.SegId == segId);
            await _context.SaveChangesAsync();

            return currentSeg;
        }

       
        public async Task<IEnumerable<SegDocumentJunctionWithTriggerDTO>> GetSpecificDocumentJunctionWithEventTrigger(int segId)
        {
            // Fetch junctions and related event triggers
            var junctions = await (from eventTrigger in _context.EventTriggers
                                    join segJunction in _context.SegDocumentJunctions
                                    on eventTrigger.TrigId equals segJunction.TrigId into junctionGroup
                                    from junction in junctionGroup.DefaultIfEmpty() // Right Join behavior
                                    where junction.SegId == segId // Keep unmatched EventTriggers and filter SegId
                                    select new
                                    {
                                        SegId = junction != null ? junction.SegId : (int?)null,
                                        Position = junction.Position,
                                        EventTrigger = eventTrigger,
                                        ObsIdList = junction != null ? junction.ObsId : new List<int>()
                                    }).ToListAsync();

            // Initialize a list to hold the final result
            var resultList = new List<SegDocumentJunctionWithTriggerDTO>();

            // Iterate through each junction and map the ObsIdList to Observation entities
            foreach (var junction in junctions)
            {
                // Fetch the observations for the given ObsIdList
                var observations = await _context.Observations
                    .Where(p => junction.ObsIdList.Contains(p.ObsId))
                    .ToListAsync();
                // Create the DTO for the current junction
                var dto = new SegDocumentJunctionWithTriggerDTO
                {
                    SegId = junction.SegId ?? 0,  // Default to 0 or another appropriate value
                    Position = junction.Position,
                    EventTrigger = junction.EventTrigger,  // Map the EventTrigger directly
                    Observations = observations  // Map the list of observations
                };
                // Add the DTO to the result list
                resultList.Add(dto);
            }
            // Return the final list of DTOs
            return resultList;
        }


        //Post Request Services

        public Task<string> InputSegInstance(
            ObservedEvent observedEventRequest,
            List<ObservedGrade> ObservedGradesRequest,
            List<ObservedGradesInstructorCritique> ObservedGradesInstructorCritiqueRequest)
        {
            Console.WriteLine($"Here is the ObservedEvent: {observedEventRequest}");
            observedEventRequest.EventCreationDate = DateTime.Today ; // Set the date to UTC
            _context.ObservedEvents.Add(observedEventRequest);

            _context.SaveChanges();

            Console.WriteLine($"Here is the ObservedEvent: {observedEventRequest}");

            // Get the event id from the event just created
            var eventId = _context.ObservedEvents
                .Where(o => o.EventCreationDate == observedEventRequest.EventCreationDate)
                .Select(o => o.EventId)
                .FirstOrDefault();
            Console.WriteLine($"Here is the EventId: {eventId}");

            var segId = _context.ObservedEvents
                .Where(o => o.EventCreationDate == observedEventRequest.EventCreationDate)
                .Select(o => o.SegId)
                .FirstOrDefault();
            Console.WriteLine($"Here is the SegId: {segId}");


            /// EDIT GRADING AFTER FEEDBACK
            // Add the event id to the observed event request
            Console.WriteLine($"Here is the ObservedGrade: {ObservedGradesRequest}");
            foreach (var e in ObservedGradesRequest)
            {
                e.EventId = eventId; // Set the EventId for each observed grade
                _context.ObservedGrades.Add(e);
            }
            _context.SaveChanges();
            
            // Save the changes to the database
            
            Console.WriteLine($"Here is the Observed Grade Summary: {ObservedGradesInstructorCritiqueRequest}");
            foreach (var e in ObservedGradesInstructorCritiqueRequest)
            {
                e.EventId = eventId; // Set the EventId for each observed grade
                e.SegId = segId; // Set the SegId for each observed grade
                _context.ObservedGradesInstructorCritiques.Add(e);
            }
            _context.SaveChanges();

            return Task.FromResult("Submited the following data: " + observedEventRequest.ToString() + ObservedGradesRequest.ToString() + ObservedGradesInstructorCritiqueRequest.ToString());
        }

        
    }
}

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.VisualBasic;
using myFlightPerformance.Models;
using SQLitePCL;

namespace myFlightPerformance.Services
{
    public class SegEditDeleteService: ISegEditDeleteService
    {
        private readonly ApplicationDbContext _context;

        public SegEditDeleteService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Delete

        public async Task<SegDocument> DeleteSegDocumentAsync(int segId)
        {
            // SegDocument Table
            var segDocumentToDelete = _context.SegDocuments.FirstOrDefault(p => p.SegId == segId);
            
            if (segDocumentToDelete != null)
            {
                
                _context.SegDocuments.Remove(segDocumentToDelete);
                await _context.SaveChangesAsync();

                
            }

            //SegJunction Table

            var segDocumentJunctionsToDelete = _context.SegDocumentJunctions.Where(p => p.SegId == segId).ToList();

            if (segDocumentJunctionsToDelete.Any()) 
            {
                _context.SegDocumentJunctions.RemoveRange(segDocumentJunctionsToDelete);
                _context.SaveChanges();
            }

            return segDocumentToDelete;
        }


        public CreateSegDocumentRequest RetrieveEditSeg(int SegId)
        {
            // SegDocument
            var foundSegDocument = _context.SegDocuments.FirstOrDefault(p => p.SegId == SegId);

            // SegDocumentJunctions
            var foundSegDocumentJunctions = _context.SegDocumentJunctions.Where(p => p.SegId == SegId).ToList();

            if (foundSegDocument != null) {
                var returnSegDocumentRequest = new CreateSegDocumentRequest
                {
                    SegDocumentInput = foundSegDocument,
                    SegDocumentJunctionsInput = foundSegDocumentJunctions           
                };
                return returnSegDocumentRequest;
            }
            return null;


        }


        public async Task<SegDocument> EditSeg(SegDocument segDocumentInput, List<SegDocumentJunction> segDocumentJunctionsInput)
        {
            // SegDocument
            var foundSegDocument =  _context.SegDocuments.FirstOrDefault(p => p.SegId == segDocumentInput.SegId);
            if (foundSegDocument != null)
            {
                foundSegDocument.SegName = segDocumentInput.SegName;
                foundSegDocument.CreatedBy = segDocumentInput.CreatedBy;
                foundSegDocument.DocName = segDocumentInput.DocName;
                foundSegDocument.TrainingProgram = segDocumentInput.TrainingProgram;
                await _context.SaveChangesAsync();
            }

            //SegJunctions
            // Remove all SegJunctions
            // logic, if one junction entry gets deleted there's no way to track that change then an api call needs to be made everytime
            // a junction entry gets deleted, the current approach removed this by creating brand new junction entires with the exisiting SegId

            var segDocumentJunctionsToDelete = _context.SegDocumentJunctions.Where(p => p.SegId == segDocumentInput.SegId).ToList();

            if (segDocumentJunctionsToDelete.Any()) 
            {
                _context.SegDocumentJunctions.RemoveRange(segDocumentJunctionsToDelete);
                await _context.SaveChangesAsync();
            }

            // Add the "new" junctions
             if (segDocumentJunctionsInput != null && segDocumentJunctionsInput.Count > 0)
            {
                foreach (var junction in segDocumentJunctionsInput)
                {
                    junction.SegId = segDocumentInput.SegId; // Associate junction with document
                    _context.SegDocumentJunctions.Add(junction);
                }
                await _context.SaveChangesAsync();
            }

            return foundSegDocument;


        }

        public async Task<SegDocument[]> GetAllSegDocuments()
        {
            return await _context.SegDocuments.ToArrayAsync();

        }

        



    }
}
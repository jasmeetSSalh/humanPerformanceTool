using myFlightPerformance.Models;

namespace myFlightPerformance
{
    public interface ISegEditDeleteService
    {
        Task<SegDocument> DeleteSegDocumentAsync(int segId);
        CreateSegDocumentRequest RetrieveEditSeg(int segId);

        Task<SegDocument[]> GetAllSegDocuments();
        Task<SegDocument> EditSeg(SegDocument segDocumentInput, List<SegDocumentJunction> segDocumentJunctionsInput);
    }
}
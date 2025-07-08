using myFlightPerformance.Models;

public class CreateSegDocumentRequest
{
    public required SegDocument SegDocumentInput { get; set; }
    public required List<SegDocumentJunction> SegDocumentJunctionsInput { get; set; }
}

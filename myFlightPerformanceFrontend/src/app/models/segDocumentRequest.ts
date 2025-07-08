export interface SegDocumentResponse {
  segDocumentInput: SegDocumentJson;
  segDocumentJunctionsInput: SegDocumentJunctionJson[]; // Matches List in C#
}

interface SegDocumentJson {
  segId?: number;
  segName: string;
  creationDate: string;
  createdBy: string;
  docName: string;
  trainingProgram: string;
}

export interface SegDocumentJunctionJson {
  segId: number;  // Matches SegId in C#
  trigId: number; // Matches TrigId in C#
  position: number; // Matches Position in C#
  obsId: number[]; // Matches List<int> ObsId in C#
}

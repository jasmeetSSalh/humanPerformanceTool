import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SegDocument } from '../models/segDocument';
import { SegDocumentJunction } from '../models/segDocumentJunction';
import { SegDocumentResponse } from '../models/segDocumentRequest';

@Injectable({
    providedIn: 'root',
  })
  export class RecordsService {
    private baseUrl = 'http://localhost:5000/api';
  
    constructor(private http: HttpClient) {}

    getSpecificSegDocument(SegId: number): Observable<SegDocumentResponse>{
      return this.http.get<SegDocumentResponse>(`http://localhost:5000/api/retrieve-seg-and-junction?SegId=${SegId}`).pipe(
        catchError((error) => {
          console.error('Error fetching documents:', error);
          return throwError(() => new Error('Failed to fetch documents'));
        })
      );
    }
  
    getAllSegDocuments(): Observable<SegDocument[]> {
      return this.http.get<SegDocument[]>(`${this.baseUrl}/get-all-seg-documents`).pipe(
        catchError((error) => {
          console.error('Error fetching documents:', error);
          return throwError(() => new Error('Failed to fetch documents'));
        })
      );
    }

    
    updateSegDocument(requestPayload: { 
      segDocumentInput: SegDocument, 
      segDocumentJunctionsInput: SegDocumentJunction[] 
    }): Observable<any> {
      return this.http.post(`http://localhost:5000/api/edit-seg-documents`, requestPayload);
    }


    deleteSegDocument(SegId: number): Observable<any> {
      return this.http.delete(`${this.baseUrl}/delete-seg-document?SegId=${SegId}`).pipe(
          catchError((error) => {
              console.error('Error deleting document:', error);
              return throwError(() => new Error('Failed to delete document'));
          })
      );
    }


  
  
  }
  

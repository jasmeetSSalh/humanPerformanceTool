import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SegDocument } from '../models/segDocument';
import { SegDocumentJunction } from '../models/segDocumentJunction';
import { SegDocumentJunctionJson } from '../models/segDocumentRequest';
import { SegFormInputJunction } from '../models/segFormInputJunction';
import { ObservedEvent } from '../models/observedEvent';
import { ObservedGrade } from '../models/observedGrade';
import { ObservedGradeSummary } from '../models/observedGradeSumary';

@Injectable({
  providedIn: 'root',
})
export class InputService {
  private baseUrl = '/api';

  constructor(private http: HttpClient) {}

  // Get Request
  getSpecificSegDocument(segId: number): Observable<SegDocument> {
    return this.http.get<{ result: SegDocument }>(`${this.baseUrl}/retrieve-seg?SegId=${segId}`).pipe(
      map(response => response.result), // Extract `result` property
      catchError(error => {
        console.error('Error fetching document:', error);
        return throwError(() => new Error('Failed to fetch document'));
      })
    );
  }

  getSpecificSegDocumentJunction(segId: number): Observable<SegFormInputJunction[]> {
    return this.http.get<{ result: SegFormInputJunction[] }>(`${this.baseUrl}/retrieve-seg-junctions?SegId=${segId}`).pipe(
      map(response => response.result), // Extract `result` property
      catchError(error => {
        console.error('Error fetching junctions:', error);
        return throwError(() => new Error('Failed to fetch junctions'));
      })
    );
  }

  // Post Request

  inputForm( requestPayload: 
    { observedEventInput: ObservedEvent, 
    observeredGradesInput: ObservedGrade[] ,
    observedGradesSummaryInput: ObservedGradeSummary[]
  }): Observable<any> {
    // print requestedPayload
    console.log('Request Payload:', requestPayload);


    return this.http.post(`${this.baseUrl}/input-seg-form`, requestPayload);
  }

}

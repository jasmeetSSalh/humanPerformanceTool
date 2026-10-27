import { Injectable } from "@angular/core";
import { catchError, map, Observable, tap, throwError } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ObservedEvent } from "../models/observedEvent";

@Injectable ({
    providedIn: 'root'
})

export class InputRecordsService {
    private baseUrl = '/api'

    constructor(private http: HttpClient) {}

    // Get Request
    getAllFormInputs(): Observable<ObservedEvent[]> {
        return this.http
          .get<ObservedEvent[]>(`${this.baseUrl}/retrieve-all-inputs`) // Correct type
          .pipe(
            // tap(response => console.log("API Response:", response)), // Debugging log
            catchError(error => {
              console.error('Error fetching documents:', error);
              return throwError(() => new Error('Failed to fetch documents'));
            })
          );
    }
    
    


    // Post Request
    deleteFormInput(eventId: number): Observable<any> {
        return this.http.delete(`${this.baseUrl}/delete-input-form?eventId=${eventId}`).pipe(
            catchError(error => {
                console.error('Error deleting input:', error);
                return throwError(() => new Error('Failed to delete input'));
            })
        );
    }



    // Download Functions

    // In your service
    downloadReportCard(eventId: number, segId: number, pilotEmpId: number): Promise<void> {

        const url = `${this.baseUrl}/download-report-card?eventId=${eventId}&segId=${segId}&pilotEmpId=${pilotEmpId}`;

        return this.http.post(url, {}, {
            // response type blob needed for file download
            responseType: 'blob',
            observe: 'response'
        }).toPromise().then(response => {

                // Check if the response is valid
                if (!response || !response.body) {
                    throw new Error('No response or response body received');
                }

                // File name based off the candidate empId 
                let filename = `ReportCard${pilotEmpId}.rtf`;

                // Get the content disposition header
                const contentDisposition = response.headers.get('content-disposition');
                if (contentDisposition) {
                    const match = contentDisposition.match(/filename="?([^"]+)"?/);
                    if (match && match[1]) filename = match[1];
                }

                // Triggering a download logic -- boilerplate
                const blob = new Blob([response.body], { type: 'application/rtf' });
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = filename;
                document.body.appendChild(a);
                a.click();
                a.remove();
                window.URL.revokeObjectURL(url);

            });
    }





}
import { Injectable } from "@angular/core";
import { catchError, map, Observable, tap, throwError } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ObservedEvent } from "../models/observedEvent";

@Injectable ({
    providedIn: 'root'
})

export class InputRecordsService {
    private baseUrl = 'https://localhost:5001/api'

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





}
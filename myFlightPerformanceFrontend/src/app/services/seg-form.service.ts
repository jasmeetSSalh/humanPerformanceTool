import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observation } from '../models/observation';
import { Observable, catchError, of } from 'rxjs';
import { SegDocument } from '../models/segDocument';
import { EventTrigger } from '../models/eventTrigger';
import { SegDocumentJunction } from '../models/segDocumentJunction';

@Injectable({
  providedIn: 'root'
})
export class SegFormService {
  constructor(private http: HttpClient) { }

  private baseUrl = 'http://localhost:5000/api';


  // ------------------------------------------ GET REQUESTS -------------------------------------------

  getMalfunctions(): Observable<EventTrigger[]> {
    return this.http.get<EventTrigger[]>(`${this.baseUrl}/get-malfunctions`).pipe(
      catchError((error) => {
        console.error('Error fetching malfunctions:', error);
        throw error;
      })
    );
  }

  getProcedures(): Observable<EventTrigger[]> {
  return this.http.get<EventTrigger[]>(`${this.baseUrl}/get-procedures`).pipe(
    catchError((error) => {
        console.error('Error fetching procedures:', error);
        throw error;
      })
    );
  }

  getOthers(): Observable<EventTrigger[]> {
    return this.http.get<EventTrigger[]>(`${this.baseUrl}/get-others`).pipe(
      catchError((error) => {
        console.error('Error fetching others:', error);
        throw error;
      })
    );
  }

  getObservations(): Observable<Observation[]> {
    return this.http.get<Observation[]>(`${this.baseUrl}/get-observations`).pipe(
      catchError((error) => {
        console.error('Error fetching observations:', error);
        throw error;
      })
    );
  }


  // ------------------------------------------- POST REQUESTS ------------------------------------------

  // New method for submitting the form
  // submitSegDocument(requestPayload: { segDocumentInput: SegDocument, segDocumentJunctionsInput: SegDocumentJunction[] }): Observable<any> {
  //   return this.http.post(`${this.baseUrl}/create-seg`, requestPayload);
  // }

  submitSegDocument(requestPayload: { 
      segDocumentInput: SegDocument, 
      segDocumentJunctionsInput: SegDocumentJunction[] 
    }): Observable<any> {
      return this.http.post(`${this.baseUrl}/create-seg`, requestPayload);
    }

  // to insert new procedure
    insertProcedure(procedureDescription: string): Observable<any> {
      return this.http.post(
        `${this.baseUrl}/procedures/insert?procedureDescription=${procedureDescription}`, 
        {}
      ).pipe(
        catchError((error: any) => {
          console.error('Error inserting procedure:', error);
          return of(null); // Return null or an empty observable on error    
        })
      );
    }

    // to insert new procedure
  insertOther(otherDescription: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/others/insert?otherDescription=${otherDescription}`, {})
    .pipe(
      catchError((error) => {
        console.error('Error inserting procedure:', error)
        return of(null); // Return null or an empty observable on error    
      })
    );
  };

  // to insert new malfunction
  insertMalfunction(malfunctionDescription: string, malfunctionCategory: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/malfunctions/insert?malfunctionDescription=${malfunctionDescription}&malfunctionCategory=${malfunctionCategory}`, {})
    .pipe(
      catchError((error) => {
        console.error('Error inserting malfunction:', error)
        return of(null); // Return null or an empty observable on error    
      })
    );
  }

  // to insert a new behaviour

  //https://localhost:5001/api/observation/insert?observationDescription=Test12&observationCategory=Test&observationSubCategory=Test

  insertBehaviour(behaviourDescription: string, behaviourCategory: string, behaviourSubCategory: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/observation/insert?observationDescription=${behaviourDescription}&observationCategory=${behaviourCategory}&observationSubCategory=${behaviourSubCategory}`, {})
    .pipe(
      catchError((error) => {
        console.error('Error inserting behaviour:', error)
        return of(null); // Return null or an empty observable on error    
      })
    );
  }




}
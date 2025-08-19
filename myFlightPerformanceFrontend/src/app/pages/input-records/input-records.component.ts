import { Component, OnInit } from '@angular/core';
import { Table } from 'primeng/table';
import { SegDocument } from '../../models/segDocument';
import { RecordsService } from '../../services/seg-records.service';
import { TagModule } from 'primeng/tag';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router'; // Import Router
import { InputRecordsService } from '../../services/input-records.service';
import { ObservedEvent } from '../../models/observedEvent';

@Component({
  selector: 'app-input-records',
  standalone: true,
  imports: [TableModule, CommonModule, InputTextModule, TagModule,
    SelectModule, MultiSelectModule, ButtonModule, IconFieldModule, InputIconModule],
  templateUrl: './input-records.component.html',
  styleUrl: './input-records.component.css'
})
export class InputRecordsComponent implements OnInit{
    loading: boolean = true;
    observedEventsList: ObservedEvent[] = [];
  
  
    constructor(private inputRecordService: InputRecordsService, private router: Router) {}
  
    ngOnInit(): void
    {
      this.loadDocuments();
    }

    
  
    loadDocuments(): void {
      this.inputRecordService.getAllFormInputs().subscribe({
        next: (data) => {
          this.observedEventsList = this.mapToObservedEventDocument(data);
          console.log("Updated Observed List" , this.observedEventsList);

        },
        error: (error) => {
          console.error('Error loading documents:', error);
        }
      });
    }
    
    mapToObservedEventDocument(data: any[]): ObservedEvent[] {
      return data.map(event => ({
        eventId: Number(event.eventId),
        eventCreationDate: event.eventCreationDate,
        segId: Number(event.segId),
        flyingPilot: Number(event.flyingPilot),
        nonFlyingPilot: Number(event.nonFlyingPilot), // Fixed incorrect assignment
        focusArea: event.focusArea
      }));
    }
  
    clear(table: Table) 
    {
      table.clear();
    }

   
  
    async deleteEvent(eventId: number) 
    {
      const removeSeg = await this.askYesNoQuestionForDeletingSeg("Are you sure?");
      if (removeSeg) {
        try {
          await this.inputRecordService.deleteFormInput(eventId).toPromise(); // Ensure it's waiting for the deletion call to complete
          alert(`Event Id: ${eventId} Deleted`);
          // Reload the documents list after deletion
          this.loadDocuments();
        } catch (error) {
          console.error("Error deleting segment:", error);
          alert("Failed to delete segment.");
        }
      }
    }
  
   askYesNoQuestionForDeletingSeg(question: string): any
   {
      let answer = prompt(question + " (yes or no)");
      if (answer?.toLowerCase() === "yes") {
        return true;
      } else if (answer?.toLowerCase() === "no") {
        return false;
      } else {
        // If the user enters an invalid response, prompt again
        alert("Please enter 'yes' or 'no'");
        return this.askYesNoQuestionForDeletingSeg(question);
      }
    }

     // editting if needed copy the structure of the editSeg component
    // editSeg(segId: number) {
    //   console.log("Navigating to edit:", segId); // Debugging step
    //   this.router.navigate(['/edit', segId]);
    // }


    downloadP1(eventId: number, segId: number, pilotEmpId: number) {
      console.log("Downloading P1 Report for Seg ID:", segId, "Pilot Employee ID:", pilotEmpId, "Event ID:", eventId);
      this.inputRecordService.downloadReportCard(eventId, segId, pilotEmpId)
        .then(() => {
          console.log("P1 Report downloaded successfully");
        })
        .catch((error) => {
          console.error("Error downloading P1 Report:", error);
        });
    }

    downloadP2(eventId: number, segId: number, pilotEmpId: number) {
      console.log("Downloading P2 Report for Seg ID:", segId);
      this.inputRecordService.downloadReportCard(eventId, segId, pilotEmpId)
        .then(() => {
          console.log("P2 Report downloaded successfully");
        })
        .catch((error) => {
          console.error("Error downloading P2 Report:", error);
        });
    }


  }

    
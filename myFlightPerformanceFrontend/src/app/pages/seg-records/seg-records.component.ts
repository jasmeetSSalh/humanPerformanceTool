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


@Component({
  selector: 'app-seg-records',
  standalone: true,
  imports: [TableModule, CommonModule, InputTextModule, TagModule,
    SelectModule, MultiSelectModule, ButtonModule, IconFieldModule, InputIconModule],
  templateUrl: './seg-records.component.html',
  styleUrl: './seg-records.component.css'
})
export class RecordsComponent implements OnInit 
{
  loading: boolean = true;
  segDocumentList: SegDocument[] = [];


  constructor(private recordService: RecordsService, private router: Router) {}

  ngOnInit() 
  {
    this.loadDocuments();
  }

  loadDocuments() 
  {
    // Fetch the updated list of documents from the server
    this.recordService.getAllSegDocuments().subscribe((data) => {
      this.segDocumentList = this.mapToSegDocument(data); // Update the list

      
    }, (error) => {
      console.error('Error loading documents:', error);
    });
  }
  // Mapping function to convert from camelCase to PascalCase
  mapToSegDocument(data: any[]): SegDocument[] 
  {
    return data.map(item => (
    {
      segId: item.segId,               // Mapping camelCase to PascalCase
      segName: item.segName,           // Mapping camelCase to PascalCase
      creationDate: new Date(item.creationDate), // Mapping camelCase to PascalCase
      createdBy: item.createdBy,       // Mapping camelCase to PascalCase
      docName: item.docName,           // Mapping camelCase to PascalCase
      trainingProgram: item.trainingProgram // Mapping camelCase to PascalCase
    }));
  }

  clear(table: Table) 
  {
    table.clear();
  }

  editSeg(segId: number) {
    console.log("Navigating to edit:", segId); // Debugging step
    this.router.navigate(['/edit', segId]);
  }
  

  async deleteSeg(segId: number) 
  {
    const removeSeg = await this.askYesNoQuestionForDeletingSeg("Are you sure?");
    if (removeSeg) {
      try {
        await this.recordService.deleteSegDocument(segId).toPromise(); // Ensure it's waiting for the deletion call to complete
        alert(`Seg Id: ${segId} Deleted`);
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

 
}

import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { SegFormService } from '../../services/seg-form.service';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { DividerModule } from 'primeng/divider';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { ButtonModule } from 'primeng/button';
import { Observation } from '../../models/observation';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { EventTrigger } from '../../models/eventTrigger';
import { SegDocument } from '../../models/segDocument';
import { SegDocumentJunction } from '../../models/segDocumentJunction';
import { ActivatedRoute, Router } from '@angular/router';
import { RecordsService } from '../../services/seg-records.service';
import { SegDocumentJunctionJson } from '../../models/segDocumentRequest';
import {  forkJoin } from 'rxjs';
import { Select } from 'primeng/select';
import { SelectModule } from 'primeng/select';
import { DialogModule } from 'primeng/dialog';
import {InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'seg-generator-component',
  templateUrl: './seg-generator.component.html',
  styleUrl: './seg-generator.component.css',
  standalone: true,
  imports: [ 
    CommonModule, 
    CardModule, 
    DividerModule, 
    ReactiveFormsModule, 
    DropdownModule, 
    MultiSelectModule, 
    ButtonModule, 
    InputTextModule,
    DragDropModule,
    Select,
    SelectModule,
    DialogModule,
    FormsModule
  ]
})

export class SegGeneratorComponent implements OnInit {
  segForm: FormGroup;

  isVisibleProcedure: boolean = false;
  isVisibleOther: boolean = false;

  //edit mode specific vars
  editMode: boolean = false;
  currentSegId: number | null = null;
  private fb = inject(FormBuilder);
  
  // Used to route to the page when editting
  private route = inject(ActivatedRoute);

  // Used to capture a new Event Trigger Information
  newEventTriggerDescription: string = '';

  // Used to store all the field information
  $existingMalfunctions: EventTrigger[] = [];
  $existingProcedures: EventTrigger[] = [];
  $existingOthers: EventTrigger[] = [];
  $existingObservation: Observation[] = [];

  masterSelectionType: any[] = [
    { label: "Malfunction", array: this.$existingMalfunctions },
    { label: "Procedure", array: this.$existingProcedures },
    { label: "Other", array: this.$existingOthers}
  ];

  isButtonDisabled = false;


  constructor(
    private segFormService: SegFormService,
    private recordFormService: RecordsService,
    private router: Router
    ) {

    this.segForm = this.fb.group({
      segDocument: this.fb.group({
        TrainingProgram: ['Human Performance Training Program'],
        DocName: ['', Validators.required],
        SegName: ['', Validators.required],
        CreationDate: ['', Validators.required],
        CreatedBy: ['', Validators.required],
      }),
      segDocumentJunction: this.fb.array([]),
    });
  }

  async ngOnInit() {
    await this.loadDataPromise(); // make sure master data is loaded

    this.route.paramMap.subscribe(async params => {
      const SegId = params.get('SegId');
      if (SegId) {
        this.editMode = true;
        this.currentSegId = +SegId;
        this.loadDocumentData(this.currentSegId);
      }
    });
  }

  // -------------------------------------- Field Data Loading Function ------------------------------------------- v

  loadDataPromise(): Promise<void> {
    return new Promise((resolve, reject) => {
      forkJoin({
        malfunctions: this.segFormService.getMalfunctions(),
        procedures: this.segFormService.getProcedures(),
        others: this.segFormService.getOthers(),
        observations: this.segFormService.getObservations()
      }).subscribe({
        next: (data) => {
          this.$existingMalfunctions = data.malfunctions || [];
          this.$existingProcedures = data.procedures || [];
          this.$existingOthers = data.others || [];
          this.$existingObservation = data.observations || [];
          this.updateMasterSelection();
          resolve();
        },
        error: (error) => {
          console.error("Error in loadData:", error);
          reject(error);
        }
      });
    });
  }
  

  private formatDate(dateString: string): string {
    if (!dateString) return ''; // Handle empty/null case
    return dateString.split('T')[0]; // Extract only the "yyyy-MM-dd" part
  }
  

  // ------------------------------------------------ Edit Mode ----------------------------------- v


  loadDocumentData(SegId: number) {
    this.recordFormService.getSpecificSegDocument(SegId).subscribe({
    next: (data) => {
      if (data.segDocumentInput && data.segDocumentJunctionsInput) {
        // Patch segDocument fields as before ...
          const segDocument = data.segDocumentInput;
         
          const creationDate = segDocument.creationDate ? new Date(segDocument.creationDate).toISOString().split('T')[0] : '';
          console.log("Creation Date From Backend: ", creationDate);

          this.segForm.get('segDocument')?.patchValue({
            TrainingProgram: segDocument.trainingProgram,
            DocName: segDocument.docName,
            SegName: segDocument.segName,
            CreationDate: creationDate,
            CreatedBy: segDocument.createdBy,
          });
        // Prepare and sort junctions
        const junctions = Array.isArray(data.segDocumentJunctionsInput)
          ? data.segDocumentJunctionsInput.slice()
          : [];
        junctions.sort((a, b) => {
          if (typeof a.position === 'number' && typeof b.position === 'number') {
            return a.position - b.position;
          }
          return 0;
        });

        const junctionArray = this.segForm.get("segDocumentJunction") as FormArray;
        junctionArray.clear();

        junctions.forEach((junction: SegDocumentJunctionJson, index: number) => {
          const trigIdRead = +junction.trigId;
          const masterSelection = this.getCategoryLabelBytrigId(junction.trigId);
          const eventTrigger = this.getEventTriggerBytrigId(trigIdRead);
          const behavioursArray = Array.isArray(junction.obsId) ? junction.obsId : [];
          const behaviours = this.getBehavioursByIds(behavioursArray);
          const position = typeof junction.position === 'number' ? junction.position : index;
          this.addNewJunctionEditMode(masterSelection, eventTrigger, behaviours, position);
        });
      } else {
        console.error("Error: segDocumentInput data is missing or malformed", data);
      }
    },
    error: (error) => {
      console.error("An error occurred while fetching the document data:", error);
    }
  });
}

  
  getCategoryLabelBytrigId(eventTriggerId: number): any {
    // Check if the eventTriggerId exists in any of the arrays by matching trigId
    const malfunction = this.$existingMalfunctions.find(item => item.trigId === eventTriggerId);
    const procedure = this.$existingProcedures.find(item => item.trigId === eventTriggerId);
    const other = this.$existingOthers.find(item => item.trigId === eventTriggerId);
    
    if (malfunction) {
      return { label: 'Malfunction', array: this.$existingMalfunctions };
    } else if (procedure) {
      return { label: 'Procedure', array: this.$existingProcedures };
    } else if (other) {
      return { label: 'Other', array: this.$existingOthers };
    } else {
      return {label: 'Everything went wrong', array: this.$existingMalfunctions};
    }
  }

  getCategoryLabel(eventTrigger: EventTrigger): string {
    if (!eventTrigger) return '';
  
    if (this.$existingMalfunctions.some(e => e.trigId === eventTrigger.trigId)) {
      return 'Malfunction';
    } else if (this.$existingProcedures.some(e => e.trigId === eventTrigger.trigId)) {
      return 'Procedure';
    } else if (this.$existingOthers.some(e => e.trigId === eventTrigger.trigId)) {
      return 'Other';
    }
  
    return '';
  }

  getEventTriggerBytrigId(trigId: number): EventTrigger {
    const emptyEventTrigger: EventTrigger = {
      trigId: -1,
      trigCat: '',
      trigDes: '',
      trigType: 0
    };
  
    if (trigId == null) {
      console.warn("trigId is null or undefined, defaulting to 1");
      trigId = 1; // Default value to avoid errors
    }
  
    return (
      this.$existingMalfunctions.find(item => item.trigId === trigId) ||
      this.$existingProcedures.find(item => item.trigId === trigId) ||
      this.$existingOthers.find(item => item.trigId === trigId) ||
      emptyEventTrigger
    );
  }

  getBehavioursByIds(obsIds: number[]): Observation[] {
    return this.$existingObservation.filter(obs => obsIds.includes(obs.obsId));
  }
  
  get segDocumentJunction(): FormArray {
    //console.log(this.segForm.get('segDocumentJunction'));
    return this.segForm.get('segDocumentJunction') as FormArray;
  }

  
  

  // -------------------------------- Junction Editing ------------------------------------- v

  // Call this after any reordering or removal to update the position/index
updateJunctionPositions() {
  this.segDocumentJunction.controls.forEach((ctrl, idx) => {
    ctrl.patchValue({ position: idx }, { emitEvent: false });
  });
}

  updateMasterSelection() {
    this.masterSelectionType = [
      { label: "Malfunction", array: this.$existingMalfunctions },
      { label: "Procedure", array: this.$existingProcedures },
      { label: "Other", array: this.$existingOthers }
    ];
  }

  onMasterSelectionChange(event: any, index: number) {
    const selectedCategory = event.value;
    console.log('Selected category:', selectedCategory);
    
    const junctions = this.segDocumentJunction;
    const selectedJunction = junctions.at(index) as FormGroup;

    if (selectedJunction) {
      selectedJunction.patchValue({ 
        CurrentEventTrigger: null,
        CurrentBehaviours: []
      });
    }
  }

  getEventTriggersForCategory(categoryLabel: string, index: number): EventTrigger[] {
    //console.log('getEventTriggersForCategory called with:', categoryLabel, 'index:', index);
    
    if (categoryLabel === 'Malfunction') {
      console.log('Returning malfunctions:', this.$existingMalfunctions);
      return this.$existingMalfunctions;
    } else if (categoryLabel === 'Procedure') {
      console.log('Returning procedures:', this.$existingProcedures);
      return this.$existingProcedures;
    } else if (categoryLabel === 'Other') {
      console.log('Returning others:', this.$existingOthers);
      return this.$existingOthers;
    }
    
    console.log('No matching category found, returning empty array');
    return [];
  }

  getCategoryArrayForJunction(index: number): EventTrigger[] {
    const junction = this.segDocumentJunction.at(index) as FormGroup;
    const selection = junction.get('CurrentMasterSelections')?.value;

    if (selection.label === 'Malfunction') {
      return this.$existingMalfunctions;
    } else if (selection.label === 'Procedure') {
      return this.$existingProcedures;
    } else if (selection.label === 'Other') {
      return this.$existingOthers;
    }
    
    return [];
  }


  addNewJunction() {
    this.segDocumentJunction.push(this.createSegDocumentJunction());
  }

  createSegDocumentJunction(): FormGroup {
    return this.fb.group({
      CurrentMasterSelections: [''],
      CurrentEventTrigger: [''],
      CurrentBehaviours: [[]],
      position: [this.segDocumentJunction.length] // Set to current length
    });
  }

addNewJunctionEditMode(
  MasterSelection: any,
  EventTrigger: EventTrigger,
  Behaviours: Observation[],
  position: number
) {
  this.segDocumentJunction.push(
    this.createSegDocumentJunctionWithParams(MasterSelection, EventTrigger, Behaviours, position)
  );
}

createSegDocumentJunctionWithParams(
  MasterSelection: string,
  EventTrigger: EventTrigger,
  Behaviours: Observation[],
  position: number
) {
  return this.fb.group({
    CurrentMasterSelections: [MasterSelection],
    CurrentEventTrigger: [EventTrigger],
    CurrentBehaviours: [Behaviours],
    position: [position]
  });
}

removeJunction(index: number) {
  this.segDocumentJunction.removeAt(index);
  this.updateJunctionPositions();
}

  // Handle drag and drop reordering
  dropJunction(event: CdkDragDrop<string[]>) {
    moveItemInArray(
      this.segDocumentJunction.controls,
      event.previousIndex,
      event.currentIndex
    );
    this.updateJunctionPositions();
  }


  /// --------------------- Drag & Droping Logic --------------------------

  // Move item up in the array
  moveUp(index: number) {
    if (index > 0) {
      moveItemInArray(this.segDocumentJunction.controls, index, index - 1);
      this.updateJunctionPositions();
    }
  }

  moveDown(index: number) {
    if (index < this.segDocumentJunction.length - 1) {
      moveItemInArray(this.segDocumentJunction.controls, index, index + 1);
      this.updateJunctionPositions();
    }
  }

  handleButtonClick() {
    this.isButtonDisabled = true;
    window.alert(this.segForm.get('segDocument.SegName')?.value + " Submitted");
    this.router.navigate(['/records'])
    this.onSubmit();
  
    // Handle the form submission or whatever logic you need here
  }

  onSubmit() {
  
      if (this.segForm.valid) {
        const segDocument: SegDocument = {
          segId: Number(this.currentSegId) ,
          segName: this.segForm.get('segDocument.SegName')?.value, // Match form control name
          creationDate: this.segForm.get('segDocument.CreationDate')?.value,
          createdBy: this.segForm.get('segDocument.CreatedBy')?.value, // Match form control name
          docName: this.segForm.get('segDocument.DocName')?.value, // Match form control name
          trainingProgram: this.segForm.get('segDocument.TrainingProgram')?.value // Match form control name
        };


        console.log("Suppose to the date: ", this.segForm.get('segDocument.CreationDate')?.value);
    
        // Ensure segDocumentJunctions is properly formatted as an array
        const segDocumentJunctionsRaw = this.segForm.get('segDocumentJunction')?.value;
        console.log("Raw", segDocumentJunctionsRaw);
        const segDocumentJunctions: SegDocumentJunction[] = Array.isArray(segDocumentJunctionsRaw)
          ? segDocumentJunctionsRaw.map((junction: any, index: number) => ({
              trigId: junction.CurrentEventTrigger.trigId,
              position: index, // Assigning position explicitly based on array index
              obsId: junction.CurrentBehaviours?.map((behaviour: any) => behaviour.obsId) || []
            }))
          : [];
            
        // The API expects an object with segDocumentInput and segDocumentJunctionsInput
        const requestPayload = {
          segDocumentInput: segDocument,
          segDocumentJunctionsInput: segDocumentJunctions
        };
    
        console.log("Sending payload:", JSON.stringify(requestPayload, null, 2)); // Debugging
    
        if (segDocument.segId != null && segDocument.segId != 0 && this.editMode){
          this.recordFormService.updateSegDocument(requestPayload).subscribe({
            next: (response) => {
              console.log('Document submitted successfully:', response);
            },
            error: (error) => {
              console.error('Error submitting document:', error);
            }
          });
        } else{
          this.segFormService.submitSegDocument(requestPayload).subscribe({
            next: (response) => {
              console.log('Document submitted successfully:', response);
            },
            error: (error) => {
              console.error('Error submitting document:', error);
            }
          });
        }
        

       
      } else {
        console.log("The form is not valid. Please fill all the required fields.");
      }
    
  }
  
  // Add event trigger
  addNewEventTrigger(inputType: string) {
    if (inputType == 'Procedure') {
      this.segFormService.insertProcedure(this.newEventTriggerDescription).subscribe({
        next: (response) => {
          if (response !== null && response !== undefined) {
            const newProcedure: EventTrigger = {
              trigId: response,
              trigCat: 'Procedure',
              trigDes: this.newEventTriggerDescription,
              trigType: 1
            };
            // Option 1: Push to array and update master selection
            this.$existingProcedures.push(newProcedure);
            this.updateMasterSelection();
            this.newEventTriggerDescription = '';
            // Option 2: (preferred) If loadData() reloads everything from backend, just call it:
            // this.loadData();
          } else {
            alert('Failed to insert procedure. No ID returned.');
          }
        },
        error: (error) => {
          console.error('Error inserting procedure:', error);
          alert('Error inserting procedure. Please try again.');
        }
      });
    } else if (inputType == 'Other') {
      this.segFormService.insertOther(this.newEventTriggerDescription).subscribe({
        next: (response) => {
          if (response !== null && response !== undefined) {
            const newOther: EventTrigger = {
              trigId: response,
              trigCat: 'Other', // Fix the category here
              trigDes: this.newEventTriggerDescription,
              trigType: 1
            };
            this.$existingOthers.push(newOther);
            this.updateMasterSelection();
            this.newEventTriggerDescription = '';
            // Or use this.loadData() if you want to reload from backend
            // this.loadData();
          } else {
            alert('Failed to insert procedure. No ID returned.');
          }
        },
        error: (error) => {
          console.error('Error inserting procedure:', error);
          alert('Error inserting procedure. Please try again.');
        }
      });
    }
  }
}

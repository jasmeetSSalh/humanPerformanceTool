import { Component, OnInit } from "@angular/core";
import { StepperModule } from "primeng/stepper";
import { ButtonModule } from "primeng/button";
import { ActivatedRoute, Router } from "@angular/router";
import { SegDocument } from "../../models/segDocument";
import { InputService } from "../../services/input.service";
import { TableModule } from "primeng/table";
import { CheckboxModule } from "primeng/checkbox";
import { CommonModule } from "@angular/common";
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  Validators,
} from "@angular/forms";
import { SplitterModule } from "primeng/splitter";
import { ReactiveFormsModule } from "@angular/forms";
import { SegFormInputJunction } from "../../models/segFormInputJunction";
import { DividerModule } from "primeng/divider";
import { RadioButtonModule } from "primeng/radiobutton";
import { createObservedGrade, GradeValue, ObservedGrade } from "../../models/observedGrade";
import { SelectModule } from 'primeng/select';
import { SliderModule } from 'primeng/slider';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { ObservedEvent } from "../../models/observedEvent";
import { Categories, ObservedGradeSummary } from "../../models/observedGradeSumary";



@Component({
  selector: "app-input",
  imports: [
    RadioButtonModule,
    SelectModule,
    DividerModule,
    ReactiveFormsModule,
    StepperModule,
    ButtonModule,
    TableModule,
    CheckboxModule,
    CommonModule,
    FormsModule,
    SplitterModule,
    InputNumberModule,
    SliderModule,
    InputTextModule,
  ],
  standalone: true,
  templateUrl: "./input.component.html",
  styleUrl: "./input.component.css",
})
export class InputComponent implements OnInit {
  inputForm: FormGroup;

  currentSeg: SegDocument = {} as SegDocument;
  currentSegJunctions: SegFormInputJunction[] = [];


  pilotGradesL: ObservedGrade[] = [];
  pilotGradesR: ObservedGrade[] = [];

  pilotLGrades: number[][] = []; // Stores grades for Pilot L
  pilotRGrades: number[][] = []; // Stores grades for Pilot R

  grades = [
    { label: 'Negative One', value: GradeValue.NegativeOne },
    { label: 'Zero', value: GradeValue.Zero },
    { label: 'One', value: GradeValue.One }
  ];

  focusAreas = [
    { label: 'Procedure Use'},
    { label: 'Briefings'},
    { label: 'Communications'},
    { label: 'Stop When Unsure'},
    { label: 'Self Check'},
    { label: 'Conservatism'},
    { label: 'Second Party Verification'}
  ]
  

  checkListPreEventData = [
    { label: "Clear all malfunctions and reset all quantities in simulator",var: "c1",},
    { label:"Reset coordinator PC to show correct flight plan and remove past aircraft track.", var: "c2",},
    { label: "Restore simulator to lesson plan N-SEG-78318-00008", var: "c3" },
    { label: "Align simulator controls for normal level flight", var: "c4" },
    { label: "Complete Pre-Event Check Sheet", var: "c5" },
    { label: "Reference procedures for exercise are not marked up (e.g., OMs/ARMs/Flowsheets).",var: "c6",},
    { label: "Reference procedures are in the normal location, up to date, and filed in the correct sequence (OMs, Flowsheets).",var: "c7",},
    { label: "Intercom/Radio system is operational (requires aircraft battery switch to be closed).",var: "c8",},
    { label: "Ensure that the documentation at the simulator instructor station is in the normal location, up to date, and in the correct sequence.", var: "c9",},
    { label: "Ensure pens, markers, paper, and sticky tabs are available as required.", var: "c10",},
    { label: "Check operation of debrief computer.", var: "c11" },
    { label: "Audio and video are poised for recording.", var: "c12" },
    { label: "Auto-Throttle Enabled.", var: "c13" },
  ];

  segJunctionOtherSectionValueVarForm: number = 0;

  constructor(
    private router: Router,
    private inputService: InputService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
   
  ) {
    this.inputForm = this.fb.group({
      c1: [false],
      c2: [false],
      c3: [false],
      c4: [false],
      c5: [false],
      c6: [false],
      c7: [false],
      c8: [false],
      c9: [false],
      c10: [false],
      c11: [false],
      c12: [false],
      c13: [false],
      checked: [false],
      pilotL: [],
      pilotR: [],
      events: this.fb.array([]), // Holds all events dynamically
      
      currentFocusArea: [], // Add default null value
      leftProceduralScore: [0],
      rightProceduralScore: [0],
      leftSituationalScore: [0],
      rightSituationalScore: [0],
      leftVerificationScore: [0],
      rightVerificationScore: [0],
      leftOverallScore: [0],
      rightOverallScore: [0]
    
    });

   
  }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params) => {
      const segId = params.get("SegId");
      if (segId) {
        this.initializeSeg(+segId);
      }
    });
  }

  initializeSeg(segId: number): void {
    this.inputService.getSpecificSegDocument(segId).subscribe({
      next: (data) => {
        if (data) {
          this.currentSeg.segId = Number(data.segId);
          this.currentSeg.segName = data.segName;
          this.currentSeg.creationDate = data.creationDate;
          this.currentSeg.createdBy = data.createdBy;
          this.currentSeg.docName = data.docName;
          this.currentSeg.trainingProgram = data.trainingProgram;
        } else {
          console.error("Data is not there");
        }
      },
      error: (err) => {
        console.error("Error retrieving data:", err);
      },
    });

    this.inputService.getSpecificSegDocumentJunction(segId).subscribe({
      next: (data) => {
        console.log("Fetched Events Data:", data); // Debugging log
        if (data) {
          this.events.clear(); // Ensure we start fresh
          data.forEach((event) => {
            this.events.push(this.createEventFormGroup(event));
          });

          // Ensure currentSegJunctions is updated
          this.currentSegJunctions = data;
          console.log("Updated currentSegJunctions:", this.currentSegJunctions);
        } else {
          console.error("Data is not there");
        }
      },
      error: (err) => {
        console.error("Error retrieving data:", err);
      },
    });

    console.log("Current Seg: ", this.currentSeg);
    console.log("Current Junctions: ", this.currentSegJunctions);
  }

  get events(): FormArray {
    return this.inputForm.get("events") as FormArray;
  }

  getObservations(eventIndex: number): FormGroup[] {
    const observationsArray = this.events.at(eventIndex)?.get("observations") as FormArray;
    return observationsArray.controls as FormGroup[];
  }
  
  createEventFormGroup(event: any): FormGroup {
    return this.fb.group({
      eventTrigger: [event.eventTrigger],
      observations: this.fb.array(
        (event.observations || []).map((obs: any) =>
          this.createObservationFormGroup(obs)
        )
      ),
    });
  }
  

  createObservationFormGroup(observation: any): FormGroup {
    return this.fb.group({
      obsId: [observation.obsId],
      behaviour: [observation.behaviour],
      subCategory: [observation.subCategory],
      lGrade: this.fb.group({
        obsId: [observation.lGrade?.obsId || observation.obsId],
        pilotEmpId: [null], // Now explicitly passing pilotL
        grade: [GradeValue.Zero],
      }),
      rGrade: this.fb.group({
        obsId: [observation.rGrade?.obsId || observation.obsId],
        pilotEmpId: [null], // Now explicitly passing pilotR
        grade: [GradeValue.Zero],
      }),
    });
  }
  
  
  

  createObservedGradeFormGroup(obsId: number): FormGroup {
    return this.fb.group({
      obsId: obsId,
      pilotEmpId: new FormControl(""), // User-inputted later
      grade: new FormControl(GradeValue.Zero), // Default
    });
  }

  get checkListPreEvent(): FormArray {
    return this.inputForm.get("checkListPreEvent") as FormArray;
  }

  checkAllBoxes(): void {
    this.checkListPreEventData.forEach((item) => {
      this.inputForm.get(item.var)?.setValue(true); // Automatically set the checkbox values to true
    });
  }

  GetFocusAreaScore(pilot: string): number {

    let score: number = 24;
    
    this.events.controls.forEach(eventGroup => {
      const observationsArray = eventGroup.get("observations") as FormArray;

      observationsArray.controls.forEach(observation => {
        const gradeControl = observation.get(pilot === "l" ? "lGrade": "rGrade");
        if (gradeControl) {
          const grade = gradeControl.get("grade")?.value;
          if (grade !== undefined) {
            score += grade;
          }
        }
      })
    })
    return score;
  }

  calculateOverallScore(): void {

    let leftScore =
      this.inputForm.get("leftProceduralScore")?.value +
      this.inputForm.get("leftSituationalScore")?.value +
      this.inputForm.get("leftVerificationScore")?.value;
    let rightScore =
      this.inputForm.get("rightProceduralScore")?.value +
      this.inputForm.get("rightSituationalScore")?.value +
      this.inputForm.get("rightVerificationScore")?.value;
  
    leftScore = leftScore + this.GetFocusAreaScore("l");
    rightScore = rightScore + this.GetFocusAreaScore("r");

    this.inputForm.patchValue({
      leftProceduralScore : this.inputForm.get("leftProceduralScore")?.value,
      leftSituationalScore : this.inputForm.get("leftSituationalScore")?.value,
      leftVerificationScore: this.inputForm.get("leftVerificationScore")?.value,
      leftOverallScore: leftScore
    
    });
    this.inputForm.patchValue({
      rightProceduralScore : this.inputForm.get("rightProceduralScore")?.value,
      rightSituationalScore : this.inputForm.get("rightSituationalScore")?.value,
      rightVerificationScore: this.inputForm.get("rightVerificationScore")?.value,
      rightOverallScore: rightScore
    })
  }
  
  onStepClick(event: Event) {
    //console.log('Step clicked', event);
    event.preventDefault(); // Prevent unexpected form submission
  }


  onSubmit(): void {
    console.log("Form Submitted", this.inputForm.value);

    const observedEventFromForm : ObservedEvent = {
      segId: Number(this.currentSeg.segId),
      flyingPilot: Number(this.inputForm.get("pilotL")?.value),
      nonFlyingPilot: Number(this.inputForm.get("pilotR")?.value),
      focusArea: this.inputForm.get("currentFocusArea")?.value?.label || "No Focus Area"
    };

    const observedGradesFromForm: ObservedGrade[] = [];

    this.events.controls.forEach((eventGroup) => {
      const observationsArray = eventGroup.get("observations") as FormArray;

      observationsArray.controls.forEach((observationGroup) => {
        const obsId = observationGroup.get("obsId")?.value;

        // Get left and right grades
        const lGradeForm = observationGroup.get("lGrade");
        const rGradeForm = observationGroup.get("rGrade");

        if (lGradeForm) {
          const lGrade: ObservedGrade = createObservedGrade(
            obsId,
            Number(this.inputForm.get("pilotL")?.value),
            lGradeForm.get("grade")?.value
          );
          observedGradesFromForm.push(lGrade);
        }

        if (rGradeForm) {
          const rGrade: ObservedGrade = createObservedGrade(
            obsId,
            Number(this.inputForm.get("pilotR")?.value),
            rGradeForm.get("grade")?.value
          );
          observedGradesFromForm.push(rGrade);
        }
      });
    });

    const observedGradeSummaryFromForm: ObservedGradeSummary[] = [];

    observedGradeSummaryFromForm.push({
      category: Categories.ProceduralUseAdherence,
      pilotEmpId: Number(this.inputForm.get("pilotL")?.value),
      score: this.inputForm.get("leftProceduralScore")?.value
    });
    
    observedGradeSummaryFromForm.push({
      category: Categories.ProceduralUseAdherence,
      pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
      score: this.inputForm.get("rightProceduralScore")?.value
    });

    observedGradeSummaryFromForm.push({
      category: Categories.SituationalAwareness,
      pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
      score: this.inputForm.get("leftSituationalScore")?.value
    });
    observedGradeSummaryFromForm.push({
      category: Categories.SituationalAwareness,
      pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
      score: this.inputForm.get("rightSituationalScore")?.value
    });
    
    observedGradeSummaryFromForm.push({
      category: Categories.VerficationPractices,
      pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
      score: this.inputForm.get("leftVerificationScore")?.value
    });
    observedGradeSummaryFromForm.push({
      category: Categories.VerficationPractices,
      pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
      score: this.inputForm.get("rightVerificationScore")?.value
    });

    observedGradeSummaryFromForm.push({
      category: Categories.OverallScore,
      pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
      score: this.inputForm.get("leftOverallScore")?.value
    });
    observedGradeSummaryFromForm.push({
      category: Categories.OverallScore,
      pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
      score: this.inputForm.get("rightOverallScore")?.value
    });
    


    // Correcting request payload structure
    const requestParameters = {
      observedEventInput: observedEventFromForm,
      observeredGradesInput: observedGradesFromForm,
      observedGradesSummaryInput: observedGradeSummaryFromForm 
    };

    // Call the service method with corrected structure
    this.inputService.inputForm(requestParameters).subscribe({
      next: (response) => console.log("Successfully submitted:", response),
      error: (err) => console.error("Submission error:", err),
      });
  }

}

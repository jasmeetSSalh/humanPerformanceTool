import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
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
import { TextareaModule } from 'primeng/textarea';
import { SelectButtonModule } from 'primeng/selectbutton';


@Component({
  selector: "app-input",
  imports: [
    SelectButtonModule,
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
    TextareaModule
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

  pilotLGrades: number[][] = [];
  pilotRGrades: number[][] = [];

  grades = [
    { label: 'Negative One', value: GradeValue.NegativeOne },
    { label: 'Zero', value: GradeValue.Zero },
    { label: 'One', value: GradeValue.One }
  ];

 

  focusAreas = [
    { label: 'Procedure Use & Adherence'},
    { label: 'Briefings'},
    { label: 'Communications'},
    { label: 'Stop When Unsure'},
    { label: 'Self Check'},
    { label: 'Conservatism'},
    { label: 'Situational Awareness'},
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
  

  // Step loading performance: Only render critique step panel when active
  stepActive: boolean = false;

  // Optionally, provide a style object for textareas to bind in template
  commentAreaStyle = {
    width: '100%',
    minWidth: '200px',
    maxWidth: '400px'
  };

  stateOptions: any[] = [
        { label: 'Off', value: false },
        { label: 'On', value: true }
    ];
    

  constructor(
    private router: Router,
    private inputService: InputService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
   private cdr: ChangeDetectorRef

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

      useProcedural: [true],
      leftProceduralScore: [{ value: 0, disabled: false }],
      leftProceduralComment: [{ value: '', disabled: false }],
      rightProceduralScore: [{ value: 0, disabled: false }],
      rightProceduralComment: [{ value: '', disabled: false }],

      useBriefing: [true],
      leftBriefingScore: [{ value: 0, disabled: false }],
      leftBriefingComment: [{ value: '', disabled: false }],
      rightBriefingScore: [{ value: 0, disabled: false }],
      rightBriefingComment: [{ value: '', disabled: false }],

      useCommunication: [true],
      leftCommunicationScore: [{ value: 0, disabled: false }],
      leftCommunicationComment: [{ value: '', disabled: false }],
      rightCommunicationScore: [{ value: 0, disabled: false }],
      rightCommunicationComment: [{ value: '', disabled: false }],

      useStopWhenUnsure: [true],
      leftStopWhenUnsureScore: [{ value: 0, disabled: false }],
      leftStopWhenUnsureComment: [{ value: '', disabled: false }],
      rightStopWhenUnsureScore: [{ value: 0, disabled: false }],
      rightStopWhenUnsureComment: [{ value: '', disabled: false }],

      useSelfCheck: [true],
      leftSelfCheckScore: [{ value: 0, disabled: false }],
      leftSelfCheckComment: [{ value: '', disabled: false }],
      rightSelfCheckScore: [{ value: 0, disabled: false }],
      rightSelfCheckComment: [{ value: '', disabled: false }],

      useConservatism: [true],
      leftConservatismScore: [{ value: 0, disabled: false }],
      leftConservatismComment: [{ value: '', disabled: false }],
      rightConservatismScore: [{ value: 0, disabled: false }],
      rightConservatismComment: [{ value: '', disabled: false }],

      useSituational: [true],
      leftSituationalScore: [{ value: 0, disabled: false }],
      leftSituationalComment: [{ value: '', disabled: false }],
      rightSituationalScore: [{ value: 0, disabled: false }],
      rightSituationalComment: [{ value: '', disabled: false }],

      useVerification: [true],
      leftVerificationScore: [{ value: 0, disabled: false }],
      leftVerificationComment: [{ value: '', disabled: false }],
      rightVerificationScore: [{ value: 0, disabled: false }],
      rightVerificationComment: [{ value: '', disabled: false }],

      leftOverallComment: [{ value: '', disabled: false }],
      rightOverallComment: [{ value: '', disabled: false }],

      leftOverallScore: [{ value: 0, disabled: true }],
      rightOverallScore: [{ value: 0, disabled: true }]


    });
  }

   ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const segId = params.get("SegId");
      if (segId) {
        this.initializeSeg(+segId);
      }
    });
  }

  setEnableState(controlName: string, enabled: boolean) {
    const ctrl = this.inputForm.get(controlName);
    if (!ctrl) return;
    if (enabled) {
      ctrl.enable({ emitEvent: false });
    } else {
      ctrl.disable({ emitEvent: false });
    }
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
        if (data) {
          this.events.clear();
          // Sort by position if present
          const sortedData = data.slice().sort((a, b) => {
            if (typeof a.position === 'number' && typeof b.position === 'number') {
              return a.position - b.position;
            }
            return 0;
          });
          sortedData.forEach((event) => {
            this.events.push(this.createEventFormGroup(event));
          });
          this.currentSegJunctions = sortedData;
        } else {
          console.error("Data is not there");
        }
      },
      error: (err) => {
        console.error("Error retrieving data:", err);
      },
    });
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
        pilotEmpId: [null],
        grade: [GradeValue.Zero],
      }),
      rGrade: this.fb.group({
        obsId: [observation.rGrade?.obsId || observation.obsId],
        pilotEmpId: [null],
        grade: [GradeValue.Zero],
      }),
    });
  }
  
  createObservedGradeFormGroup(obsId: number): FormGroup {
    return this.fb.group({
      obsId: obsId,
      pilotEmpId: new FormControl(""),
      grade: new FormControl(GradeValue.Zero),
    });
  }

  get checkListPreEvent(): FormArray {
    return this.inputForm.get("checkListPreEvent") as FormArray;
  }

  checkAllBoxes(): void {
    this.checkListPreEventData.forEach((item) => {
      this.inputForm.get(item.var)?.setValue(true);
    });
  }

  GetObservationPoints(): number {

    var observationPoints = 0;
    this.events.controls.forEach(eventGroup => {
        const observationsArray = eventGroup.get("observations") as FormArray;
        // where the value is not zero
        observationPoints += observationsArray.controls.filter(obs => obs.get("lGrade.grade")?.value !== 0).length;
    });

    return observationPoints;
  }

  GetFocusAreaScore(pilot: string): number {

    let score: number = 0; // needs to updated to the number of observation points for each element
    //console.log("Count of Triggers: ", this.currentSegJunctions.length);


    this.events.controls.forEach(eventGroup => {
      const observationsArray = eventGroup.get("observations") as FormArray;
      observationsArray.controls.forEach(observation => {
        const gradeControl = observation.get(pilot === "r" ? "rGrade": "lGrade");
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
      this.cdr.detectChanges();

    let leftScore = 0;
    let rightScore = 0;

    const focusAreas = [
      { use: 'useProcedural', left: 'leftProceduralScore', right: 'rightProceduralScore' },
      { use: 'useSituational', left: 'leftSituationalScore', right: 'rightSituationalScore' },
      { use: 'useVerification', left: 'leftVerificationScore', right: 'rightVerificationScore' },
      { use: 'useBriefing', left: 'leftBriefingScore', right: 'rightBriefingScore' },
      { use: 'useCommunication', left: 'leftCommunicationScore', right: 'rightCommunicationScore' },
      { use: 'useStopWhenUnsure', left: 'leftStopWhenUnsureScore', right: 'rightStopWhenUnsureScore' },
      { use: 'useSelfCheck', left: 'leftSelfCheckScore', right: 'rightSelfCheckScore' },
      { use: 'useConservatism', left: 'leftConservatismScore', right: 'rightConservatismScore' },
    ];

    focusAreas.forEach(area => {
      if (this.inputForm.get(area.use)?.value) {
        leftScore += this.inputForm.get(area.left)?.value || 0;
        rightScore += this.inputForm.get(area.right)?.value || 0;
      }
    });

    leftScore += this.GetFocusAreaScore("l");
    rightScore += this.GetFocusAreaScore("r");

    this.inputForm.patchValue({
      // patch all the values in the form so they update in the ui
      leftProceduralScore: this.inputForm.get('useProcedural')?.value ? this.inputForm.get('leftProceduralScore')?.value : 0,
      rightProceduralScore: this.inputForm.get('useProcedural')?.value ? this.inputForm.get('rightProceduralScore')?.value : 0,

      leftSituationalScore: this.inputForm.get('useSituational')?.value ? this.inputForm.get('leftSituationalScore')?.value : 0,
      rightSituationalScore: this.inputForm.get('useSituational')?.value ? this.inputForm.get('rightSituationalScore')?.value : 0,

      leftVerificationScore: this.inputForm.get('useVerification')?.value ? this.inputForm.get('leftVerificationScore')?.value : 0,
      rightVerificationScore: this.inputForm.get('useVerification')?.value ? this.inputForm.get('rightVerificationScore')?.value : 0,

      leftBriefingScore: this.inputForm.get('useBriefing')?.value ? this.inputForm.get('leftBriefingScore')?.value : 0,
      rightBriefingScore: this.inputForm.get('useBriefing')?.value ? this.inputForm.get('rightBriefingScore')?.value : 0,

      leftCommunicationScore: this.inputForm.get('useCommunication')?.value ? this.inputForm.get('leftCommunicationScore')?.value : 0,
      rightCommunicationScore: this.inputForm.get('useCommunication')?.value ? this.inputForm.get('rightCommunicationScore')?.value : 0,

      leftStopWhenUnsureScore: this.inputForm.get('useStopWhenUnsure')?.value ? this.inputForm.get('leftStopWhenUnsureScore')?.value : 0,
      rightStopWhenUnsureScore: this.inputForm.get('useStopWhenUnsure')?.value ? this.inputForm.get('rightStopWhenUnsureScore')?.value : 0,

      leftSelfCheckScore: this.inputForm.get('useSelfCheck')?.value ? this.inputForm.get('leftSelfCheckScore')?.value : 0,
      rightSelfCheckScore: this.inputForm.get('useSelfCheck')?.value ? this.inputForm.get('rightSelfCheckScore')?.value : 0,

      leftConservatismScore: this.inputForm.get('useConservatism')?.value ? this.inputForm.get('leftConservatismScore')?.value : 0,
      rightConservatismScore: this.inputForm.get('useConservatism')?.value ? this.inputForm.get('rightConservatismScore')?.value : 0,

      leftOverallScore: leftScore,
      rightOverallScore: rightScore
    });
  }
  
  
  onStepClick(event: Event) {
    event.preventDefault(); // Prevent unexpected form submission
  }


  onSubmit(): void {

    // Pilot Validation

    const pilotL = this.inputForm.get("pilotL")?.value;
    const pilotR = this.inputForm.get("pilotR")?.value;

    if (!pilotL || !pilotR) {
      // You can use your own error display mechanism here
      alert("Both Pilot L and Pilot R must be selected before submitting.");
      return; // Prevent submission
    }



    //console.log("Form Submitted", this.inputForm.value);

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

    if (this.inputForm.get("useProcedural")?.value) {
      observedGradeSummaryFromForm.push({
        category: Categories.ProceduralUseAdherence,
        pilotEmpId: Number(this.inputForm.get("pilotL")?.value),
        score: this.inputForm.get("leftProceduralScore")?.value,
        comment: this.inputForm.get("leftProceduralComment")?.value || ""
      });

      observedGradeSummaryFromForm.push({
      category: Categories.ProceduralUseAdherence,
      pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
      score: this.inputForm.get("rightProceduralScore")?.value
      ,comment: this.inputForm.get("rightProceduralComment")?.value || ""
    });
    }

    if (this.inputForm.get("useSituational")?.value) {
      observedGradeSummaryFromForm.push({
        category: Categories.SituationalAwareness,
        pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
        score: this.inputForm.get("leftSituationalScore")?.value
        ,comment: this.inputForm.get("leftSituationalComment")?.value || ""
      });
      observedGradeSummaryFromForm.push({
        category: Categories.SituationalAwareness,
        pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
        score: this.inputForm.get("rightSituationalScore")?.value
        ,comment: this.inputForm.get("rightSituationalComment")?.value || ""
      });
    }

    if (this.inputForm.get("useVerification")?.value) {
        observedGradeSummaryFromForm.push({
          category: Categories.VerficationPractices,
          pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
          score: this.inputForm.get("leftVerificationScore")?.value
        ,comment: this.inputForm.get("leftVerificationComment")?.value || ""
      });
      observedGradeSummaryFromForm.push({
        category: Categories.VerficationPractices,
        pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
        score: this.inputForm.get("rightVerificationScore")?.value
        ,comment: this.inputForm.get("rightVerificationComment")?.value || ""
      });
    }

    if (this.inputForm.get("useBriefing")?.value) {
      observedGradeSummaryFromForm.push({
        category: Categories.Briefing,
        pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
        score: this.inputForm.get("leftBriefingScore")?.value
        ,comment: this.inputForm.get("leftBriefingComment")?.value || ""
      });
      observedGradeSummaryFromForm.push({
        category: Categories.Briefing,
        pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
        score: this.inputForm.get("rightBriefingScore")?.value
        ,comment: this.inputForm.get("rightBriefingComment")?.value || ""
      });
    }

    if (this.inputForm.get("useCommunication")?.value) {



      observedGradeSummaryFromForm.push({
        category: Categories.Communication,
        pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
        score: this.inputForm.get("leftCommunicationScore")?.value,
        comment: this.inputForm.get("leftCommunicationComment")?.value || ""

      });

      observedGradeSummaryFromForm.push({
        category: Categories.Communication,
        pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
        score: this.inputForm.get("rightCommunicationScore")?.value,
        comment: this.inputForm.get("rightCommunicationComment")?.value || ""
      });
    }

    if (this.inputForm.get("useStopWhenUnsure")?.value) {
      observedGradeSummaryFromForm.push({
        category: Categories.StopWhenUnsure,
        pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
        score: this.inputForm.get("leftStopWhenUnsureScore")?.value,
        comment: this.inputForm.get("leftStopWhenUnsureComment")?.value || ""
      });

      observedGradeSummaryFromForm.push({
        category: Categories.StopWhenUnsure,
        pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
        score: this.inputForm.get("rightStopWhenUnsureScore")?.value,
        comment: this.inputForm.get("rightStopWhenUnsureComment")?.value || ""
      });
    }

    if (this.inputForm.get("useSelfCheck")?.value) {
      observedGradeSummaryFromForm.push({
        category: Categories.SelfCheck,
        pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
        score: this.inputForm.get("leftSelfCheckScore")?.value,
        comment: this.inputForm.get("leftSelfCheckComment")?.value || ""
      });

      observedGradeSummaryFromForm.push({
        category: Categories.SelfCheck,
        pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
        score: this.inputForm.get("rightSelfCheckScore")?.value,
        comment: this.inputForm.get("rightSelfCheckComment")?.value || ""
      });
    }

    if (this.inputForm.get("useConservatism")?.value) {
      observedGradeSummaryFromForm.push({
        category: Categories.Conservatism,
        pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
        score: this.inputForm.get("leftConservatismScore")?.value,
        comment: this.inputForm.get("leftConservatismComment")?.value || ""
      });

      observedGradeSummaryFromForm.push({
        category: Categories.Conservatism,
        pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
        score: this.inputForm.get("rightConservatismScore")?.value,
        comment: this.inputForm.get("rightConservatismComment")?.value || ""
      });

    }

    observedGradeSummaryFromForm.push({
      category: Categories.OverallScore,
      pilotEmpId:Number(this.inputForm.get("pilotL")?.value),
      score: this.inputForm.get("leftOverallScore")?.value,
      comment: this.inputForm.get("leftOverallComment")?.value || ""
    });
    observedGradeSummaryFromForm.push({
      category: Categories.OverallScore,
      pilotEmpId:Number(this.inputForm.get("pilotR")?.value),
      score: this.inputForm.get("rightOverallScore")?.value,
      comment: this.inputForm.get("rightOverallComment")?.value || ""
    });
    
    // request payload 
    const requestParameters = {
      observedEventInput: observedEventFromForm,
      observeredGradesInput: observedGradesFromForm,
      observedGradesSummaryInput: observedGradeSummaryFromForm 
    };

    // Call the service method with corrected structure
    this.inputService.inputForm(requestParameters).subscribe({
      next: (response) => this.router.navigate(['/submission']),
      error: (err) => console.error("Submission error:", err),
      });
  }


  toggleSection(section: string, enabled: boolean) {
    // Enable or disable form controls based on the section and enabled state
    

    const sectionKey = section.charAt(0).toUpperCase() + section.slice(1);

    const leftScore = `left${sectionKey}Score`;
    const rightScore = `right${sectionKey}Score`;
    const leftComment = `left${sectionKey}Comment`;
    const rightComment = `right${sectionKey}Comment`;

    if (enabled) {
      this.inputForm.get(leftScore)?.enable();
      this.inputForm.get(rightScore)?.enable();
      this.inputForm.get(leftComment)?.enable();
      this.inputForm.get(rightComment)?.enable();
    } else {
      this.inputForm.get(leftScore)?.disable();
      this.inputForm.get(rightScore)?.disable();
      this.inputForm.get(leftComment)?.disable();
      this.inputForm.get(rightComment)?.disable();
    }
  }

}

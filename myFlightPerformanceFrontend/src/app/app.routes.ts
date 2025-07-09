import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { InputComponent } from './pages/input/input.component';
import { SegGeneratorComponent } from './pages/seg-generator/seg-generator.component';
import { RecordsComponent } from './pages/seg-records/seg-records.component';
import { InputRecordsComponent } from './pages/input-records/input-records.component';
import { PowerbiWrapperComponent } from './pages/powerbi-wrapper/powerbi-wrapper.component';
import { ReportCardComponent } from './pages/report-card/report-card.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'test', component: HomeComponent },

  { path: 'input', component: InputComponent },

  { path: 'input/:SegId', component: InputComponent },

  { path: 'inputseg', component: SegGeneratorComponent},
  { path: 'edit/:SegId', component: SegGeneratorComponent }, // Editing an existing document

  { path: 'records', component: RecordsComponent},

  { path: 'submission', component: InputRecordsComponent},

  { path: 'reportcard', component: ReportCardComponent},

  { path: 'analytics', component: PowerbiWrapperComponent},

  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

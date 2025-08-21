import { Component, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { routes } from '../../app.routes';
import { RecordingService } from '../../services/recording.service';

@Component({
  selector: 'app-tooltip',
  standalone: true, // Ensure it's a standalone component
  templateUrl: './tooltip.component.html',
  styleUrls: ['./tooltip.component.scss'],
  //encapsulation: ViewEncapsulation.None,
  imports: [CommonModule, RouterModule, ButtonModule]
})
export class TooltipComponent {

    constructor(private recordingService: RecordingService) {}
  
  
  items: any[] = [];
  isCollapsed: boolean = false;

  ngOnInit() {
    this.items = [
      {
        label: 'Core Functions',
        items: [
          // { lable: 'Home', route: '/', icon: 'pi pi-home' },
          { label: 'Select Test', route: '/test', icon: 'pi pi-server'},
          { label: 'Form Input', route: '/input', icon: 'pi pi-plus' },
          { label: 'Submissions', route: '/submission', icon: 'pi pi-book' },
          // { label: 'Report Card', route: '/reportcard', icon: 'pi pi-file-pdf' }
        ]
      },
      {
        label: 'Admin Functions',
        items: [
          { label: 'Generate New SEGs', route: '/inputseg', icon: 'pi pi-plus' },
          { label: 'Edit/Delete Existing SEGs', route: '/records', icon: 'pi pi-database' },
          // { label: 'Analytics', route: '/analytics', icon: 'pi pi-chart-line' }
        ]
      },
      // OBS controls are hard corded - Look in html
    ];
  }

  startR() {
    this.recordingService.startRecording();
  }
  stopR() {
    this.recordingService.stopRecording();
  }
  frameR() {
    this.recordingService.frameRecording();
  }



  toggleNavbar() {
    this.isCollapsed = !this.isCollapsed;
  }
}

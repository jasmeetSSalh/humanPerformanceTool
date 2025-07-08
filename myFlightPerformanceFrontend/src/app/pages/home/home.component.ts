import { Component, inject, OnInit } from '@angular/core';
import { SplitterModule } from 'primeng/splitter';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { RecordsService } from '../../services/seg-records.service';
import { SegDocument } from '../../models/segDocument';
import { CommonModule } from '@angular/common';
import { DividerModule } from 'primeng/divider';
import { SelectModule } from 'primeng/select';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: true,
  imports: [ 
    SplitterModule, 
    ButtonModule, 
    CardModule,
    CommonModule, 
    DividerModule, 
    ButtonModule,
    SelectModule
  ]
})
export class HomeComponent implements OnInit {

  segDocumentList: SegDocument[] = [];
  private route = inject(ActivatedRoute);

  constructor(private recordFormService: RecordsService, private router: Router ) {}

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments() 
  {
    // Fetch the a list of documents from api
    this.recordFormService.getAllSegDocuments().subscribe((data) => {
      this.segDocumentList = data; // Update the list
    }, (error) => {
      console.error('Error with loading all the documents into home:', error);
    });
  }

  startSeg(segId: any) : void
  {
    console.log("Current Seg Being Edited: ", +segId);
    this.router.navigate(['/input', +segId]);
  }


}

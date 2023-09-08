import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import Data from '../../../../assets/data/data.json';
import { Pie } from '@antv/g2plot';
import { Router } from '@angular/router';
import { Document } from 'src/app/models/document';

@Component({
  selector: 'app-statistic',
  templateUrl: './statistic.component.html',
  styleUrls: ['./statistic.component.css']
})
export class StatisticComponent implements OnInit , AfterViewInit{
  @Input() docName: string | undefined ;
  documents: Document[]=[] ;
  data: { type: string; value: number }[] = [];
  filter:string="";
  constructor(public activeModal: NgbActiveModal,private router:Router) { }
  @ViewChild('chartContainer') chartContainer!: ElementRef;

  ngOnInit(): void {
  }


  ngAfterViewInit(): void {
    this.documents = JSON.parse(JSON.stringify(Data));
    this.data = this.calculateStatusData(this.documents);
  
    // Delay the chart rendering slightly using setTimeout
    setTimeout(() => {
      const data = this.calculateStatusData(this.documents);
      const piePlot = new Pie(this.chartContainer.nativeElement, {
        data,
        angleField: 'value',
        colorField: 'type',
      });
  
      piePlot.render();
    }, 0);
  }
  


    /*PIE DATA*/

  calculateStatusData(documents: Document[]): { type: string; value: number }[] {
    const statusTotals = documents.reduce((totals, doc) => {
      const status = doc.Status;
      totals[status] = (totals[status] || 0) + 1;
      return totals;
    }, {} as { [status: string]: number });
  
    return Object.keys(statusTotals).map(status => ({
      type: status,
      value: statusTotals[status]
    }));
  }



  filterByTag(tagName: string) {
      this.router.navigateByUrl(`list/${tagName}`);
  }
}

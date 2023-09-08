import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import DataForm from '../../../../assets/data/data.json';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LanguageModalComponent } from '../../partials/language-modal/language-modal.component';
import { StatisticComponent } from '../../partials/statistic/statistic.component';

@Component({
  selector: 'app-list-docs',
  templateUrl: './list-docs.component.html',
  styleUrls: ['./list-docs.component.css']
})
export class ListDocsComponent implements OnInit {

  @ViewChild('htmlData') htmlData!: ElementRef;

  p:number=1;
  itemsPerPage:number=6;
  Data = DataForm ;
  searchDoc='';
  formModal:any;

  Status=[{"type":"Approved","value":false},{"type":"Rejected","value":false},{"type":"New","value":false}];

  constructor(private route : Router , private modalService: NgbModal,activatedRouter:ActivatedRoute) {
    activatedRouter.params.subscribe((params) => {
      if (params['tag']) {
        this.Data = DataForm.filter(d => d.Status?.includes(params['tag']));
      } else {
        this.Data = DataForm; 
      }
    });
    }

  ngOnInit(): void {
  }

  openWindow(){
    this.route.navigate(['languages'])
  }

  openModal(docName: string ) {
    const modalRef = this.modalService.open(LanguageModalComponent);
    modalRef.componentInstance.docName = docName;
  }

  openModalStatistic( ) {
    const modalRef = this.modalService.open(StatisticComponent);
  }

  filterByTag(tagName: string) {
    this.Status.forEach(status => {
      if (status.type === tagName) {
        status.value = true;
      } else {
        status.value = false;
      }
    })
      this.route.navigateByUrl(`list/${tagName}`);
      
    }

  getStatus(Name: string): string {
  switch (Name) {
    case 'Approved':
      return 'fa fa-check';
    case 'Rejected':
      return 'fa fa-times';
    case 'New':
      return 'fa fa-star';
    default:
      return ''; 
  }
}


}

import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-language-modal',
  templateUrl: './language-modal.component.html',
  styleUrls: ['./language-modal.component.css']
})
export class LanguageModalComponent implements OnInit {
  @Input() docName: string | undefined ;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit(): void {
  }




}

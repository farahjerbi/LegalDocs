import { Component, OnInit } from '@angular/core';
import { Template } from 'src/app/models/template.model';
import { TemplateService } from 'src/app/services/template.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {

  constructor(private serviceTemplate:TemplateService) { }

  ngOnInit(): void {
  
  }

}

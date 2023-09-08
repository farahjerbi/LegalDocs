import { Component, OnInit } from '@angular/core';
import { Template } from 'src/app/models/template.model';
import { TemplateService } from 'src/app/services/template.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-legal-docs',
  templateUrl: './my-legal-docs.component.html',
  styleUrls: ['./my-legal-docs.component.css']
})
export class MyLegalDocsComponent implements OnInit {
  myTemplates:Template[]=[];
  userId="CBCD060A-470F-43F6-87E1-5F07903FF179";
  pdfUrls:string[]=[];

  constructor(private serviceTemplate:TemplateService , private router:Router) { }

  ngOnInit(): void {
    this.serviceTemplate.getTemplatesByUser(this.userId).subscribe({
      next: (templates) => {
        this.myTemplates=templates;
        console.log(this.myTemplates);
      },
      error: (response) => {
        console.log(response);
      }
      })
  }


  PrintPdf(templateId:any){
    this.serviceTemplate.generatePdf(this.userId,templateId).subscribe(res=>{
      let blob:Blob=res.body as Blob;
      let url=window.URL.createObjectURL(blob);
      window.open(url);
    })
  }


  DownloadPdf(templateId:any){
    this.serviceTemplate.generatePdf(this.userId,templateId).subscribe(res=>{
      let blob:Blob=res.body as Blob;
      let url=window.URL.createObjectURL(blob);
  
      let a =document.createElement('a');
      a.download="Template";
      a.href=url;
      a.click();
    })
  }

  DeleteTemplate(templateId:any){
    this.serviceTemplate.deleteTemplateUser(this.userId,templateId).subscribe({
      next: (res) => {
      },
      error: (response) => {
        console.log(response);
      }
      })
      alert('Deleted');
      this.router.navigate(['myLegalDocs']);

  }

  update(templateId:any){
    this.router.navigate([`updateTemplate/${templateId}`]);
  }

  
  
  
}

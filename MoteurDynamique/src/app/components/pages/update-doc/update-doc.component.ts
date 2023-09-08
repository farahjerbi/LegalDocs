import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Alias } from 'src/app/models/alias.model';
import { Group } from 'src/app/models/group.model';
import { Template } from 'src/app/models/template.model';
import { TemplateService } from 'src/app/services/template.service';

@Component({
  selector: 'app-update-doc',
  templateUrl: './update-doc.component.html',
  styleUrls: ['./update-doc.component.css']
})
export class UpdateDocComponent implements OnInit {
  templateContent?: Template ;
  templateId:string='';
  templateForm: FormGroup | undefined ;
  aliasData: Alias[] = [];
  groupsData: Group[] = [];
  pdfUrl="";
  isFileUploading = false;
  formData!: FormData;
  aliasId:string='';
  userId="CBCD060A-470F-43F6-87E1-5F07903FF179";
  constructor(activatedRouter:ActivatedRoute,private templateService: TemplateService,private fb: FormBuilder , private router:Router) 
  {
    activatedRouter.params.subscribe(params => {
      if (params['templateId']) {
        this.templateId = params['templateId']; 
      }
    });
   }

  ngOnInit(): void {
    this.loadTemplate(this.templateId);
  }


  loadTemplate(templateId: string) {
    this.templateService.getTemplateByUser(this.userId,templateId)
      .subscribe(content => {
        this.templateContent = content;
        console.log("🚀 ~ file: update-doc.component.ts:43 ~ UpdateDocComponent ~ loadTemplate ~ content:", content)
        this.processTemplateContent(content);
        this.templateForm = this.createTemplateForm();
        this.PreviewPdf(this.templateId);
  
      });}

      processTemplateContent(content: Template) {
        if (content && content.groups) {
          content.groups.forEach(group => {
            const { groupId } = group;
      
            if (group.aliases && group.aliases.length > 0) {
              const aliasesWithIds: Alias[] = group.aliases.map(alias => ({
                ...alias,
                groupId,
                templateId: content.id
              }));
      
              this.aliasData = this.aliasData.concat(aliasesWithIds);
            }
          });
      
          console.log("aliases",this.aliasData);
        }
      }


  /*FORM*/
  
createTemplateForm(): FormGroup {
  const formGroup: { [key: string]: FormControl } = {};

  this.aliasData.forEach(alias => {
    formGroup[alias.id] = new FormControl(alias.typeSetting.defaultValue);
  });

  return this.fb.group(formGroup);
}

logFormValues() {

  const aliasValues = this.templateForm?.value;

  const aliasRequests = Object.keys(aliasValues).map(aliasId => ({
    id: aliasId,
    defaultValue: String(aliasValues[aliasId]),
  }));


  this.templateService.updateTemplate(aliasRequests,this.userId,this.templateId).subscribe(
    response => {
      console.log('Aliases assigned successfully:', response);
      this.templateService.uploadFileToServer(this.formData,this.aliasId,this.userId).subscribe(
        response => {
          console.log('Upload successfully:', response);
        },
        error => {
          console.error('Error Upload:', error);
        }
      );
    },
    error => {
      console.error('Error assigning aliases:', error);
    }
  );
  alert(' Updated successfully');
  this.router.navigateByUrl('myLegalDocs');

}


getFile(event: Event,aliasId:any){
  const inputElement = event.target as HTMLInputElement;
  const file = inputElement.files?.[0];

  if (file) {
    this.formData = new FormData();
    this.formData.append('imageFile', file);
   }
   this.aliasId=aliasId;
}



/*PDF*/


PreviewPdf(templateId:any){
  this.templateService.generatePdf(this.userId,templateId).subscribe(res=>{
    let blob:Blob=res.body as Blob;
    let url=window.URL.createObjectURL(blob);
    this.pdfUrl=url;
  })
}


goBack(){
  this.router.navigateByUrl('myLegalDocs');
}



  }

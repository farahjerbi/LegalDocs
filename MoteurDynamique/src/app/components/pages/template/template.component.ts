import { Component, OnInit } from '@angular/core';
import { TemplateService } from 'src/app/services/template.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Template } from 'src/app/models/template.model';
import { Group } from 'src/app/models/group.model';
import { Alias } from 'src/app/models/alias.model';


@Component({
  selector: 'app-template',
  templateUrl: './template.component.html',
  styleUrls: ['./template.component.css']
})
export class TemplateComponent implements OnInit {
  templateContent?: Template ;
  templateId:string='';
  templateForm: FormGroup | undefined 
  aliasData: Alias[] = [];
  groupsData: Group[] = [];
  pdfUrl="";
  isFileUploading = false;
  formData!: FormData;
  aliasId:string='';
  userId="CBCD060A-470F-43F6-87E1-5F07903FF179";
  language:string=''
  isLoading = true;

  constructor(activatedRouter:ActivatedRoute,private templateService: TemplateService,private fb: FormBuilder , private router:Router) 
  { 
    activatedRouter.params.subscribe(params => {
      if (params['templateId']) {
        this.templateId = params['templateId']; 
      }
      if(params['lang']=="en"){
        this.language = "english"; 
      }
      if(params['lang']=="fr"){
        this.language = "frensh"; 
      }
    });
  }

  ngOnInit(): void {
    this.loadTemplate(this.templateId);
  }



loadTemplate(templateId: string) {
  if(this.language==="frensh"){
    this.templateService.getTemplateFileById(templateId)
    .subscribe(content => {
      this.templateService.translateTemplate(content,this.language).subscribe(result=>{
        this.templateContent = result;
        this.processTemplateContent(result);
        this.templateForm = this.createTemplateForm();
        this.PreviewPdf(result);
        this.isLoading = false; 
      })
    });
  }
  if(this.language=== "english"){
    this.templateService.getTemplateFileById(templateId)
    .subscribe(content => {
      this.templateContent = content;
      this.processTemplateContent(content);
      this.templateForm = this.createTemplateForm();
      this.PreviewPdf(content);
      this.isLoading = false; 
    });
  }

}


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

    console.log(this.aliasData);
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


  this.templateService.assignAliases(aliasRequests,this.userId,this.templateId).subscribe(
    response => {
      console.log('Aliases assigned successfully:', response);
      this.templateService.uploadFileToServer(this.formData,this.aliasId,this.userId).subscribe(
        response => {
          console.log('Upload successfully:', response);
        },
        error => {
          console.error('Error Upload:', error);
        }
      );;
      this.templateForm?.reset();
      alert('Registered Document successfully');
      this.router.navigateByUrl('');
      
    },
    error => {
      console.error('Error assigning aliases:', error);
    }
  );

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


PreviewPdf(template:Template){
  this.templateService.generatePdfTemplate(template).subscribe(res=>{
    let blob:Blob=res.body as Blob;
    let url=window.URL.createObjectURL(blob);
    this.pdfUrl=url;
  })
}


goBack(){
  this.router.navigateByUrl('');
}





}
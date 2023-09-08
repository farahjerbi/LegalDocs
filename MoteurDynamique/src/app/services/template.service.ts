import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Alias } from '../models/alias.model';
import { Template } from '../models/template.model';

@Injectable({
  providedIn: 'root'
})
export class TemplateService {
  
  baseApiUrl : string = environment.baseApiUrl;

  constructor(private http: HttpClient) { }


  getTemplateFileById(templateName: string): Observable<any> {
    const folderPath = 'assets/templateFiles/';
    const fileExtension = '.json';

    return this.http.get<any>(`${folderPath}${templateName}${fileExtension}`)
      .pipe(
        map(response => response),
        catchError(error => {
          console.error('Error loading template file:', error);
          return [];
        })
      );
  }

  assignAliases(aliasRequests: any[], userId: any, templateId: any): Observable<any> {
    return this.http.post<any>(this.baseApiUrl + `Template/AssignAliases/${userId}?templateId=${templateId}`, aliasRequests);
  }

  generatePdf(userId: any, templateId: any){
    return this.http.get(this.baseApiUrl + `Template/generatePdf/${userId}/${templateId}`,{observe:'response',responseType:'blob'});
  }

  generatePdfTemplate(template: Template){
    return this.http.post(this.baseApiUrl + `Template/generatePdfTemplate`,template,{observe:'response',responseType:'blob'});
  }

  getTemplatesByUser(userId: any):Observable<Template[]>{
    return this.http.get<Template[]>(this.baseApiUrl + `Template/getTemplatesByUser/${userId}`);
  }

  getTemplateByUser(userId: string,templateId:string):Observable<Template>{
    return this.http.get<Template>(this.baseApiUrl + `Template/getTemplateByUser/${userId}/${templateId}`);
  }

  uploadFileToServer(formData: any,aliasId:string,userId:string): Observable<any> {
  
    return this.http.post(this.baseApiUrl + `Template/upload/${aliasId}/${userId}`, formData);
  }
  
  uploadFile(formData: any): Observable<any> {
  
    return this.http.post(this.baseApiUrl + `Template/upload`, formData);
  }
  

  deleteTemplateUser(userId:string,templateId:string): Observable<any> {
  
    return this.http.delete(this.baseApiUrl + `Template/delete/${userId}/${templateId}`);
  }

  updateTemplate(aliasRequests: any[], userId: any, templateId: any): Observable<any>{
    return this.http.put<any>(this.baseApiUrl + `Template/update/${userId}/${templateId}`, aliasRequests);

  }

  translateTemplate(template: Template, language: string): Observable<Template> {
    return this.http.post<Template>(`${this.baseApiUrl}Template/translateTemplate?language=${language}`, template);
  }

  
}



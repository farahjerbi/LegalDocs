import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Template } from '../models/template.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LegalDocService {
  baseApiUrl : string = environment.baseApiUrl;

  constructor(private http : HttpClient) { }

  addTemplate(addTemplate:Template): Observable<Template>{
    return this.http.post<Template>(this.baseApiUrl + 'Template/addTemplate' , addTemplate);
  }


}

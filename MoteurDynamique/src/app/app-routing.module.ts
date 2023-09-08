import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListDocsComponent } from './components/pages/list-docs/list-docs.component';
import { TemplateComponent } from './components/pages/template/template.component';
import { MyLegalDocsComponent } from './components/pages/my-legal-docs/my-legal-docs.component';
import { UpdateDocComponent } from './components/pages/update-doc/update-doc.component';

const routes: Routes = [
  {path:'', component:ListDocsComponent},
  {path:'myLegalDocs',component:MyLegalDocsComponent},
  {path:'list/:tag',component:ListDocsComponent},
  {path:'template/:lang/:templateId',component:TemplateComponent },
  {path:'updateTemplate/:templateId',component:UpdateDocComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }

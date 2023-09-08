import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {NgxPaginationModule} from 'ngx-pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SearchPipe } from './pipes/search.pipe';
import { LanguageModalComponent } from './components/partials/language-modal/language-modal.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {  HttpClientModule } from '@angular/common/http';
import { ListDocsComponent } from './components/pages/list-docs/list-docs.component';
import { NavbarComponent } from './components/partials/navbar/navbar.component';
import { SideBarComponent } from './components/partials/side-bar/side-bar.component';
import { TemplateComponent } from './components/pages/template/template.component';
import {NgxExtendedPdfViewerModule} from 'ngx-extended-pdf-viewer'
import {CloudinaryModule} from '@cloudinary/ng';
import { MyLegalDocsComponent } from './components/pages/my-legal-docs/my-legal-docs.component';
import { PdfViewerComponent } from './components/partials/pdf-viewer/pdf-viewer.component';
import { UpdateDocComponent } from './components/pages/update-doc/update-doc.component';
import { StatisticComponent } from './components/partials/statistic/statistic.component';

@NgModule({
  declarations: [
    AppComponent,
    ListDocsComponent,
    SearchPipe,
    LanguageModalComponent,
    NavbarComponent,
    SideBarComponent,
    TemplateComponent,
    MyLegalDocsComponent,
    PdfViewerComponent,
    UpdateDocComponent,
    StatisticComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgxPaginationModule,
    FormsModule,
    NgbModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxExtendedPdfViewerModule,
    CloudinaryModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyLegalDocsComponent } from './my-legal-docs.component';

describe('MyLegalDocsComponent', () => {
  let component: MyLegalDocsComponent;
  let fixture: ComponentFixture<MyLegalDocsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyLegalDocsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyLegalDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

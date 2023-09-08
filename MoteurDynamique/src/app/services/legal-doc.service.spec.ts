import { TestBed } from '@angular/core/testing';

import { LegalDocService } from './legal-doc.service';

describe('LegalDocService', () => {
  let service: LegalDocService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LegalDocService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

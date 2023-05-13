import { TestBed } from '@angular/core/testing';

import { ConfigurationLoaderService } from './configuration-loader.service';

describe('ConfigurationLoaderService', () => {
  let service: ConfigurationLoaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConfigurationLoaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

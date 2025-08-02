import { TestBed } from '@angular/core/testing';

import { DungeonSimulatorService } from './dungeon-simulator.service';

describe('DungeonSimulatorService', () => {
  let service: DungeonSimulatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DungeonSimulatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

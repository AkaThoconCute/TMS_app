import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivateLayout } from './private-layout';

describe('PrivateLayout', () => {
  let component: PrivateLayout;
  let fixture: ComponentFixture<PrivateLayout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PrivateLayout],
    }).compileComponents();

    fixture = TestBed.createComponent(PrivateLayout);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

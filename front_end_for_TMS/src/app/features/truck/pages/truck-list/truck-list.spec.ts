import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { TruckListPage } from './truck-list/truck-list';

describe('TruckListPage', () => {
  let component: TruckListPage;
  let fixture: ComponentFixture<TruckListPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TruckListPage],
      providers: [provideHttpClient()],
    }).compileComponents();

    fixture = TestBed.createComponent(TruckListPage);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

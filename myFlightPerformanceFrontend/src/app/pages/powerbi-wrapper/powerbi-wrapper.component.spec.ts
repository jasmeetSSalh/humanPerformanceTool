import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PowerbiWrapperComponent } from './powerbi-wrapper.component';

describe('PowerbiWrapperComponent', () => {
  let component: PowerbiWrapperComponent;
  let fixture: ComponentFixture<PowerbiWrapperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PowerbiWrapperComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PowerbiWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

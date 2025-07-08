import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';

bootstrapApplication(AppComponent, {
  ...appConfig, // Spread existing appConfig properties
  providers: [...(appConfig.providers || []), provideHttpClient()] // Ensure HttpClient is provided
}).catch((err) => console.error(err));

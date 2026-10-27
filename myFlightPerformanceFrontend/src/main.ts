import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/services/auth.interceptor';

bootstrapApplication(AppComponent, {
  ...appConfig, // Spread existing appConfig properties
  providers: [...(appConfig.providers || []), provideHttpClient(withInterceptors([authInterceptor]))] // Ensure HttpClient is provided
}).catch((err) => console.error(err));

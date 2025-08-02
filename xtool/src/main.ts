import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));

// Overwrite fetch so that we can capture the response body and URL for game requests.
// TODO: We might optimise this to only capture requests to the game server, rather than all fetch requests.
const {fetch: origFetch} = window;
window.fetch = async (...args) => {
  const response = await origFetch(...args);
  response
      .clone()
      .text()
      .then(body => {
        document.dispatchEvent(new CustomEvent("SFCommand", { detail: { body: body, url: response.url } }));
      })
      .catch(err => console.error(err));

  return response;
};

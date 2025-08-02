import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { SFGameRequest } from './app/sfgame/SFGameModels';

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));

const { fetch: origFetch } = window;
window.fetch = async (...args) => {
  const response = await origFetch(...args);
  if (!args || (args[0] as any)?.method === 'OPTIONS' || args[1]?.method === 'OPTIONS') {
    return response;
  }
  if (!response.ok) {
    return response;
  }

  const url = response.url;
  const urlParams = new URLSearchParams(url.split('?')[1] || '');
  const req = urlParams.get('req');
  if (!req) {
    return response;
  }
  let params = urlParams.get('params');

  if (params) {
    params = atob(params);
  }

  const body = await response.clone().text();

  const dictionary: { [key: string]: string } = {};
  const data = body.split('&').reduce((model, item) => {
    const [key, ...val] = item.split(':');

    const normalizedKey = key.replace(/\(\d+?\)| /g, '').split('.')[0].toLowerCase();
    if (normalizedKey === 'otherplayerachievement' && val[0].startsWith('achievement')) {
      val.splice(0, 1);
    }

    model[normalizedKey] = val.join(':');

    return model;
  }, dictionary);

  const gameRequest: SFGameRequest = {
    req: req,
    params: params,
    data: data
  };

  document.dispatchEvent(new CustomEvent("SFCommand", {
    bubbles: true, composed: true, detail: gameRequest
  }));

  return response;
};

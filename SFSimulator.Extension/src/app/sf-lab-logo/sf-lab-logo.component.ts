import { Component } from '@angular/core';

@Component({
  imports: [],
  selector: 'sf-lab-logo',
  template: `
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100" width="32" height="32">
  <g   transform="translate(50 50) scale(0.9) translate(-50 -50)">
  <defs>
    <linearGradient id="liquidGrad" x1="0" y1="1" x2="1" y2="0">
      <stop offset="0%" stop-color="#FF5E1E" />
      <stop offset="45%" stop-color="#FF9F1C" />
      <stop offset="65%" stop-color="#2EC4B6" />
      <stop offset="100%" stop-color="#05A8AA" />
    </linearGradient>
  </defs>
  <path d="M 33 58 L 22.5 76 C 20 80.5 23 86 28 86 L 72 86 C 77 86 80 80.5 77.5 76 L 67 58 C 61 61.5 39 54 33 58 Z" fill="url(#liquidGrad)" />
  <circle cx="58" cy="71" r="2.5" fill="#FFFFFF" opacity="0.6" />
  <circle cx="42" cy="77" r="1.5" fill="#FFFFFF" opacity="0.6" />
  <circle cx="48" cy="65" r="1" fill="#FFFFFF" opacity="0.6" />
  <path d="M 41 12 L 59 12 M 45 12 L 45 32 L 18.5 76 C 15.5 81.2 19.2 88 25.2 88 L 74.8 88 C 80.8 88 84.5 81.2 81.5 76 L 55 32 L 55 12"
    fill="none" stroke="#8a8986" stroke-width="5" stroke-linecap="round" stroke-linejoin="round" />
  <path d="M 34 58 C 40 54 60 62 66 58" fill="none" stroke="#8a8986" stroke-width="3.5" stroke-linecap="round" />
  <path d="M 27.5 76 L 47 43" fill="none" stroke="#FFFFFF" stroke-width="2.5" stroke-linecap="round" opacity="0.7" />
  </g>
</svg>
`,
})
export class SfLabLogo {
}

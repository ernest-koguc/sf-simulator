import { OVERLAY_DEFAULT_CONFIG } from '@angular/cdk/overlay';
import { type EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';

export function provideSpartanHlm(): EnvironmentProviders {
	return makeEnvironmentProviders([
		{
			provide: OVERLAY_DEFAULT_CONFIG,
			useValue: { usePopover: false },
		},
	]);
}

import {
  Observable,
  OperatorFunction,
} from 'rxjs';

export function debounceWithMaxWait<T>(
  delay: number,
  maxWait: number
): OperatorFunction<T, T> {
  return source => new Observable<T>(subscriber => {
    let latest: T;
    let pending = false;
    let debounceTimer: ReturnType<typeof setTimeout> | undefined;
    let maxTimer: ReturnType<typeof setTimeout> | undefined;

    const clearTimers = () => {
      clearTimeout(debounceTimer);
      clearTimeout(maxTimer);
      debounceTimer = undefined;
      maxTimer = undefined;
    };

    const emit = () => {
      if (!pending) return;

      clearTimers();
      pending = false;
      subscriber.next(latest);
    };

    const subscription = source.subscribe({
      next: value => {
        latest = value;

        if (!pending) {
          pending = true;
          maxTimer = setTimeout(emit, maxWait);
        }

        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(emit, delay);
      },
      error: error => subscriber.error(error),
      complete: () => {
        if (pending) emit();
        subscriber.complete();
      }
    });

    return () => {
      clearTimers();
      subscription.unsubscribe();
    };
  });
}

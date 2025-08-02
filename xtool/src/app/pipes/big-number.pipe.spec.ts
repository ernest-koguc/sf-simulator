import { BigNumberPipe } from './big-number.pipe';

describe('BigNumberPipe', () => {
  it('create an instance', () => {
    const pipe = new BigNumberPipe();
    expect(pipe).toBeTruthy();
  });
});

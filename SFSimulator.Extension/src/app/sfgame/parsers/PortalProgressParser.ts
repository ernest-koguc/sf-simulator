import { PortalProgress } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parsePortalProgress(data: number[], serverTime: number): PortalProgress {
  const byteParser = new ByteParser(data);

  const finished = byteParser.long();
  const hpPercentage = byteParser.long();
  const dayOfYearOfLastFight = byteParser.long();
  const serverDayOfYear = getDayOfYear(new Date(serverTime));
  const canAttack = dayOfYearOfLastFight < serverDayOfYear || serverDayOfYear === 1 && dayOfYearOfLastFight >= 365;

  const portalProgress = {
    Finished: finished,
    HpPercentage: hpPercentage,
    CanAttack: canAttack,
  };

  return portalProgress;
}

function getDayOfYear(date: Date): number {
  const start = new Date(date.getFullYear(), 0, 0).getTime();
  const diff = date.getTime() - start;
  const oneDay = 1000 * 60 * 60 * 24;
  return Math.floor(diff / oneDay);
}

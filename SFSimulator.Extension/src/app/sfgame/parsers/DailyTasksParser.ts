import { DailyTask, DailyTaskReward } from '../SFGameModels';
import { ByteParser, chunks, unsigned } from './ByteParser';

// parse_rewards / RewardChest::parse. Reward pairs only determine the stride.
export function parseDailyTaskRewards(data: number[]): DailyTaskReward[] {
  const parser = new ByteParser(data);
  const rewards: DailyTaskReward[] = [];
  for (let i = 0; i < 3; i++) {
    if (parser.remaining < 3) {
      // Rust defaults a chest whose header/payload cannot be parsed.
      rewards.push({ Claimed: false, PointsRequired: 0 });
      continue;
    }
    const claimed = parser.read() !== 0;
    const pointsRequired = parser.read();
    const rewardCount = parser.read();
    if (rewardCount < 0 || rewardCount * 2 > parser.remaining || pointsRequired < 0 || pointsRequired > 0xffffffff) {
      while (rewards.length < 3) {
        rewards.push({ Claimed: false, PointsRequired: 0 });
      }
      break;
    }
    parser.skip(rewardCount * 2);
    rewards.push({ Claimed: claimed, PointsRequired: pointsRequired });
  }
  return rewards;
}

// gamestate/mod.rs: dailytasklist skips one word, then reads complete 4-word tasks.
export function parseDailyTaskList(data: number[]): DailyTask[] {
  return Array.from(chunks(data.slice(1), 4), task => ({
    Completed: unsigned(task[1], Number.MAX_VALUE) >= unsigned(task[2], Number.MAX_VALUE, Number.MAX_VALUE),
    Points: unsigned(task[3]),
  }));
}

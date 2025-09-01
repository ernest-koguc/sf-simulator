import { DailyTask, DailyTaskReward } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parseDailyTaskRewards(data: number[]): DailyTaskReward[] {
  const parser = new ByteParser(data);
  const rewards = [];

  for (let i = 0; i < 3; i++) {
    const claimed = parser.long() === 1;
    const pointsRequired = parser.long();
    const rewardCount = parser.long();
    const taskReward = [];
    for (let j = 0; j < rewardCount; j++) {
      const resourceType = parser.long();
      const amount = parser.long();
      taskReward.push({ ResourceType: resourceType, Amount: amount });
    }
    rewards.push({ Claimed: claimed, PointsRequired: pointsRequired, Rewards: taskReward });
  }

  return rewards;
}

export function parseDailyTaskList(data: number[]): DailyTask[] {
  const parser = new ByteParser(data);
  parser.skip(1);
  const taskCount = data.length - 1;
  const tasks = [];

  for (let i = 0; i < taskCount; i++) {
    const taskType = parser.long();
    const current = parser.long();
    const target = parser.long();
    const points = parser.long();
    const completed = current >= target;
    tasks.push({ TaskType: taskType, Current: current, Target: target, Points: points, Completed: completed });
  }

  return tasks;
}

export interface BuffetWaste {
  id?: string;
  itemName: string;
  dishName?: string;
  trayCapacityKg?: number;
  remainingPercentage: number;
  wastedWeightKg: number;
  estimatedLossUsd: number;
  timestamp?: Date | string;
}

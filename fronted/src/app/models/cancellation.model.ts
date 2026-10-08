export interface CancellationRequest {
  bookingName: string;
  canceledCovers: number;
  cancellationRate: number;
  estimatedWasteCost: number;
}

export interface CanceledOrderRecord {
  id: string;
  bookingName: string;
  canceledCovers: number;
  cancellationRate: number;
  estimatedWasteCost: number;
  timestamp: string;
}

export interface CancellationResponse {
  message: string;
  count: number;
  records: CanceledOrderRecord[];
}

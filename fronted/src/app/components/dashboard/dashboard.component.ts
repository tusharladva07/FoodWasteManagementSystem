import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { HospitalityService } from '../../services/hospitality.service';
import { BuffetWaste } from '../../models/buffet-waste.model';
import { Forecast } from '../../models/forecast.model';
import { CancellationRequest } from '../../models/cancellation.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  // Top KPI Metrics (Updated directly from API responses)
  totalWasteKg: number = 0;
  cumulativeLostRevenueUsd: number = 0;
  tomorrowRecommendedPrepCount: number = 0;

  // Left Card: Post-Service Buffet Audit
  selectedFile: File | null = null;
  imagePreviewUrl: string | null = null;
  isAnalyzing: boolean = false;
  auditResult: BuffetWaste | null = null;
  auditErrorMessage: string | null = null;

  // Right Card: Morning Prep & Shortage Preventer
  expectedGuests: number | null = 150;
  isCalculating: boolean = false;
  forecastResult: Forecast | null = null;
  prepErrorMessage: string | null = null;

  // Booking / Order Cancellation Ingestion
  showCancellationForm: boolean = false;
  bookingName: string = '';
  canceledCovers: number | null = 25;
  cancellationRatePercent: number | null = 15;
  estimatedWasteCost: number | null = 150;
  isSubmittingCancellation: boolean = false;
  cancellationSuccessMessage: string | null = null;
  cancellationErrorMessage: string | null = null;

  // Epic 2, User Story 2.2: Untouched Food Re-Routing Alert
  rerouteCandidateCovers: number = 30; // Pre-loaded from baseline cancellations
  isRerouted: boolean = false;
  rerouteSuccessMessage: string | null = null;

  constructor(private hospitalityService: HospitalityService) {}

  ngOnInit(): void {
    // Initial fetch of dashboard metrics and forecast data
    this.refreshAllData();
  }

  /**
   * Refreshes all dashboard metrics, forecast calculations,
   * and cumulative totals from the backend whenever any POST event occurs.
   */
  refreshAllData(): void {
    const covers = (this.expectedGuests && this.expectedGuests > 0) ? Number(this.expectedGuests) : 150;
    this.hospitalityService.getForecast(covers).subscribe({
      next: (forecast: Forecast) => {
        this.applyForecastResponse(forecast);
      },
      error: () => {
        // Silently preserve current metrics if backend is offline
      }
    });
  }

  // Epic 1, User Story 1.2: High-Cost Dish Alert (>20% waste)
  isHighCostAlert(record: BuffetWaste | null): boolean {
    if (!record) return false;
    return record.remainingPercentage > 20;
  }

  // Epic 2, User Story 2.2: Mark untouched unserved food as re-routed to staff cafeteria / recovery
  markAsRerouted(): void {
    this.isRerouted = true;
    this.rerouteSuccessMessage = `Successfully flagged ${this.rerouteCandidateCovers} unserved portions for safe diversion to the Staff Cafeteria / Food Recovery Network.`;
  }

  // 1. File selection stores the selected file
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.auditErrorMessage = null;

      // Generate local preview of the selected image
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreviewUrl = reader.result as string;
      };
      reader.readAsDataURL(this.selectedFile);
    } else {
      this.selectedFile = null;
      this.imagePreviewUrl = null;
    }
  }

  // 2. Analyze button calls analyzeTray()
  analyzeBuffet(): void {
    this.analyzeTray();
  }

  analyzeTray(): void {
    // Handle missing file
    if (!this.selectedFile) {
      this.auditErrorMessage = 'Please select a buffet tray image file before analyzing.';
      return;
    }

    this.isAnalyzing = true;
    this.auditErrorMessage = null;

    // POST /api/vision/analyze
    this.hospitalityService.analyzeTray(this.selectedFile).subscribe({
      next: (result: BuffetWaste) => {
        this.auditResult = result;
        this.isAnalyzing = false;

        // Auto-refresh all data from backend following POST event
        this.refreshAllData();
      },
      error: (error: HttpErrorResponse | Error) => {
        this.isAnalyzing = false;
        this.auditErrorMessage = this.extractErrorMessage(error, 'Failed to analyze tray image. Please check API connection and try again.');
      }
    });
  }

  // 3. Expected guests input binds to number & Calculate button calls getForecast()
  calculateForecast(): void {
    if (this.expectedGuests !== null && this.expectedGuests !== undefined) {
      this.getForecast(Number(this.expectedGuests));
    } else {
      this.getForecast(0);
    }
  }

  getForecast(expectedCovers: number): void {
    // Handle invalid expected guest count
    if (!expectedCovers || expectedCovers <= 0 || isNaN(expectedCovers)) {
      this.prepErrorMessage = 'Please enter a valid number of expected hotel guests (> 0).';
      return;
    }

    this.isCalculating = true;
    this.prepErrorMessage = null;

    this.hospitalityService.getForecast(expectedCovers).subscribe({
      next: (forecast: Forecast) => {
        this.applyForecastResponse(forecast);
        this.isCalculating = false;
      },
      error: (error: HttpErrorResponse | Error) => {
        this.isCalculating = false;
        this.prepErrorMessage = this.extractErrorMessage(error, 'Failed to retrieve forecast. Please check API connection and try again.');
      }
    });
  }

  // 4. Record new cancellation using hospitalityService.postCancellations
  toggleCancellationForm(): void {
    this.showCancellationForm = !this.showCancellationForm;
    this.cancellationSuccessMessage = null;
    this.cancellationErrorMessage = null;
  }

  submitCancellation(): void {
    if (!this.bookingName.trim()) {
      this.cancellationErrorMessage = 'Please enter a booking or event name.';
      return;
    }
    if (this.canceledCovers === null || this.canceledCovers < 0) {
      this.cancellationErrorMessage = 'Please enter valid canceled covers (>= 0).';
      return;
    }
    if (this.cancellationRatePercent === null || this.cancellationRatePercent < 0 || this.cancellationRatePercent > 100) {
      this.cancellationErrorMessage = 'Please enter a cancellation percentage between 0% and 100%.';
      return;
    }
    if (this.estimatedWasteCost === null || this.estimatedWasteCost < 0) {
      this.cancellationErrorMessage = 'Please enter a valid estimated waste cost ($ >= 0).';
      return;
    }

    this.isSubmittingCancellation = true;
    this.cancellationErrorMessage = null;
    this.cancellationSuccessMessage = null;

    const request: CancellationRequest = {
      bookingName: this.bookingName.trim(),
      canceledCovers: Number(this.canceledCovers),
      cancellationRate: Number(this.cancellationRatePercent) / 100,
      estimatedWasteCost: Number(this.estimatedWasteCost)
    };

    // POST /api/orders/cancellations
    this.hospitalityService.postCancellations([request]).subscribe({
      next: () => {
        this.isSubmittingCancellation = false;
        this.cancellationSuccessMessage = `Cancellation for "${request.bookingName}" recorded successfully!`;
        this.bookingName = '';

        // Increment candidate covers available for food re-routing
        this.rerouteCandidateCovers += request.canceledCovers;
        this.isRerouted = false;
        this.rerouteSuccessMessage = null;

        // Auto-refresh all data from backend following POST event
        this.refreshAllData();
      },
      error: (error: HttpErrorResponse | Error) => {
        this.isSubmittingCancellation = false;
        this.cancellationErrorMessage = this.extractErrorMessage(error, 'Failed to record cancellation. Please try again.');
      }
    });
  }

  // Update top metrics and cards from the API response
  private applyForecastResponse(forecast: Forecast): void {
    this.forecastResult = forecast;
    this.totalWasteKg = forecast.totalWasteKg;
    this.cumulativeLostRevenueUsd = forecast.totalCumulativeDollarLoss;
    this.tomorrowRecommendedPrepCount = forecast.recommendedPrepCount;
  }

  // Helper to extract clean error message
  private extractErrorMessage(error: any, fallback: string): string {
    if (error?.error?.message) {
      return error.error.message;
    }
    if (typeof error?.error === 'string' && error.error.trim().length > 0) {
      return error.error;
    }
    if (error?.message) {
      return error.message;
    }
    return fallback;
  }
}

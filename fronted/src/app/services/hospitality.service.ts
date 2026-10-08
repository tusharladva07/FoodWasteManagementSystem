import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BuffetWaste } from '../models/buffet-waste.model';
import { Forecast } from '../models/forecast.model';
import { CancellationRequest, CancellationResponse } from '../models/cancellation.model';

@Injectable({
  providedIn: 'root'
})
export class HospitalityService {
  private readonly baseUrl = '/api';

  constructor(private http: HttpClient) {}

  /**
   * 1. analyzeTray(file: File)
   * - Creates FormData.
   * - Appends the file using the field name "image".
   * - POSTs to /api/vision/analyze.
   */
  analyzeTray(file: File): Observable<BuffetWaste> {
    const formData = new FormData();
    formData.append('image', file, file.name);
    return this.http.post<BuffetWaste>(`${this.baseUrl}/vision/analyze`, formData);
  }

  /**
   * Backward-compatible alias for analyzeTray
   */
  analyzeBuffetImage(file: File): Observable<BuffetWaste> {
    return this.analyzeTray(file);
  }

  /**
   * 2. getForecast(expectedCovers: number)
   * - GET /api/forecast?expectedCovers={value}.
   */
  getForecast(expectedCovers: number): Observable<Forecast> {
    return this.http.get<Forecast>(`${this.baseUrl}/forecast?expectedCovers=${expectedCovers}`);
  }

  /**
   * Records buffet booking cancellations
   * - POST /api/orders/cancellations
   */
  postCancellations(cancellations: CancellationRequest[]): Observable<CancellationResponse> {
    return this.http.post<CancellationResponse>(`${this.baseUrl}/orders/cancellations`, cancellations);
  }
}

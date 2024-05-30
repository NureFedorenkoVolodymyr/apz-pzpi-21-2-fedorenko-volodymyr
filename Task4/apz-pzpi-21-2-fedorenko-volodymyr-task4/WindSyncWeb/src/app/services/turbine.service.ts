import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { TurbineReadViewModel } from '../../assets/models/turbine.read.viewmodel';
import { TurbineDataReadViewModel } from '../../assets/models/turbine.data.read.viewmodel';

@Injectable({
  providedIn: 'root'
})
export class TurbineService {
  private apiUrl = 'https://localhost:7213/api/turbines';
  
  private http = inject(HttpClient);

  getDataHistorical(turbineId: number, start: Date, end: Date){
    let params = new HttpParams()
      .set('start', start.toISOString())
      .set('end', end.toISOString());

      return this.http.get<TurbineDataReadViewModel[]>(`${this.apiUrl}/${turbineId}/data`, { params });
  }
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { TurbineReadViewModel } from '../../assets/models/turbine.read.viewmodel';
import { TurbineDataReadViewModel } from '../../assets/models/turbine.data.read.viewmodel';
import { TurbineAddViewModel } from '../../assets/models/turbine.add.viewmodel';

@Injectable({
  providedIn: 'root'
})
export class TurbineService {
  private apiUrl = 'https://localhost:7213/api/turbines';
  
  private http = inject(HttpClient);

  getAll() {
    return this.http.get<TurbineReadViewModel[]>(`${this.apiUrl}`);
  }

  getById(id: number) {
    return this.http.get<TurbineReadViewModel>(`${this.apiUrl}/${id}`);
  }

  getMy() {
    return this.http.get<TurbineReadViewModel[]>(`${this.apiUrl}/my`);
  }

  add(turbine: TurbineAddViewModel) {
    return this.http.post(this.apiUrl, turbine);
  }

  update(id: number, turbine: TurbineAddViewModel) {
    return this.http.put(`${this.apiUrl}/${id}`, turbine);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getDataHistorical(turbineId: number, start: Date, end: Date){
    let params = new HttpParams()
      .set('start', start.toISOString())
      .set('end', end.toISOString());

      return this.http.get<TurbineDataReadViewModel[]>(`${this.apiUrl}/${turbineId}/data`, { params });
  }
}

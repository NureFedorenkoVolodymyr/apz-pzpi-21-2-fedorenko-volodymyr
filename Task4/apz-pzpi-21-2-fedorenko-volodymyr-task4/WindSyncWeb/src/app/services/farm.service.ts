import { Injectable } from '@angular/core';
import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FarmReadViewModel } from '../../assets/models/farm.read.viewmodel';
import { FarmAddViewModel } from '../../assets/models/farm.add.viewmodel';
import { TurbineReadViewModel } from '../../assets/models/turbine.read.viewmodel';

@Injectable({
  providedIn: 'root'
})
export class FarmService {
  private apiUrl = 'https://localhost:7213/api/windfarms';
  
  private http = inject(HttpClient);

  getAll() {
    return this.http.get<FarmReadViewModel[]>(`${this.apiUrl}`);
  }

  getMy() {
    return this.http.get<FarmReadViewModel[]>(`${this.apiUrl}/my`);
  }

  getById(id: number) {
    return this.http.get<FarmReadViewModel>(`${this.apiUrl}/${id}`);
  }

  getTurbines(id: number) {
    return this.http.get<TurbineReadViewModel[]>(`${this.apiUrl}/${id}/turbines`);
  }

  add(farm: FarmAddViewModel) {
    return this.http.post(this.apiUrl, farm);
  }

  update(farm: FarmReadViewModel) {
    return this.http.put(`${this.apiUrl}/${farm.id}`, farm);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}

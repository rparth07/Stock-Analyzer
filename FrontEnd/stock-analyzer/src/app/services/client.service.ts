import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { Client } from '../types/Client';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private http: HttpClient) { }

  fetchAllClients() {
    return this.http.get<Client[]>(DomainConstants.StockAnalyzer_URL + 'all-clients/');
  }
}
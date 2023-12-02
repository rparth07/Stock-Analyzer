import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { Notebook } from '../types/Notebook';

@Injectable({
  providedIn: 'root'
})
export class NotebookService {
  constructor(private http: HttpClient) { }

  getExistingNotebook(notebookDate: Date) {
    const url = `${DomainConstants.Notebook_URL}get-notebook`;
    const params = {
      notebookDate: notebookDate.toISOString()
    };
    return this.http.get<Notebook>(url, { params });
  }

  getAllNotebooks() {
    const url = `${DomainConstants.Notebook_URL}get-notebooks`;
    return this.http.get<Notebook[]>(url);
  }

  updateNotebooks(notebooks: Notebook[]) {
    const url = `${DomainConstants.Notebook_URL}update-all-notebooks`;
    return this.http.post<Notebook[]>(url, notebooks);
  }
}

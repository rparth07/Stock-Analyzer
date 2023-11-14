import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { Notebook } from '../types/Notebook';

@Injectable({
  providedIn: 'root'
})
export class NotebookService {

  constructor(private http: HttpClient) { }

  getExistingNotebook(filterDate: Date) {
    const url = `${DomainConstants.Filter_URL}get-notebook-content`;
    const params = {
      filterDate: filterDate.toISOString()
    };
    return this.http.get<Notebook>(url, { params });
  }

  updateNotebook(notebook: Notebook) {
    const url = `${DomainConstants.Filter_URL}update-notebook-content`;
    const params = {
      notebook: notebook
    };
    this.http.put<Notebook>(url, { params });
  }
}

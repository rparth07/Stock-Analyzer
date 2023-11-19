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
    console.log('notebook date = ' + notebookDate);
    const url = `${DomainConstants.Notebook_URL}get-notebook`;
    const params = {
      notebookDate: notebookDate.toISOString()
    };
    return this.http.get<Notebook>(url, { params });
  }

  updateNotebook(notebook: Notebook) {
    const url = `${DomainConstants.Notebook_URL}update-notebook`;
    return this.http.post<Notebook>(url, notebook);
  }
}

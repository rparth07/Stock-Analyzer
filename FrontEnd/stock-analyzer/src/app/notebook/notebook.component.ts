import { Component, OnInit, ViewChild } from '@angular/core';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../types/Notebook';

@Component({
  selector: 'app-notebook',
  templateUrl: './notebook.component.html',
  styleUrls: ['./notebook.component.css']
})
export class NotebookComponent implements OnInit {

  filterDate: string = new Date().toISOString().split('T')[0];
  openChatBox: Boolean = false;
  existingNotebook: Notebook = {
    contentDate: this.filterDate,
    content: ''
  };

  constructor(private notebookService: NotebookService) {

  }

  ngOnInit(): void {
    this.onNotebookDateChange();
  }

  toggleState() {
    this.openChatBox = !this.openChatBox;
  }

  updateContent(content: string) {
    this.existingNotebook.content = content;
    this.notebookService.updateNotebook(this.existingNotebook)
      .subscribe({
        next: (value) => {
          console.log(value);
        },
        error: (err) => console.log(err),
      });
    this.toggleState();
  }

  onNotebookDateChange() {
    this.notebookService.getExistingNotebook(new Date(this.filterDate))
      .subscribe({
        next: (value: Notebook) => {
          value.contentDate = value.contentDate.split('T')[0];
          this.existingNotebook = value;
          console.dir(this.existingNotebook, { depth: null });
        },
        error: (err) => console.log(err),
      });
  }
}
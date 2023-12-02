import { Component, OnInit, ViewChild } from '@angular/core';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../types/Notebook';

@Component({
  selector: 'app-notebook',
  templateUrl: './notebook.component.html',
  styleUrls: ['./notebook.component.css']
})
export class NotebookComponent implements OnInit {
  openChatBox: Boolean = false;

  notebooks: Notebook[] = [];

  constructor(private notebookService: NotebookService) {
  }

  ngOnInit(): void {
    this.getAllNotebooks();
  }

  getAllNotebooks() {
    this.notebookService.getAllNotebooks()
      .subscribe({
        next: (value: Notebook[]) => {
          // console.log("Value received", value);
          this.notebooks = value.sort((a, b) => {
            if (this.toDate(a.contentDate) < this.toDate(b.contentDate)) {
              return 1;
            }
            return -1;
          });
          this.addTodayNotebookIfMissing();
        },
        error: (err) => console.log(err),
      });
  }

  toDate(contentDate: string) {
    return new Date(contentDate);
  }

  formatDate(date: string): string {
    // Implement your date formatting logic here
    // Example: Convert '2023-12-02T12:34:56' to 'Dec 2, 2023'
    //.split('T')[0]
    const formattedDate = new Date(date).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
    });

    return formattedDate;
  }

  toggleState() {
    this.openChatBox = !this.openChatBox;
  }

  updateNoteBooks() {
    // console.log('updated notebooks = ');
    // console.dir(this.notebooks, { depth: null });
    this.notebookService.updateNotebooks(this.notebooks)
      .subscribe({
        next: (value) => {
          // console.log('updated all notebooks successfully! ' + value);
          this.getAllNotebooks();
        },
        error: (err) => console.log(err),
      });
    this.toggleState();
  }

  addTodayNotebookIfMissing() {
    const today = new Date().toISOString().split('T')[0];
    if (this.notebooks.findIndex(_ => this.toDateStr(_.contentDate) == today) == -1) {
      // console.log('added the today notebook');
      this.notebooks.unshift({
        contentDate: today,
        content: ''
      });
    }
  }

  toDateStr(dateStr: string) {
    return dateStr.split('T')[0];
  }
}


import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../types/Notebook';

@Component({
  selector: 'app-notebook',
  templateUrl: './notebook.component.html',
  styleUrls: ['./notebook.component.css']
})
export class NotebookComponent implements OnInit {
  @ViewChild('noteFeedback') noteFeedback?: ElementRef<HTMLElement>;
  focusFeedback() { setTimeout(() => this.noteFeedback?.nativeElement.focus()); }
  openChatBox: Boolean = false;

  notebooks: Notebook[] = [];
  loaded = false;
  saving = false;
  loading = false;
  message = '';
  loadError = false;
  dirty = false;

  constructor(private notebookService: NotebookService) {
  }

  ngOnInit(): void {
    // Fetch only when the notes drawer is opened.
  }

  getAllNotebooks() {
    if (this.loading) return;
    this.loading = true; this.loadError = false;
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
          this.loaded = true; this.loading = false;
        },
        error: () => { this.loading = false; this.loadError = true; },
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
    if (this.openChatBox && !this.loaded) this.getAllNotebooks();
  }

  updateNoteBooks() {
    if (!this.loaded || this.saving) return;
    this.saving = true; this.message = '';
    // console.log('updated notebooks = ');
    // console.dir(this.notebooks, { depth: null });
    this.notebookService.updateNotebooks(this.notebooks)
      .subscribe({
        next: (value) => {
          // console.log('updated all notebooks successfully! ' + value);
          this.saving = false;
          this.dirty = false; this.message = 'Saved to the shared notebook.'; this.focusFeedback();
        },
        error: () => { this.saving = false; this.message = 'Save could not be confirmed. Your draft is still here. Check before submitting again.'; this.focusFeedback(); },
      });

  }

  addTodayNotebookIfMissing() {
    const today = new Intl.DateTimeFormat('en-CA', {timeZone:'Asia/Kolkata', year:'numeric',month:'2-digit',day:'2-digit'}).format(new Date());
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


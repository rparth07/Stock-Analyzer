import { Component, OnInit } from '@angular/core';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../types/Notebook';

@Component({
  selector: 'app-notebook',
  templateUrl: './notebook.component.html',
  styleUrls: ['./notebook.component.css']
})
export class NotebookComponent implements OnInit {

  filterDate: Date = new Date(new Date().toISOString().split('T')[0]);
  openChatBox: Boolean = true;
  existingNotebook: Notebook = {
    contentDate: this.filterDate,
    content: ''
  };

  constructor(private notebookService: NotebookService) {

  }

  ngOnInit(): void {
    this.notebookService.getExistingNotebook(this.filterDate)
      .subscribe({
        next: (value) => {
          this.existingNotebook = value;
          console.log('filter values = ' + value);
          // console.dir(value, { depth: null });
        },
        error: (err) => console.log(err),
      });
  }

  toggleState() {
    this.openChatBox = !this.openChatBox;
  }

  updateContent(content: string) {
    if (content != this.existingNotebook.content) {
      this.existingNotebook.content = content;
      this.notebookService.updateNotebook(this.existingNotebook);
    }
    console.log('content = ' + content);
  }
}

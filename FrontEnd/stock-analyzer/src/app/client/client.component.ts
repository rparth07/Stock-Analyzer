import { Component, OnInit } from '@angular/core';
import { Form } from '@angular/forms';
import { Client } from '../types/Client';
import { BulkDeal, StockAction } from '../types/BulkDeal';
import { ClientService } from '../services/client.service';
import { SortEvent } from 'primeng/api';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css', '../shared/css/common-page-style.css']
})
export class ClientComponent implements OnInit {
  clients!: Client[];
  selectedClients!: Client[];
  loading: boolean = true;

  getStockAction(value: number) {
    return StockAction[value];
  }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  customSort(event: SortEvent) {
    // console.dir(event, { depth: null });
    event.data!.sort((data1, data2) => {
      let value1 = null;
      let value2 = null;
      if (event.field!.startsWith('deals')) {
        let field = event.field!.split('.')[1];
        value1 = data1.deals.length > 0 ? data1.deals[0][field] : null;
        value2 = data2.deals.length > 0 ? data2.deals[0][field] : null;
      } else {
        value1 = data1[event.field!];
        value2 = data2[event.field!];
      }
      let result = null;

      if (value1 == null && value2 != null)
        result = -1;
      else if (value1 != null && value2 == null)
        result = 1;
      else if (value1 == null && value2 == null)
        result = 0;
      else if (typeof value1 === 'string' && typeof value2 === 'string')
        result = value1.localeCompare(value2);
      else {
        // Compare the values directly
        result = (value1 < value2) ? -1 : (value1 > value2) ? 1 : 0;
      }

      return (event.order! * result);
    });
  }

  constructor(private clientService: ClientService) { }

  ngOnInit() {
    this.getAllClients()
      .subscribe((response: Client[]) => {
        this.clients = response;
        this.loading = false;
        // console.log("this is the data", JSON.stringify(this.clients));
      });
  }

  getAllClients() {
    return this.clientService.fetchAllClients();
  }
}
import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Product } from './product';

@Component({
  selector: 'app-data',
  templateUrl: './data.component.html',
  styleUrls: ['./data.component.less'],
})
export class DataComponent implements OnInit {
  products$: Observable<Product[]>;

  error = false;

  constructor(private data: DataService) {}

  ngOnInit(): void {
    this.error = false;
    this.products$ = this.data.getProducts().pipe(
      catchError((err) => {
        if (err) {
          this.error = true;
          return throwError(err);
        }
      })
    );
  }
}

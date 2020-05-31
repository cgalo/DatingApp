import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  // Properties
  values: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues()
  {
    this.http.get('http://10.0.0.23:5000/api/values').subscribe(response =>{
      this.values = response; // Response will contain the object with our values inside
    }, error => {
      console.log(error);     // Pass the error if we get here
    });
  }

}

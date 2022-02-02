import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private httpClient: HttpClient){}
  
  ngOnInit(): void {
    
    var newAds
    this.getNewAds().subscribe(a=>newAds=a.count)
    console.log(newAds)
  }

  public getNewAds(): Observable<any> {
    return this.httpClient.get('http://localhost:5000/Ad/GetNewAds');
  }

  

  title = 'CLIENT';
}

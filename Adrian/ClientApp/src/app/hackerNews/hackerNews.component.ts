import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-hackerNews',
  templateUrl: './hackerNews.component.html'
})

export class HackerNewsComponent implements OnInit {
  public hackerNews: IHackerNew[] = [];
  private readonly baseUrl: string;
  private readonly http: HttpClient;

  ngOnInit() {
    this.hackerNews = [];
  }

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.search("", 0, 0);
  }

  search(searchFor: string, offset: number, page: number) {
    let params = new HttpParams();
    params = params.append('searchFor', searchFor);
    params = params.append('offset', offset.toString());
    params = params.append('linePerPage', page.toString());
    this.http.get<IHackerNew[]>(this.baseUrl + 'apis/HackerNews/GetHackerNews', { params: params }).subscribe(results => this.hackerNews = results, error => console.error(error));
  }
}

interface IHackerNew {
  id: number;
  title?: string;
  link?: string;
  lastUpdateUtc: string;
}

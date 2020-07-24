import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-hackerNews',
  templateUrl: './hackerNews.component.html',
  styleUrls: ['./hackerNews.component.css']
})

export class HackerNewsComponent implements OnInit {
  public hackerNews: IHackerNew[] = [];
  private readonly baseUrl: string;
  private readonly http: HttpClient;
  private readonly hackerNewApiBaseUrl: string = "https://hacker-news.firebaseio.com/v0/";

  ngOnInit() {
    this.hackerNews = [];
  }

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.search(0, 0);
  }

  search(offset: number, page: number) {
    this.hackerNews = [];
    this.http.get<number[]>(this.hackerNewApiBaseUrl + 'newstories.json?print=pretty').subscribe(
      (itemIds) => {
        if (!offset || offset == 0) { offset = 1; }
        if (!page || page == 0) { page = 50; }

        itemIds.splice(offset-1, itemIds.length - page);

        for (let itemId of itemIds) {
          this.getNewsInfo(itemId);
        }
    }, error => console.error(error));
  }

  getNewsInfo = (itemId) => {
    let params = new HttpParams();
    params = params.append('id', itemId);
    this.http.get<IHackerNew>(this.baseUrl + 'apis/HackerNews/Item', { params: params }).subscribe(
      result => {
        if (result == null) {
          this.requestNewsInfo(itemId)
        } else {
          this.hackerNews.push({ id: itemId, title: result.title, url: result.url });
        }
      }, error => console.error(error));
  }

  requestNewsInfo = (itemId) => {
    this.http.get<IHackerNew>(this.hackerNewApiBaseUrl + 'item/' + itemId + '.json?print=pretty').subscribe(
      (result) => {
        if (result != null) {
          const item = { id: itemId, title: result.title != null ? result.title : '', url: result.url != null ? result.url : '' };
          this.hackerNews.push(item);
          this.cachingNewsInfo(item);
        }
      }, error => console.error(error));
  }

  cachingNewsInfo = (item) => {
    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(item);
    this.http.post(this.baseUrl + 'apis/HackerNews/Item', body, { 'headers': headers }).subscribe();
  }
}

interface IHackerNew {
  id: number;
  title?: string;
  url?: string;
}

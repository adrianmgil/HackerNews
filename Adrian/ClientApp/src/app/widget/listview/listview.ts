import { OnInit, Component, Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-listview',
  templateUrl: './listview.component.html',
  styleUrls: ['./listview.component.css']
})

abstract class IBaseListViewComponent<T extends IList> {
  abstract getTitle(): string;
  abstract getBaseUrl(): string;
  abstract getSearchUrl(): string;
  abstract getInfoUrl(): string;
  abstract createObject(item: T): T;
}

export class BaseListViewComponent<T extends IList> implements OnInit, IBaseListViewComponent<T> {
  public items: T[];
  public title: string;
  private readonly http: HttpClient;
  private readonly baseUrl: string;

  ngOnInit() {
    this.items = [];
  }

  constructor(http: HttpClient, title: string, baseUrl: string) {
    this.http = http;
    this.title = title;
    this.baseUrl = baseUrl;

    if (this.getSearchUrl() !== null) {
      this.search(0, 0);
    }
  }

  getTitle() {
    return null;
  }
  getBaseUrl() {
    return null;
  }

  getSearchUrl() {
    return null;
  }
  getInfoUrl() {
    return null;
  }
  createObject(item: T): T {
    return null;
  }

  search(offset: number, page: number) {
    this.items = [];
    this.http.get<number[]>(this.baseUrl + this.getSearchUrl()).subscribe(
      (itemIds) => {
        if (!offset || offset == 0) { offset = 1; }
        if (!page || page == 0) { page = 50; }

        itemIds.splice(offset - 1, itemIds.length - page);

        for (let itemId of itemIds) {
          this.getInfo(itemId);
        }
      }, error => console.error(error));
  }

  getInfo = (itemId) => {
    const cachedRecord = localStorage.getItem(this.baseUrl + itemId);

    if (cachedRecord !== null) {
      const result = JSON.parse(cachedRecord);
      if (result.expiration + (5 * 60 * 1000) > new Date().getTime()) {
        this.items.push(this.createObject(result.item));
        return;
      }
    }

    this.requestInfo(itemId);
  }

  requestInfo = (itemId) => {
    this.http.get<T>(this.baseUrl + this.getInfoUrl().replace("{0}", itemId)).subscribe(
      (result) => {
        if (result != null) {
          const item = this.createObject(result);
          this.items.push(item);
          this.cachingInfo(item);
        }
      }, error => console.error(error));
  }

  cachingInfo = (item) => {
    const record = {
      item: item,
      expiration: new Date().getTime()
    };
    localStorage.setItem(this.baseUrl + item.id, JSON.stringify(record));
  }
}

export interface IList {
  id: number;
}

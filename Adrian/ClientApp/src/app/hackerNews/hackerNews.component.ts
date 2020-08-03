import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseListViewComponent, IList } from '../widget/listview/listview';

@Component({
  selector: 'app-hackerNews',
  templateUrl: './hackerNews.component.html',
  styleUrls: ['./hackerNews.component.css']
})

export class HackerNewsComponent extends BaseListViewComponent<IHackerNew> {

  constructor(http: HttpClient) {
    super(http, 'Hacker News', 'https://hacker-news.firebaseio.com/v0/')
  }

  getSearchUrl() {
    return 'newstories.json?print=pretty';
  }

  getInfoUrl() {
    return 'item/{0}.json?print=pretty';
  }

  createObject(item: IHackerNew) {
    return { id: item.id, title: item.title != null ? item.title : '', url: item.url != null ? item.url : '' };
  }
}

interface IHackerNew extends IList {
  title?: string;
  url?: string;
}

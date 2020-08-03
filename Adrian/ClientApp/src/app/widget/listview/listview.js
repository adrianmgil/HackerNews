"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var IBaseListViewComponent = /** @class */ (function () {
    function IBaseListViewComponent() {
    }
    IBaseListViewComponent = __decorate([
        core_1.Component({
            selector: 'app-listview',
            templateUrl: './listview.component.html',
            styleUrls: ['./listview.component.css']
        })
    ], IBaseListViewComponent);
    return IBaseListViewComponent;
}());
var BaseListViewComponent = /** @class */ (function () {
    function BaseListViewComponent(http, title, baseUrl) {
        var _this = this;
        this.getInfo = function (itemId) {
            var cachedRecord = localStorage.getItem(_this.baseUrl + itemId);
            if (cachedRecord !== null) {
                var result = JSON.parse(cachedRecord);
                if (result.expiration + (5 * 60 * 1000) > new Date().getTime()) {
                    _this.items.push(_this.createObject(result.item));
                    return;
                }
            }
            _this.requestInfo(itemId);
        };
        this.requestInfo = function (itemId) {
            _this.http.get(_this.baseUrl + _this.getInfoUrl().replace("{0}", itemId)).subscribe(function (result) {
                if (result != null) {
                    var item = _this.createObject(result);
                    _this.items.push(item);
                    _this.cachingInfo(item);
                }
            }, function (error) { return console.error(error); });
        };
        this.cachingInfo = function (item) {
            var record = {
                item: item,
                expiration: new Date().getTime()
            };
            localStorage.setItem(_this.baseUrl + item.id, JSON.stringify(record));
        };
        this.http = http;
        this.title = title;
        this.baseUrl = baseUrl;
        if (this.getSearchUrl() !== null) {
            this.search(0, 0);
        }
    }
    BaseListViewComponent.prototype.ngOnInit = function () {
        this.items = [];
    };
    BaseListViewComponent.prototype.getTitle = function () {
        return null;
    };
    BaseListViewComponent.prototype.getBaseUrl = function () {
        return null;
    };
    BaseListViewComponent.prototype.getSearchUrl = function () {
        return null;
    };
    BaseListViewComponent.prototype.getInfoUrl = function () {
        return null;
    };
    BaseListViewComponent.prototype.createObject = function (item) {
        return null;
    };
    BaseListViewComponent.prototype.search = function (offset, page) {
        var _this = this;
        this.items = [];
        this.http.get(this.baseUrl + this.getSearchUrl()).subscribe(function (itemIds) {
            if (!offset || offset == 0) {
                offset = 1;
            }
            if (!page || page == 0) {
                page = 50;
            }
            itemIds.splice(offset - 1, itemIds.length - page);
            for (var _i = 0, itemIds_1 = itemIds; _i < itemIds_1.length; _i++) {
                var itemId = itemIds_1[_i];
                _this.getInfo(itemId);
            }
        }, function (error) { return console.error(error); });
    };
    return BaseListViewComponent;
}());
exports.BaseListViewComponent = BaseListViewComponent;
//# sourceMappingURL=listview.js.map
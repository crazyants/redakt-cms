import {inject} from 'aurelia-framework';

export class SiteListItem {
    constructor() {
    }

    public id: string;
    public name: string;
    public homePageId: string;
}

export class Site extends SiteListItem {
    constructor() {
        super();
    }
}

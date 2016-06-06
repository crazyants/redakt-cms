import {inject} from 'aurelia-framework';

export class PageTreeItem {
    constructor() {
    }

    public id: string;
    public name: string;
    public parentId: string;
    public hasChildren: boolean;
}

export class Page extends PageTreeItem {
    constructor() {
        super();
    }
}

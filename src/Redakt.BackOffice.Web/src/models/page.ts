import {inject} from 'aurelia-framework';
import {IPageTreeItem} from '../models/interfaces';

export class Page implements IPageTreeItem {
    constructor(dto) {
        Object.apply(this, dto);
    }

    public id: string;
    public name: string;
    public parentId: string;
    public iconClass: string;
    public hasChildren: boolean;
}

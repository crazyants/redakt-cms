import {inject} from 'aurelia-framework';
import {ISiteListItem} from '../models/interfaces';

export class Site implements ISiteListItem {
    constructor(dto: ISiteListItem) {
        Object.apply(this, dto);
    }

    public id: string;
    public name: string;
    public homePageId: string;
}

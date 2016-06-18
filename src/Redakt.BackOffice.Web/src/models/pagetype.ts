import {inject} from 'aurelia-framework';
import {IPageType, IFieldDefinition} from '../models/interfaces';

export class PageType implements IPageType {
    constructor(dto) {
        Object.apply(this, dto);
    }

    public id: string;
    public dbCreated: Date;
    public dbUpdated: Date;
    public name: string;
    public compositedPageTypeIds: Array<string>;
    public fields: Array<IFieldDefinition>;
    public iconClass: string;
}

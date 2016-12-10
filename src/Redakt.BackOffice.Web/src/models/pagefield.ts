import {inject} from 'aurelia-framework';
import {IFieldDefinition} from './interfaces';

export class PageField {
    key: string;
    value: any;
    definition: IFieldDefinition;
}

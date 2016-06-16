import {inject} from 'aurelia-framework';
import {IFieldDefinition} from './fielddefinition';

export interface IPageType {
    id: string;
    name: string;
    fields: Array<IFieldDefinition>;
}

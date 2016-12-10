import {bindable, customElement} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {IFieldDefinition} from '../models/interfaces';
import {PageField} from '../models/pagefield';

export class TextboxEditor {
    field: PageField;
    settings: any;

    constructor() {
        
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(field: PageField) {
        this.field = field;
        this.settings = field.definition.editorSettings;
    }  
}
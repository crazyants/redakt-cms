import {bindable, customElement} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {IFieldDefinition} from '../models/interfaces';
import {PageField} from '../models/pagefield';

export class TextboxEditor {
    private field: PageField;
    private config: any;

    constructor() {
        
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(field: PageField) {
        this.field = field;
        this.config = field.definition.editorConfig;
    }  
}
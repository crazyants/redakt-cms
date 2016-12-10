import {bindable, customElement} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {IFieldDefinition} from '../models/interfaces';
import {PageField} from '../models/pagefield';
import 'summernote'

export class RichTextEditor {
    field: PageField;

    constructor() {
        
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(field: PageField) {
        this.field = field;
    }  

    public attached() {
		// this.editor = $(''`#${this.editorId}`'');
		// this.editor.data('view-model', this);
		// this.editor.summernote({
		// 	height: this.height,
		// 	toolbar: this.toolbar,
		// 	onChange: (contents) => {
		// 		this.value = contents;
		// 	}
		// });
		// this.editor.code(this.value);

        (<any>$('.summernote')).summernote({
			//height: this.height,
			//toolbar: this.toolbar,
			onChange: (contents) => {
				//this.value = contents;
			}
		});
	}

	public detached() {
		//this.editor.destroy();
	}
}
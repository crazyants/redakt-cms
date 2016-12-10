import {bindable, customElement} from 'aurelia-framework';
import {activationStrategy} from 'aurelia-router';
import {IFieldDefinition} from '../models/interfaces';
import {PageField} from '../models/pagefield';
import 'summernote'

export class RichTextEditor {
    private field: PageField;
    private config: any;
	private $editor: any;

    constructor() {
        
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(field: PageField) {
        this.field = field;

		var config = field.definition.editorConfig || { };
		this.config = 
		{ 
			height: config.height || 200
		};
    }  

    public attached() {
		this.$editor = $('#editor-' + this.field.key);
		let field = this.field;
		this.$editor.summernote({
			height: this.config.height,
			callbacks: {
				onChange: function(contents, $editable) {
					field.value = contents;
				}
			}
		});
	}

	public detached() {
		this.$editor.summernote('destroy');
	}
}
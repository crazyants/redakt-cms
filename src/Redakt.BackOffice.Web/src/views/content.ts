import {bindable, autoinject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Page} from '../models/page';
import {IPageType} from '../models/interfaces';
import {IFieldDefinition} from '../models/interfaces';
import {PageField} from '../models/pagefield';
import {PageService, PageTypeService} from '../services/services';

@autoinject
export class ContentView {
    private page: Page;
    private pageType: IPageType;

    constructor(private router: Router, private pageService: PageService, private pageTypeService: PageTypeService) {
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(page: Page) {
        this.page = page;
        if (page) {
            return this.pageTypeService.getPageType(page.pageTypeId).then(pageType => {
                this.pageType = pageType;
            });
        }
    }

    public field(key: string): PageField {
        var field = new PageField();
        field.key = key;
        field.definition = this.pageType.fields.find(x => x.key === key);
        var fieldValue = this.page.fields.find(x => x.key === key);
        if (fieldValue) field.value = fieldValue.value;
        
        return field;
    }
}
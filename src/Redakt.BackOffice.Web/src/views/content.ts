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
    private fields: Array<PageField>;

    constructor(private router: Router, private pageService: PageService, private pageTypeService: PageTypeService) {
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(page: Page) {
        this.page = page;
        if (page) {
            this.fields = [];
            return this.pageTypeService.getPageType(page.pageTypeId).then(pageType => {
                this.pageType = pageType;
                this.setFields();
            });
        }
    }

    private setFields() {
        this.pageType.fields.forEach(f => {
            var field = new PageField();
            field.key = f.key;
            field.definition = f;
            var fieldValue = this.page.fields.find(x => x.key === f.key);
            if (fieldValue) field.value = fieldValue.value;
            this.fields.push(field);
        });
    }

    public save() 
    {
        this.page.fields = [];
        this.fields.forEach(f => {
            this.page.fields.push({ key: f.key, value: f.value });
        });
        this.pageService.updatePage(this.page);
    }
}
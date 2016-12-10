import {bindable, autoinject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Page} from '../models/page';
import {IPageType} from '../models/interfaces';
import {IFieldDefinition, IPage} from '../models/interfaces';
import {PageField} from '../models/pagefield';
import {PageService, PageTypeService} from '../services/services';

@autoinject
export class ContentView {
    private page: IPage;
    private pageType: IPageType;
    private fields: Array<PageField>;

    constructor(private router: Router, private pageService: PageService, private pageTypeService: PageTypeService) {
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(page: IPage) {
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
        var content = this.page.content[0];
        this.pageType.fields.forEach(f => {
            var field = new PageField();
            field.key = f.key;
            field.definition = f;
            field.value = content.fields[f.key];
            this.fields.push(field);
        });
    }

    public save() 
    {
        var content = this.page.content[0];
        content.fields = [];
        this.fields.forEach(f => {
            content.fields.push({ key: f.key, value: f.value });
        });
        this.pageService.updatePage(this.page);
    }
}
import {bindable, autoinject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Page} from '../models/page';
import {IPageType} from '../models/pagetype';
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
            //this.pageTypeService.getPageType(page.
        }
    }
}
import {bindable, autoinject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {PageType} from '../models/pagetype';
import {PageTypeService} from '../services/services';

@autoinject
export class PageTypeView {
    private pageType: PageType;
    public selectedTab = 'settings';

    constructor(private router: Router, private pageTypeService: PageTypeService) {
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(pageType: PageType) {
        this.pageType = pageType;
        if (pageType) {
            //this.pageTypeService.getPageType(page.
        }
    }
}
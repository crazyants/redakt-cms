import {bindable, autoinject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {Page} from '../models/page';
import {PageService} from '../services/services';

@autoinject
export class ContentView {
    private page: Page;

    constructor(private router: Router, private pageService: PageService) {
    }

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(page: Page) {
        this.page = page;
        if (page) {
        }
    }
}
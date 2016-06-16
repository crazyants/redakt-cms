import {autoinject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {ISiteListItem, IPageTreeItem} from '../models/interfaces';
import {Page} from '../models/page';
import {SiteService, PageService} from '../services/services';

@autoinject
export class PageView {
    constructor(private router: Router, private siteService: SiteService, private pageService: PageService) {
		//debugger;
    }

    public sites: Array<ISiteListItem>;
    public selectedSite: ISiteListItem;
    public homePage: IPageTreeItem;
    public currentPage: Page;

    public determineActivationStrategy() {
        return activationStrategy.invokeLifecycle;
    }

    public activate(params, config, instruction) {
        if (params.id) {
            this.pageService.getPage(params.id).then(page => {
                this.currentPage = page;
            });
        }

        return this.sites || this.siteService.getAllSites().then(sites => {
            this.sites = sites;
            if (this.sites.length > 0) {
                this.selectedSite = sites[0];
                this.siteSwitched();
            }
        });
    }

    public siteSwitched() {
        this.pageService.getPageTreeItem(this.selectedSite.homePageId).then(page => {
            this.homePage = page;
        });
    }

    public newSite() {
    }
}
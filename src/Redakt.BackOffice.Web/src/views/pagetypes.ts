import {autoinject} from 'aurelia-framework';
import {Router, activationStrategy} from 'aurelia-router';
import {IPageType, IPageTypeListItem} from '../models/interfaces';
import {PageTypeService} from '../services/services';

@autoinject
export class PageTypesView {
    constructor(private router: Router, private pageTypeService: PageTypeService) {
		//debugger;
    }

    public showList = true;
    public pageTypes: Array<IPageTypeListItem>;
    public currentPageType: IPageType;

    public activate(params, config, instruction) {
        this.showList = !params.id;

        if (params.id) {
            return this.pageTypeService.getPageType(params.id).then(pageType => {
                this.currentPageType = pageType;
            });
        }

        return this.pageTypes || this.pageTypeService.getAllPageTypes().then(pageTypes => {
            this.pageTypes = pageTypes;
        });
    }

    public newPageType() {
    }
}
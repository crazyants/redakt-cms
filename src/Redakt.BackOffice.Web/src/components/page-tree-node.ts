import {bindable, autoinject} from 'aurelia-framework';
import {PageService} from '../services/services';
import {IPageTreeItem} from '../models/interfaces';

@autoinject
export class PageTreeNode {
    @bindable page: IPageTreeItem;

    constructor(private pageService: PageService) {
        //debugger;
    }

    public isVisible: boolean = true;
    public isExpanded: boolean = false;
    public isLoading: boolean = false;
    public children: Array<IPageTreeItem>;

    public toggle() {
        this.isExpanded = !this.isExpanded;
        if (this.isExpanded && this.page.hasChildren && !this.children) {
            this.isLoading = true;
            this.pageService.getPageChildrenTreeItem(this.page.id).then(pages => {
                this.children = pages;
                this.isLoading = false;
            });
        }
    }
}
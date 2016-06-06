import {bindable, autoinject} from 'aurelia-framework';
import {PageService} from '../services/services';
import {PageTreeItem} from '../models/page';

@autoinject
export class PageTreeNode {
    @bindable page: PageTreeItem;

    constructor(private pageService: PageService) {
        //debugger;
    }

    public isVisible: boolean = true;
    public isExpanded: boolean = false;
    public isLoading: boolean = false;
    public children: Array<PageTreeItem>;

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
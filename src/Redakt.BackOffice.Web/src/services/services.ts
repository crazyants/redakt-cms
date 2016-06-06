import {autoinject} from 'aurelia-framework';
import {Api} from '../api';
import {Site, SiteListItem} from '../models/site';
import {Page, PageTreeItem} from '../models/page';

@autoinject()
export class PageService {
    constructor(private api: Api) { }

    public getPage(id: string): Promise<Page> {
        return this.api.get('pages/' + id);
    }

    public getPageTreeItem(id: string): Promise<PageTreeItem> {
        return this.api.get('pages/' + id + '/treeitem');
    }

    public updatePage(page: Page): Promise<void> {
        return this.api.put('pages/' + page.id, page);
    }

    public createPage(page: Page): Promise<string> {
        return this.api.post('pages', page);
    }

    public deletePage(id: string): Promise<void> {
        return this.api.delete('pages/' + id);
    }

    public getPageChildren(parentId: string): Promise<Array<Page>> {
        return this.api.get('pages/' + parentId + '/children');
    }

    public getPageChildrenTreeItem(parentId: string): Promise<Array<PageTreeItem>> {
        return this.api.get('pages/' + parentId + '/children/treeitem');
    }
}

@autoinject()
export class SiteService {
    constructor(private api: Api) { }

    public getSite(id: string): Promise<Site> {
        return this.api.get('sites/' + id);
    }

    public updateSite(site: Site): Promise<void> {
        return this.api.put('sites/' + site.id, site);
    }

    public createSite(site: Site): Promise<string> {
        return this.api.post('sites', site);
    }

    public deleteSite(id: string): Promise<void> {
        return this.api.delete('sites/' + id);
    }

    public getAllSites(): Promise<Array<SiteListItem>> {
        return this.api.get('sites/list');
    }
}

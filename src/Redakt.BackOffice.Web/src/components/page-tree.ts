import {bindable} from "aurelia-framework";
import {PageTreeItem} from '../models/page';

export class PageTree {
    @bindable root: PageTreeItem;

    constructor() {
    }
}

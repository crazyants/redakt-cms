
export interface IPageTreeItem {
    id: string;
    parentId: string;
    pageTypeId: string;
    name: string;
    iconClass: string;
    hasChildren: boolean;
}

export interface IPageUpdate {
    pageTypeId: string;
    name: string;
}

export interface IPage extends IPageUpdate {
    id: string;
    parentId: string;
    content: Array<IPageContent>;
}

export interface IFieldDefinition {
    key: string;
    label: string;
    fieldTypeId: string;
    groupName: string;
    sectionName: string;
    editorElementName: string;
    editorConfig: any;
}

export interface ISiteListItem {
    id: string;
    name: string;
    homePageId: string;
}

export interface IPageTypeListItem {
    id: string;
    name: string;
    iconClass: string;
}

export interface IPageType {
    id: string;
    name: string;
    iconClass: string;
    fields: Array<IFieldDefinition>;
}

export interface IPageContentUpdate {
    culture: string;
    fields: any;
}

export interface IPageContent extends IPageContentUpdate {
    id: string;
    pageId: string;
}


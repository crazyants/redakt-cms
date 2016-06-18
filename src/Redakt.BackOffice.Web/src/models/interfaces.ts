
export interface IPageTreeItem {
    id: string;
    parentId: string;
    name: string;
    iconClass: string;
    hasChildren: boolean;
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

export interface IUser extends IPersistedEntity {
    name: string;
}

export interface IFieldType extends IPersistedEntity {
    name: string;
    fieldEditorId: string;
    fieldEditorSettings: any;
}

export interface IObjectId {
}

export interface IFieldDefinition {
    key: string;
    label: string;
    fieldTypeId: string;
    groupName: string;
    sectionName: string;
}

export interface IPage extends IPersistedEntity {
    name: string;
    parentId: string;
    ancestorIds: any;
    hasChildren: boolean;
    pageTypeId: string;
    templateId: string;
    createdAt: Date;
    createdByUserId: string;
    publishedAt?: Date;
    publishedByUserId: string;
    isPublished: boolean;
}

export interface IResource extends IPersistedEntity {
    name: string;
    contentId: string;
    contentTypeId: string;
}

export interface IPageContent extends IPersistedEntity {
    pageId: string;
    culture: string;
    created: Date;
    fields: any;
    createdUserId: string;
}

export interface IPersistedEntity {
    id: string;
    dbCreated: Date;
    dbUpdated: Date;
}

export interface ISite extends IPersistedEntity {
    homePageId: string;
    name: string;
}

export interface IContentView {
    content: IPageContent;
    pageType: IPageType;
}

export interface IPageType extends IPersistedEntity {
    name: string;
    compositedPageTypeIds: Array<any>;
    fields: Array<IFieldDefinition>;
    iconClass: string;
}


export type GetFolderContentDTO = {
    id: string;
    name: string;
    description: string;
    path: string;
    createdDate: Date;
    updatedDate: Date;
    parentFolderId: string;
    isFolder: boolean;
};
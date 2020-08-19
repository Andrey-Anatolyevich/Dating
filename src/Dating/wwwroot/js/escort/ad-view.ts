class AdViewPicsManager {
    constructor(activePicImgElementId: string, miniPicsContainerId: string, picsList: PicViewInfo[]) {
        this._activePicImgElementId = activePicImgElementId;
        this._activePicImgElement = document.getElementById(activePicImgElementId) as HTMLImageElement;

        this._miniPicsBox = document.getElementById(miniPicsContainerId) as HTMLDivElement;

        this._pics = picsList;
        this._activePic = picsList.find(pic => pic.isMain);
        if (this._activePic == null && picsList.length > 0)
            this._activePic = picsList[0];
    }

    private _miniPicIdAttributeName = 'data-pic-id';
    private _selectMiniPicClassName = 'ad-view-media-preview-item-selected';

    private _activePicImgElementId: string;
    private _activePicImgElement: HTMLImageElement;

    private _miniPicsBox: HTMLDivElement;

    private _pics: PicViewInfo[];
    private _activePic: PicViewInfo;

    public selectPic(picId: number) {
        if (picId == null)
            return;

        this._pics.forEach(pc => pc.selected = pc.id == picId);
        this._activePic = this._pics.find(pic => pic.selected);
        this._selectChosenMiniPic();
        this._renderSelectPic();
    }

    private _selectChosenMiniPic(): void {
        for (let i = 0; i < this._miniPicsBox.children.length; i++) {
            let someNode = this._miniPicsBox.children[i];
            if (someNode == null)
                continue;

            let domElement = (someNode as HTMLElement);
            if (!domElement.hasAttribute(this._miniPicIdAttributeName))
                continue;

            let elementPicId = (someNode as HTMLElement).getAttribute(this._miniPicIdAttributeName);
            if (elementPicId == this._activePic.id.toString())
                domElement.classList.add(this._selectMiniPicClassName);
            else
                domElement.classList.remove(this._selectMiniPicClassName);
        }
    }

    private _renderSelectPic(): void {
        this._activePicImgElement.src = this._activePic.url;
    }
}

class PicViewInfo {
    public id: number;
    public url: string;
    public isMain: boolean;
    public selected: boolean;
}
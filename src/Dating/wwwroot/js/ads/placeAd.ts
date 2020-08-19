/// <reference path="../tools/Utils.ts" />

class PicsManager {
    constructor(picturesContainerId: string, pics: PicInfo[], picsChanged: Function) {
        this._pictures = pics;
        this._picsChanged = picsChanged;
        this._picsBoxElement = document.getElementById(picturesContainerId) as HTMLDivElement;
        this.PrepPicsBox();

        this.sortableContainer = new SortableContainer(picturesContainerId, this.pullOrderFromDom.bind(this));
        this.sortableContainer.reinit();

        this.SetMainPicIfNone();
        this.RenderPics();
        this.reportChanges();
    }

    private readonly _uploadPicUrl: string = '/Ads/UploadPicture';
    private readonly _picBoxAttributeId: string = 'data-pic-id';
    private _pictures: PicInfo[];
    private _picsBoxElement: HTMLDivElement;
    private sortableContainer: SortableContainer;
    private _picsChanged: Function;

    private pullOrderFromDom() {
        let picturesReordered = [];

        let domPicElements = this._picsBoxElement.children;
        for (let index = 0; index < domPicElements.length; index++) {
            let thisDomPic = domPicElements[index];
            let idString = thisDomPic.getAttribute(this._picBoxAttributeId);
            let id = parseInt(idString);

            let foundPic = this._pictures.filter(x => x.adMediaId == id)[0];
            picturesReordered.push(foundPic);
        }

        this._pictures = picturesReordered;

        this.reportChanges();
    }

    private reportChanges() {
        this._picsChanged == null || this._picsChanged(this._pictures);
    }

    private PrepPicsBox() {
        this._picsBoxElement.style.display = 'flex';
        this._picsBoxElement.style.flexDirection = 'row';
        this._picsBoxElement.style.flexWrap = 'wrap';
    }

    public UploadPic() {
        let uploader = new FileUploader();

        let newFileInput = document.createElement('input') as HTMLInputElement;
        newFileInput.type = 'file';
        newFileInput.click();
        newFileInput.onchange = (ev) => {
            if (newFileInput.files != null && newFileInput.files.length > 0) {
                let firstFile = newFileInput.files[0] as File;

                uploader.UploadFile(
                    this._uploadPicUrl
                    , firstFile
                    , (responseText) => {
                        var pic = JSON.parse(responseText) as PicInfo;
                        this.AddPic(pic);
                    }
                    , () => {
                        console.error('Request failed.');
                    });
            }
        };
    }

    private AddPic(pic: PicInfo): void {
        this._pictures.push(pic);
        this.SetMainPicIfNone();
        this.RenderPics();
        this.reportChanges();
    }

    public RemovePic(picId: number): void {
        this._pictures = this._pictures.filter((pic, index, allPics) => pic.adMediaId != picId);
        this.SetMainPicIfNone();
        this.RenderPics();
        this.reportChanges();
    }

    public MakePicMain(picId: number): void {
        this._pictures.forEach((pic, index, allPics) => pic.isMain = pic.adMediaId == picId);
        this.RenderPics();
        this.reportChanges();
    }

    private SetMainPicIfNone(): void {
        if (this._pictures.length > 0 && this._pictures.every(pic => pic.isMain != true))
            this._pictures[0].isMain = true;
    }

    private RenderPics(): void {
        this._picsBoxElement.innerHTML = '';
        this._pictures.forEach((pic, index, allPics) => {

            var eBox = document.createElement('div');
            eBox.id = 'user_tmp_pic_' + index;
            eBox.setAttribute(this._picBoxAttributeId, pic.adMediaId.toString());
            eBox.classList.add('user-pic-box');
            eBox.classList.add('draggable');
            if (pic.isMain)
                eBox.classList.add('is-main-picture');

            let eImg = document.createElement('img');
            eImg.classList.add('the-pic');
            eImg.src = pic.relativePicUrl;
            eBox.appendChild(eImg);

            let btnsPanel = document.createElement('div');
            btnsPanel.classList.add('pic-box-btns');
            eBox.appendChild(btnsPanel);

            let eRemoveBtn = document.createElement('span');
            eRemoveBtn.classList.add('pic-remove');
            eRemoveBtn.innerText = 'X';
            eRemoveBtn.onclick = (ev) => {
                this.RemovePic(pic.adMediaId);
            };
            btnsPanel.appendChild(eRemoveBtn);

            if (!pic.isMain) {
                let eMakeMain = document.createElement('span');
                eMakeMain.classList.add('pic-make-main');
                eMakeMain.innerText = 'Is main';
                eMakeMain.onclick = (ev) => {
                    this.MakePicMain(pic.adMediaId);
                };
                btnsPanel.appendChild(eMakeMain);
            }

            this._picsBoxElement.appendChild(eBox);
        });

        this.sortableContainer.reinit();
    }
}

class IdedElement {
    constructor(id: number, element: HTMLElement) {
        this.privateId = id;
        this.element = element;
    }

    public privateId: number;
    public element: HTMLElement;
}

class SortableContainer {
    constructor(elementId: string, orderChanged: Function) {
        this._elementsContainerId = elementId;
        this._dragoverClass = 'dragover';
        this._draggingClass = 'dragging';
        this._idedLastId = 0;
        this._onOrderChanged = orderChanged;

        this._elementsContainer = document.getElementById(elementId) as HTMLElement;
        this._sortableElements = [];

        this.reinit();
    }

    private _dragoverClass: string;
    private _draggingClass: string;

    private _elementsContainerId: string;
    private _elementsContainer: HTMLElement;

    private _draggingElement: IdedElement;

    private _idedLastId: number;
    private _sortableElements: IdedElement[];

    private _onOrderChanged: Function;

    public reinit(): void {
        this.collectChildrenUpdateSortableIds();
        this.makeChildrenSortable();
    }

    private collectChildrenUpdateSortableIds(): void {
        this._idedLastId = 0;
        this._sortableElements = [];

        let children = this._elementsContainer.children;

        for (let index = 0; index < children.length; index++) {
            let child = children[index];
            if (child == null)
                continue;

            this._idedLastId += 1;
            child.setAttribute('data-sortable-id', this._idedLastId.toString());
            var ided = new IdedElement(this._idedLastId, child as HTMLElement)
            this._sortableElements.push(ided);
        }
    }

    private makeChildrenSortable() {
        let children = this._elementsContainer.children;

        for (let index = 0; index < children.length; index++) {
            let child = children[index];
            if (child == null)
                continue;

            child.setAttribute('draggable', 'true');
            child.addEventListener('dragstart', this.onDragStart.bind(this));
            child.addEventListener('dragend', this.onDragEnd.bind(this));
            child.addEventListener('dragover', this.onDragover.bind(this));
            child.addEventListener('dragleave', this.onDragLeave.bind(this));
            child.addEventListener('dragend', this.onDragLeave.bind(this));
            child.addEventListener('drop', this.onDrop.bind(this));
        }
    }

    private removeDragoverClass(element: HTMLElement) {
        if (element.classList != null)
            element.classList.remove(this._dragoverClass);
    }

    private getDraggable(element: HTMLElement): IdedElement {
        let tmpElement = element;
        let foundElement;
        let sortableId;
        while (foundElement == null && tmpElement != null) {
            sortableId = tmpElement.getAttribute('data-sortable-id');
            if (sortableId == null || sortableId.length <= 0)
                tmpElement = tmpElement.parentElement;
            else {
                foundElement = tmpElement;
            }
        }

        if (foundElement == null)
            return null;

        return new IdedElement(parseInt(sortableId), foundElement);
    }

    private onDragStart(event: DragEvent) {
        let pickedElement = event.target as HTMLElement;
        let idedElement = this.getDraggable(pickedElement);
        if (idedElement == null)
            return;

        this._draggingElement = idedElement;

        this._draggingElement.element.classList.add(this._draggingClass);
        event.dataTransfer.setData("Text", idedElement.privateId.toString());
    }

    private onDragEnd(event: DragEvent) {
        this._sortableElements.forEach(elm => this.removeDragoverClass(elm.element));
    }

    private onDragover(event: DragEvent) {
        event.preventDefault();

        var targetElement = this.getDraggable(event.target as HTMLElement);
        if (targetElement != null)
            targetElement.element.classList.add(this._dragoverClass);
    }

    private onDragLeave(event: DragEvent) {
        event.stopPropagation();
        event.preventDefault();

        let underDragElement = this.getDraggable(event.target as HTMLElement);
        if (underDragElement != null)
            this.removeDragoverClass(underDragElement.element);
    }

    private onDrop(dEvent: any) {
        dEvent.stopPropagation();
        dEvent.preventDefault();

        let sourceSortableId = dEvent.dataTransfer.getData("Text") as string;
        let dropTargetIdedElement = this.getDraggable(dEvent.target as HTMLElement);
        let targetSortableId = dropTargetIdedElement.privateId;

        if (sourceSortableId == null || sourceSortableId.length <= 0) {
            return;
        }
        if (sourceSortableId == targetSortableId.toString()) {
            return;
        }

        this.moveSortableOntoAnother(parseInt(sourceSortableId), targetSortableId);

        if (this._onOrderChanged)
            this._onOrderChanged();
    }

    private moveSortableOntoAnother(sourceSortableId: number, targetSortableId: number): any {
        let froms = this._sortableElements.filter(elm => elm.privateId == sourceSortableId);
        let tos = this._sortableElements.filter(elm => elm.privateId == targetSortableId);

        let from = froms[0];
        let to = tos[0];

        if (from.privateId > to.privateId) {
            to.element.before(from.element);
        }
        else {
            to.element.after(from.element);
        }

        this.collectChildrenUpdateSortableIds();
    }
}

class PicInfo {
    public adMediaId: number;
    public relativePicUrl: string;
    public isMain: boolean;
}

class FileUploader {
    constructor() {
    }

    public UploadFile(url: string, file: File, onSuccess: Function, onError: Function): void {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", url, true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4) {
                if (xhr.status == 200) {
                    if (onSuccess != null)
                        onSuccess(xhr.responseText);
                }
                else {
                    if (onError != null)
                        onError();
                }
            }
        };

        let fData = new FormData();
        fData.append(file.name, file);

        xhr.send(fData);
    }
}

class EditAdSettings {
    public maxPicsAllowed: number;
    public picIdsSeparator: string;
    public existingPicsJson: string;
}

let picsManager: PicsManager;
declare var editAdSettings: EditAdSettings;

(function () {
    let jsonUnescaped = Utils.decodeHtml(editAdSettings.existingPicsJson);
    let existingPics: PicInfo[] = JSON.parse(jsonUnescaped);

    picsManager = new PicsManager('PicturesBox', existingPics, (pics: PicInfo[]) => {
        let mainPicIdInput = document.getElementById("main-pic-id") as HTMLInputElement;
        let picsIdsInput = document.getElementById("pics-ids") as HTMLInputElement;

        let allPicsIds = [];
        pics.forEach(pc => {
            if (pc.isMain)
                mainPicIdInput.value = pc.adMediaId.toString();

            allPicsIds.push(pc.adMediaId);
        });
        picsIdsInput.value = allPicsIds.join(editAdSettings.picIdsSeparator);

        let picsUploaded = document.getElementById("pics-uploaded") as HTMLSpanElement;
        picsUploaded.innerText = allPicsIds.length.toString();

        let addPicBtn = document.getElementById('add-pic-btn') as HTMLInputElement;
        addPicBtn.disabled = allPicsIds.length >= editAdSettings.maxPicsAllowed;
    });
})();
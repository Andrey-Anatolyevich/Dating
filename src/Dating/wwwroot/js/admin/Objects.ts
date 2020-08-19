var jQuery: any;
var treeContainer: HTMLSelectElement;
var tree: any;
var jsTree: any;
var translator: ObjectTranslator;

function ForceReloadData(): void {
    if (treeContainer == null)
        treeContainer = document.getElementById('ObjectTypeSelector') as HTMLSelectElement;

    ObjectTypeChanged(treeContainer);
}

function ObjectTypeChanged(objectTypeSelector: HTMLSelectElement): void {

    let selectIndex = objectTypeSelector.selectedIndex;
    let selectTypeOption = objectTypeSelector[selectIndex] as HTMLOptionElement;
    let selectType = selectTypeOption.value;

    let url = `/Admin/Basics/GetObjectsOfType?typeId=${selectType}`;


    jQuery.ajax({
        async: true,
        type: "POST",
        url: url,
        dataType: "json",
        success: function (json) {
            let result = [];
            json.forEach((item, index, array) => {
                item.id = item.Id;
                item.parent = item.ParentId == null ? '#' : item.ParentId;
                item.text = item.ObjectCode;
                result.push(item);
            });

            SetTreeWithData(result);
        },

        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function SetTreeWithData(data): void {
    if (jsTree != null) {
        jsTree.settings.core.data = data;
        jsTree.refresh();
    } else {
        tree = jQuery('#ObjectsTree').jstree({
            'core': {
                'data': data
            },
            'plugins': ["search"]
        });
        jsTree = tree.jstree(true);

        tree.bind(
            "select_node.jstree", function (evt, data) {
                if (data.selected.length > 0) {
                    var id = parseInt(data.selected[0]);
                    translator.loadForId(id);
                }
                else {
                    translator.clear();
                }
            }
        );
    }
}

function Add(parentId: number): void {
    let value = GetInputNameValue();
    let typeId = GetTypeId();

    jQuery.ajax({
        async: true,
        type: "POST",
        url: '/Admin/Basics/ObjectCreate',
        data: {
            typeId: typeId,
            parentId: parentId,
            code: value
        },
        success: function (json) {
            ForceReloadData();
            ClearInputNameValue();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function AddAsChild(): void {
    let selectedId = GetSelectedObjectId();
    Add(selectedId);
}

function SetCode(): void {
    let id = GetSelectedObjectId();
    let code = GetInputNameValue();

    jQuery.ajax({
        async: true,
        type: "POST",
        url: '/Admin/Basics/ObjectSetCode',
        data: {
            id: id,
            code: code
        },
        success: function (json) {
            ForceReloadData();
            ClearInputNameValue();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function DeleteSelected(): void {
    let selectedId = GetSelectedObjectId();

    var agree = confirm(`Delete object with ID: '${selectedId}'?`);
    if (!agree)
        return;

    jQuery.ajax({
        async: true,
        type: "POST",
        url: '/Admin/Basics/ObjectDelete',
        data: {
            objectId: selectedId
        },
        success: function (json) {
            ForceReloadData();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function GetTypeId() {
    let typeSelect = document.getElementById('ObjectTypeSelector') as HTMLSelectElement;
    let valueString = typeSelect.value as string;
    let value = parseInt(valueString);
    return value;
}

function GetInputNameElement(): HTMLInputElement {
    let result = (document.getElementById('objectName') as HTMLInputElement);
    return result;
}

function GetInputNameValue(): string {
    var element = GetInputNameElement();
    let result = element.value;
    return result;
}

function ClearInputNameValue(): void {
    var element = GetInputNameElement();
    element.value = '';
}

function GetSelectedObjectId(): number {
    let selected = jsTree.get_selected() as Array<string>;
    if (selected.length < 1)
        return;

    let selectedIdString = selected[0];
    let selectedId = parseInt(selectedIdString);
    return selectedId;
}

class ObjectTranslator {
    constructor(translatorContainerId: string) {
        this._containerId = translatorContainerId;
        this._container = document.getElementById(this._containerId) as HTMLDivElement;
        return this;
    }

    private _containerId: string;
    private _container: HTMLDivElement;
    private _currentObjectId: number;

    public loadForId(objectId: number): void {
        this.clear();
        this._currentObjectId = objectId;
        jQuery.ajax({
            async: true,
            type: "POST",
            url: '/Admin/Basics/ObjectTranslationsPartial',
            data: {
                objectId: this._currentObjectId
            },
            success: (data) => {
                this.loadIntoContainer(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    public clear(): void {
        this._container.innerHTML = "";
    }

    public setValue(objectId: number, localeId: number, btn: HTMLInputElement): void {
        var value = this.findValueOf(btn.parentElement, 'value');
        if (value == null)
            throw new Error("Can't find value.");

        jQuery.ajax({
            async: true,
            type: "POST",
            url: '/Admin/Basics/SetObjectTranslation',
            data: {
                objectId: objectId,
                localeId: localeId,
                value: value
            },
            success: (data) => {

            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    private findValueOf(element: HTMLElement, inputName: string): string {

        var result = null;

        var scanElementAndChildren = function (element: HTMLElement, inputName: string): void {
            if (result != null)
                return;
            ;
            if (element.getAttribute('name') == inputName) {
                result = (element as HTMLInputElement).value;
                return;
            }

            if (element.children.length > 0) {
                for (let innerIndex = 0; innerIndex < element.children.length; innerIndex++) {
                    var innerChild = element.children[innerIndex] as HTMLElement;
                    scanElementAndChildren(innerChild, inputName);
                }
            }
        };

        scanElementAndChildren(element, inputName);

        return result;
    }

    private loadIntoContainer(html: string): void {
        this._container.innerHTML = html;
    }
}

(function () {
    translator = new ObjectTranslator("Translator");
})();
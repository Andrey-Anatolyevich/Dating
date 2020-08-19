import * as React from "react";
import { Http } from "../../../tool/http";
import { MergeTool } from "../../../tool/mergeTool";
import { ViewComponent } from "../../ViewComponent";

interface AdminObjectTypesState {
    objectTypes: ObjectType[];
    renamingObjectType: ObjectType;
    renamingObjectTypeNewCode: string;
    selectObjectType: ObjectType;
    failedToLoadObjectTypes: boolean;
    loadingObjectTypes: boolean;
    newObjectTypeCode: string;
}

export class AdminObjectTypes extends ViewComponent<{}, AdminObjectTypesState> {
    constructor(props) {
        super(props);

        this.objectTypeClicked = this.objectTypeClicked.bind(this);
        this.objectTypeRenameClicked = this.objectTypeRenameClicked.bind(this);
        this.objectTypeRenameSaveClicked = this.objectTypeRenameSaveClicked.bind(this);
        this.objectTypeRenameCancelClicked = this.objectTypeRenameCancelClicked.bind(this);
        this.objectTypeRenameValueChanged = this.objectTypeRenameValueChanged.bind(this);
        this.newObjectTypeValueChanged = this.newObjectTypeValueChanged.bind(this);
        this.createNewObjectTypeClicked = this.createNewObjectTypeClicked.bind(this);
        this.fromObjectsToObjectTypesClick = this.fromObjectsToObjectTypesClick.bind(this);

        this.state = {
            objectTypes: [],
            renamingObjectType: null,
            renamingObjectTypeNewCode: '',
            selectObjectType: null,
            failedToLoadObjectTypes: false,
            loadingObjectTypes: false,
            newObjectTypeCode: ''
        };
    }

    public componentDidMount() {
        this.refreshObjectTypes();
    }

    private refreshObjectTypes() {
        new Http()
            .onUrlGet('/Admin/Objects/ObjectTypesGetAll')
            .onSuccess((response) => {
                var objectTypes = JSON.parse(response) as ObjectType[];
                objectTypes = objectTypes.sort((alpha, beta) => alpha.code == beta.code ? 0 : (alpha.code > beta.code ? 1 : -1));
                this.setState({ objectTypes: objectTypes });
            })
            .onError(response => {
                this.setState({ failedToLoadObjectTypes: true })
            })
            .onAll(() => {
                let state = this.state;
                this.applyStateChanges({
                    loadingObjectTypes: false,
                    renamingObjectType: null,
                    renamingObjectTypeNewCode: '',
                    newObjectTypeCode: ''
                } as AdminObjectTypesState);
            })
            .send();

        let state = this.state;
        this.applyStateChanges({
            loadingObjectTypes: true
        } as AdminObjectTypesState);
    }

    private createUpdateObjectType(id: number, code: string): void {
        new Http()
            .onUrlPost('/Admin/Objects/ObjectTypeCreateOrUpdate')
            .onAll(() => {
                this.refreshObjectTypes()
            })
            .send({
                id: id,
                code: code
            });
    }

    public objectTypeClicked(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>, objectType: ObjectType): void {
        e.preventDefault();

        this.applyStateChanges({
            selectObjectType: objectType
        } as AdminObjectTypesState);
    }

    public objectTypeRenameClicked(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>, objectType: ObjectType): void {
        e.preventDefault();

        let clonedObjectType = MergeTool.mergeDeep({}, objectType);
        this.applyStateChanges({
            renamingObjectType: clonedObjectType,
            renamingObjectTypeNewCode: objectType.code
        } as AdminObjectTypesState);
    }

    public objectTypeRenameSaveClicked(e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        e.preventDefault();

        if (this.state.renamingObjectTypeNewCode == null || this.state.renamingObjectTypeNewCode.length <= 0)
            return;

        this.createUpdateObjectType(this.state.renamingObjectType.id, this.state.renamingObjectTypeNewCode);
    }

    public objectTypeRenameCancelClicked(e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        e.preventDefault();

        this.applyStateChanges({
            renamingObjectType: null,
            renamingObjectTypeNewCode: ''
        } as AdminObjectTypesState);
    }

    public objectTypeRenameValueChanged(e: React.ChangeEvent<HTMLInputElement>): void {
        let newState = MergeTool.mergeDeep({}, this.state, { renamingObjectTypeNewCode: e.target.value } as AdminObjectTypesState);
        this.setState(newState);
    }

    public newObjectTypeValueChanged(e: React.ChangeEvent<HTMLInputElement>): void {
        let newState = MergeTool.mergeDeep({}, this.state, { newObjectTypeCode: e.target.value } as AdminObjectTypesState);
        this.setState(newState);
    }

    public createNewObjectTypeClicked(e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        e.preventDefault();

        this.createUpdateObjectType(null, this.state.newObjectTypeCode);
    }

    public fromObjectsToObjectTypesClick(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>): void {
        e.preventDefault();

        this.applyStateChanges({
            selectObjectType: null
        } as AdminObjectTypesState);
    }

    public render() {
        if (this.state.loadingObjectTypes)
            return <span>Loading...</span>;

        if (this.state.failedToLoadObjectTypes)
            return (
                <div>
                    <span>Failed to load object types.</span>
                </div>)

        if (this.state.selectObjectType == null)
            return (
                <React.Fragment>
                    <h3>Object types</h3>
                    <div className="data-block">
                        <div className="data-row">
                            <input type="text" onChange={this.newObjectTypeValueChanged} value={this.state.newObjectTypeCode} />
                            <button className="btn" onClick={this.createNewObjectTypeClicked}>Add</button>
                        </div>
                        {this.state.objectTypes.map((objectType, index) =>
                            <div className="data-row" key={objectType.id}>
                                {
                                    (this.state.renamingObjectType == null
                                        || this.state.renamingObjectType.id != objectType.id)
                                        ? (<React.Fragment>
                                            <a href="#"
                                                onClick={(e) => { this.objectTypeClicked(e, objectType) }}>
                                                <span>{`${objectType.code}`}</span>
                                            </a>
                                            <span> {`(id: ${objectType.id})`}</span>
                                            <a href="#"
                                                onClick={e => { this.objectTypeRenameClicked(e, objectType) }}>
                                                <span>   Rename</span>
                                            </a>
                                        </React.Fragment>)
                                        : (<React.Fragment>
                                            <input type="text"
                                                value={this.state.renamingObjectTypeNewCode}
                                                onChange={this.objectTypeRenameValueChanged} />
                                            <button className="btn" onClick={this.objectTypeRenameSaveClicked}>Save</button>
                                            <button className="btn" onClick={this.objectTypeRenameCancelClicked}>Cancel</button>
                                        </React.Fragment>)
                                }
                            </div>
                        )}
                    </div>
                </React.Fragment>
            )

        return (
            <div>
                <h3>Objects of type: {this.state.selectObjectType.code}</h3>
                <a href="#" onClick={this.fromObjectsToObjectTypesClick}>&larr;</a>
            </div>
        )
    }
}
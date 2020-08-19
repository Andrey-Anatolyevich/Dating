import { ViewComponent } from "../../ViewComponent";
import { PlaceInfoModel } from "../../../_cs_to_ts/areas/api/geo/placeInfoModel";
import * as React from "react";
import { Http } from "../../../tool/http";
import { MergeTool } from "../../../tool/mergeTool";
import { ObjectComparer } from "../../../tool/objectComparer";

export interface PlaceComponentProps {
    dataChanged: () => void,
    allPlaces: PlaceInfoModel[],
    currentPlaceId: number
}

export interface PlaceComponentState {
    currentPlace: PlaceInfoModel,
    placeChildren: PlaceInfoModel[],
    dataUpdateFailed: boolean
}

export class PlaceComponent extends ViewComponent<PlaceComponentProps, PlaceComponentState> {
    constructor(props) {
        super(props);

        this.DisablePlace = this.DisablePlace.bind(this);
        this.EnablePlace = this.EnablePlace.bind(this);

        this.state = this.GetNewState();
    }

    private _comparer = new ObjectComparer();

    public componentDidUpdate() {
        let newState = this.GetNewState();
        if (this._comparer.areDeepEqual(this.state, newState))
            return;

        this.setState(newState);
    }

    private GetNewState() {
        let newState = {
            currentPlace: this.props.allPlaces.find(place => place.id == this.props.currentPlaceId),
            placeChildren: this.props.allPlaces.filter(place => place.parentPlaceId == this.props.currentPlaceId),
            dataUpdateFailed: false
        };
        return newState;
    }

    private DisablePlace(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>): void {
        this.SetPlaceEnabledState(e, false);
    }
    private EnablePlace(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>): void {
        this.SetPlaceEnabledState(e, true);
    }

    private SetPlaceEnabledState(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>, isEnabled: boolean): void {
        e.preventDefault();

        new Http()
            .onUrlPost('/Admin/Geo/SetPlaceState')
            .onSuccess(response => {
                this.props.dataChanged();
            })
            .onError(response => {
                let newState = MergeTool.mergeDeep({}, this.state, {
                    dataUpdateFailed: true
                } as PlaceComponentState);
                this.setState(newState);
            })
            .send({
                placeId: this.props.currentPlaceId,
                isEnabled: isEnabled
            })
    }

    public render() {
        return <React.Fragment>
            <div className={"data-row " + (this.state.currentPlace.isEnabled ? "enabled" : "")}>
                {
                    this.state.currentPlace.isEnabled
                        ? <a href="#" onClick={this.DisablePlace}>Disable</a>
                        : <a href="#" onClick={this.EnablePlace}>Enable</a>
                }
                <span>&nbsp;&nbsp;&nbsp;</span>
                <span>{this.state.currentPlace.placeCode} ({this.state.currentPlace.placeType})</span>
            </div>
            {
                this.state.placeChildren.length <= 0
                    ? ""
                    : <div className="data-row">
                        {
                            this.state.placeChildren.map(child => <PlaceComponent key={child.id} allPlaces={this.props.allPlaces} currentPlaceId={child.id} dataChanged={this.props.dataChanged} />)
                        }
                    </div>
            }
        </React.Fragment>
    }
}
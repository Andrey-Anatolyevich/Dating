import * as React from "react";
import { Http } from "../../../tool/http";
import { PlaceInfoModel } from "../../../_cs_to_ts/areas/api/geo/placeInfoModel";
import { ViewComponent } from "../../ViewComponent";
import { PlaceComponent } from "./placeComponent";

interface AdminPlacesState {
    allPlaces: PlaceInfoModel[];
    loadingFailed: boolean;
}

export class AdminPlacesList extends ViewComponent<{}, AdminPlacesState> {
    constructor(props) {
        super(props);

        this.refreshData = this.refreshData.bind(this);

        this.state = {
            allPlaces: [],
            loadingFailed: false
        };
    }

    public componentDidMount() {
        this.refreshData();
    }

    private refreshData(): void {
        new Http()
            .onUrlGet('/Admin/Geo/GetPlaces')
            .onSuccess((response) => {
                var places = JSON.parse(response) as PlaceInfoModel[];
                this.setState({ allPlaces: places, loadingFailed: false });
            })
            .onError(response => {
                this.setState({ loadingFailed: true })
            })
            .send();
    }

    public render() {
        return (
            <React.Fragment>
                <h3>Places. Total: {this.state.allPlaces.length}</h3>

                <div className="data-block">
                    {
                        this.state.loadingFailed
                            ? <p>Failed to load places.</p>
                            : this.state.allPlaces
                                .filter(item => item.parentPlaceId == null)
                                .map((item, index) => {
                                    return <PlaceComponent key={item.id} allPlaces={this.state.allPlaces} currentPlaceId={item.id} dataChanged={this.refreshData} />
                                })
                    }
                </div>
            </React.Fragment>
        );
    }
}
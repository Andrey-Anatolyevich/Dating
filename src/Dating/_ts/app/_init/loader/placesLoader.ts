import { Http } from "../../../tool/http";
import { ActionFactory } from "../../state/action/actionFactory";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";

export class PlacesLoader {
    public load(): Promise<string> {

        let loadDataPromise = new Http()
            .onUrlGet('/DataApi/GetEnabledPlaces')
            .onSuccess((data) => {
                let locations = JSON.parse(data) as LocationInfo[];
                let locationsLoadedAction = ActionFactory.LocationsLoaded(locations);
                ReduxAppStateStore.dispatch(locationsLoadedAction);

                return Promise.resolve();
            })
            .onError((response) => {
                console.error(response);

                return Promise.reject(response);
            })
            .send();

        return loadDataPromise;
    }
}
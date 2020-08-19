import { ActionKey } from "../../../action/actionKey";
import { LocationsLoadedAction } from "../../../action/interfaces/_init/locationsLoadedAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class LocationsLoadedHandler implements IActionHandler {
    actionType: string = ActionKey.locationsLoaded;

    getStateDeltaFromAction(lastState: AppState, action: LocationsLoadedAction): AppState {
        let newStateDelta = {
            locations: action.locations,
            dating: {
                filter: {
                    country: action.locations[0],
                    city: action.locations[0].Children[0]
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
import { ActionKey } from "../../actionKey";

export interface LocationsLoadedAction {
    type: ActionKey.locationsLoaded,
    locations: Array<LocationInfo>
}
import { ActionKey } from "../../../action/actionKey";
import { AgeRangeLoadedAction } from "../../../action/interfaces/_init/ageRangeLoadedAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class AgeRangeLoadedHandler implements IActionHandler {
    actionType: string = ActionKey.ageRangeLoaded;

    getStateDeltaFromAction(lastState: AppState, action: AgeRangeLoadedAction): AppState {
        let newStateDelta = {
            dating: {
                filter: {
                    age: {
                        range: action.ageRange,
                        currentMin: action.ageRange.ageMin,
                        currentMax: action.ageRange.ageMax
                    }
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
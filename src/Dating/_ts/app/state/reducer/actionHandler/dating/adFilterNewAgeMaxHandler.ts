import { ActionKey } from "../../../action/actionKey";
import { AdFilterNewAgeMaxAction } from "../../../action/interfaces/dating/adFilterNewAgeMaxAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class AdFilterNewAgeMaxHandler implements IActionHandler {
    actionType: string = ActionKey.adFilterNewAgeMax;

    getStateDeltaFromAction(lastState: AppState, action: AdFilterNewAgeMaxAction): AppState {
        let newAgeMin = Math.min(lastState.dating.filter.age.currentMin, action.newAgeMax);

        let newStateDelta = {
            dating: {
                filter: {
                    age: {
                        currentMin: newAgeMin,
                        currentMax: action.newAgeMax
                    }
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
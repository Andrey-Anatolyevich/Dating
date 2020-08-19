import { ActionKey } from "../../../action/actionKey";
import { AdFilterNewAgeMinAction } from "../../../action/interfaces/dating/adFilterNewAgeMinAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class AdFilterNewAgeMinHandler implements IActionHandler {
    actionType: string = ActionKey.adFilterNewAgeMin;

    getStateDeltaFromAction(lastState: AppState, action: AdFilterNewAgeMinAction): AppState {
        let newAgeMax = Math.max(lastState.dating.filter.age.currentMax, action.newAgeMin);

        let newStateDelta = {
            dating: {
                filter: {
                    age: {
                        currentMin: action.newAgeMin,
                        currentMax: newAgeMax
                    }
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
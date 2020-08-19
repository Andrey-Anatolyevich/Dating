import { ActionKey } from "../../../action/actionKey";
import { AdFilterCitySelectAction } from "../../../action/interfaces/dating/adFilterCitySelectAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class AdFilterNewCitySelectHandler implements IActionHandler {
    actionType: string = ActionKey.adFilterCitySelect;

    getStateDeltaFromAction(lastState: AppState, action: AdFilterCitySelectAction): AppState {
        let newStateDelta = {
            dating: {
                filter: {
                    city: action.newCity
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
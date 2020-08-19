import { ActionKey } from "../../../action/actionKey";
import { AdFilterCountrySelectAction } from "../../../action/interfaces/dating/adFilterCountrySelectAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class AdFilterNewCountrySelectHandler implements IActionHandler {
    actionType: string = ActionKey.adFilterCountrySelect;

    getStateDeltaFromAction(lastState: AppState, action: AdFilterCountrySelectAction): AppState {
        let newStateDelta = {
            dating: {
                filter: {
                    country: action.newCountry,
                    city: action.newCountry.Children[0]
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
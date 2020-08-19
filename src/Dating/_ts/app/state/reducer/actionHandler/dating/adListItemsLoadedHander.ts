import { ActionKey } from "../../../action/actionKey";
import { AdListItemsLoadedAction } from "../../../action/interfaces/dating/adListItemsLoadedAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class AdListItemsLoadedHandler implements IActionHandler {
    actionType: string = ActionKey.adListItemsLoaded;

    getStateDeltaFromAction(lastState: AppState, action: AdListItemsLoadedAction): AppState {
        let newStateDelta = {
            dating: {
                ads: {
                    isLoading: false,
                    items: action.items
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
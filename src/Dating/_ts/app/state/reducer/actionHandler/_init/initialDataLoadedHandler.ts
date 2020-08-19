import { ActionKey } from "../../../action/actionKey";
import { InitialDataLoadedAction } from "../../../action/interfaces/_init/initialDataLoadedAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class InitialDataLoadedHandler implements IActionHandler {
    actionType: string = ActionKey.initialDataLoaded;

    getStateDeltaFromAction(lastState: AppState, action: InitialDataLoadedAction): AppState {
        let newStateDelta = {
            initialLoadingComplete: true
        } as AppState;
        return newStateDelta;
    }
}
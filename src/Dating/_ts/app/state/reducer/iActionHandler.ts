import { ActionTypes } from "../action/actionTypes";
import { AppState } from "../appState";

export interface IActionHandler {
    actionType: string;
    getStateDeltaFromAction(lastState: AppState, action: ActionTypes): AppState;
}
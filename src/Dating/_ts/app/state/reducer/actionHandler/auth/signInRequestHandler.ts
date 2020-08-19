import { ActionKey } from "../../../action/actionKey";
import { SignInRequestAction } from "../../../action/interfaces/auth/signInRequestAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class SignInRequestHandler implements IActionHandler {
    actionType: string = ActionKey.signInRequest;

    getStateDeltaFromAction(lastState: AppState, action: SignInRequestAction): AppState {
        let newStateDelta = {
            auth: {
                signInInProgress: true
            }
        } as AppState;
        return newStateDelta;
    }
}
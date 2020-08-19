import { ActionKey } from "../../../action/actionKey";
import { UserSignedOutAction } from "../../../action/interfaces/auth/userSignedOutAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class UserSignedOutHandler implements IActionHandler {
    actionType: string = ActionKey.userSignedOut;

    getStateDeltaFromAction(lastState: AppState, action: UserSignedOutAction): AppState {
        let newStateDelta = {
            auth: {
                signInInProgress: false,
                userSignedIn: false,
                currentUser: {}
            }
        } as AppState;
        return newStateDelta;
    }
}
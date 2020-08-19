import { ActionKey } from "../../../action/actionKey";
import { UserSignedInAction } from "../../../action/interfaces/auth/userSignedInAction";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class UserSignedInHandler implements IActionHandler {
    actionType: string = ActionKey.userSignedIn;

    getStateDeltaFromAction(lastState: AppState, action: UserSignedInAction): AppState {
        let newStateDelta = {
            auth: {
                signInInProgress: false,
                userSignedIn: true,
                currentUser: {
                    email: action.signInResponse.email,
                    login: action.signInResponse.login,
                    localeCode: action.signInResponse.localeCode,
                    chosenPlaceId: action.signInResponse.chosenPlaceId,
                    claims: action.signInResponse.claims
                }
            }
        } as AppState;
        return newStateDelta;
    }
}
import { ActionKey } from "../../actionKey";

export interface UserSignedInAction {
    type: ActionKey.userSignedIn,
    signInResponse: SignInResponseModel
}

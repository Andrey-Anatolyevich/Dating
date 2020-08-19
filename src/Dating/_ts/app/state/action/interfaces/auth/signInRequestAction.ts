import { ActionKey } from "../../actionKey";

export interface SignInRequestAction {
    type: ActionKey.signInRequest,
    login: string,
    pass: string
}
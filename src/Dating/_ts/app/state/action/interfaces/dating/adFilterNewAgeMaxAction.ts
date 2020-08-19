import { ActionKey } from "../../actionKey";

export interface AdFilterNewAgeMaxAction {
    type: ActionKey.adFilterNewAgeMax,
    newAgeMax: number
}
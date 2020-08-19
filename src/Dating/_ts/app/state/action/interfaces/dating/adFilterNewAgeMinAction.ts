import { ActionKey } from "../../actionKey";

export interface AdFilterNewAgeMinAction {
    type: ActionKey.adFilterNewAgeMin,
    newAgeMin: number
}
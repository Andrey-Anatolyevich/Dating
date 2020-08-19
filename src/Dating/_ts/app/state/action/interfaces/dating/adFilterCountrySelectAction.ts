import { ActionKey } from "../../actionKey";

export interface AdFilterCountrySelectAction {
    type: ActionKey.adFilterCountrySelect,
    newCountry: LocationInfo
}